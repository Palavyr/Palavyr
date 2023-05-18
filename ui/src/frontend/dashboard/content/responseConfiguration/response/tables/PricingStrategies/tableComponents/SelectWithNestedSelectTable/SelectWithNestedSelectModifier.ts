import { cloneDeep, findIndex, groupBy, max, uniq } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { Modifier, SetState, TableGroup } from "@Palavyr-Types";
import { uuid } from "uuidv4";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { TableData, TwoNestedCategoryResource } from "@common/types/api/EntityResources";

export class TwoNestedCategoriesModifier implements Modifier {
    onClick: SetState<TableData>;
    tableType: string;

    constructor(onClick: SetState<TableData>) {
        this.onClick = onClick;
        this.tableType = PricingStrategyTypes.TwoNestedCategory;
    }
    setTables(newState: TwoNestedCategoryResource[]) {
        this.onClick(cloneDeep(newState));
    }

    groupByOuterCategory(tableData: TwoNestedCategoryResource[]): TableGroup<TwoNestedCategoryResource[]> {
        return groupBy(tableData, x => x.itemId);
    }

    getItemRows(tableData: TwoNestedCategoryResource[], rowId: String) {
        const index = findIndex(tableData, (x: TwoNestedCategoryResource) => x.rowId === rowId);
        return tableData[index];
    }

    async addOuterCategory(tableData: TwoNestedCategoryResource[], repository: PalavyrRepository, intentId: string, tableId: string) {
        const template = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<TwoNestedCategoryResource>(intentId, this.tableType, tableId);

        // get all current inner categories from the first category and assign them to the new one
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.

        const firstCategory = Object.keys(outerCategoryGroups)[0];
        const firstCategoryRows = outerCategoryGroups[firstCategory];
        const maxOrder = Object.keys(outerCategoryGroups).length - 1;

        const newOuterCategory = cloneDeep(firstCategoryRows);
        newOuterCategory.forEach((newRow: TwoNestedCategoryResource) => {
            newRow.itemName = "";
            newRow.itemId = template.itemId;
            newRow.itemOrder = maxOrder;
            newRow.rowId = uuid();
        });

        const newTableData = [...tableData, ...newOuterCategory];
        this.setTables(newTableData);
    }

    async addInnerCategory(tableData: TwoNestedCategoryResource[], repository: PalavyrRepository, intentId: string, tableId: string) {
        const template = (await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate(intentId, this.tableType, tableId)) as TwoNestedCategoryResource;

        // need to copy across all outer categories...
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        const newTableData: TwoNestedCategoryResource[] = [...tableData];
        const nextRowOrder = Object.values(outerCategoryGroups)[0].length;
        itemIds.forEach((itemId: string) => {
            const outerCategoryName = outerCategoryGroups[itemId][0].itemName;
            const newRowId = uuid();
            const templateCopy = cloneDeep(template);
            templateCopy.rowId = newRowId;
            templateCopy.itemId = itemId;
            templateCopy.rowOrder = nextRowOrder;
            templateCopy.itemName = outerCategoryName;

            newTableData.push(templateCopy);
        });
        this.setTables(newTableData);
    }

    removeInnerCategory(tableData: TwoNestedCategoryResource[], rowOrder: number) {
        // get row order, delete from all groups, then rewrite rowOrder for each group.
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryResource[] = [];

        const firstGroup = outerCategoryGroups[Object.keys(outerCategoryGroups)[0]]; // TODO: Get legit first group by filtering on itemOrder === 0
        if (firstGroup.length > 1) {
            itemIds.forEach((itemId: string) => {
                let group = outerCategoryGroups[itemId];
                delete group[rowOrder];
                group = group.filter((x: TwoNestedCategoryResource) => x);
                group.forEach((x: TwoNestedCategoryResource, newRowOrder: number) => {
                    x.rowOrder = newRowOrder;
                });

                updated = [...updated, ...group];
            });
            this.setTables(updated);
        } else {
            alert("Table must have at least one option.");
        }
    }

    findIndices(tableData: TwoNestedCategoryResource[], value: string) {
        const indices: number[] = [];
        tableData.forEach((x: TwoNestedCategoryResource, index: number) => {
            if (x.rowId === value) {
                indices.push(index);
            }
        });
        return indices;
    }

    setInnerCategoryName(tableData: TwoNestedCategoryResource[], rowOrder: number, newValue: string) {
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryResource[] = [];
        itemIds.forEach((itemId: string) => {
            const group = outerCategoryGroups[itemId];
            group[rowOrder].innerItemName = newValue;
            updated = [...updated, ...group];
        });
        this.setTables(updated);
    }

    setValueMin(tableData: TwoNestedCategoryResource[], rowId: string, newValue: number) {
        const indices = this.findIndices(tableData, rowId);
        indices.forEach((index: number) => {
            tableData[index].valueMin = newValue;
        });
        this.setTables(tableData);
    }

    setValueMax(tableData: TwoNestedCategoryResource[], rowId: string, newValue: number) {
        const indices = this.findIndices(tableData, rowId);
        indices.forEach((index: number) => {
            tableData[index].valueMax = newValue;
        });
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: TwoNestedCategoryResource[], rowOrder: number) {
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryResource[] = [];
        itemIds.forEach((itemId: string) => {
            const group = outerCategoryGroups[itemId];
            group[rowOrder].range = !group[rowOrder].range;
            updated = [...updated, ...group];
        });
        this.setTables(updated);
    }

    setOuterCategoryName(tableData: TwoNestedCategoryResource[], itemId: string, newName: string) {
        const itemData = tableData.filter((x: TwoNestedCategoryResource) => x.itemId === itemId);
        let indices: number[] = [];
        itemData.forEach((item: TwoNestedCategoryResource) => {
            const index = findIndex(tableData, (x: TwoNestedCategoryResource) => x.rowId === item.rowId);
            indices.push(index);
        });

        indices.forEach((idx: number) => {
            tableData[idx].itemName = newName;
        });
        this.setTables(tableData);
    }

    removeOuterCategory(tableData: TwoNestedCategoryResource[], itemId: string) {
        const itemIds: string[] = [];
        tableData.forEach(x => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1) {
            const updatedTable = tableData.filter((x: TwoNestedCategoryResource) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one outer category.");
        }
    }

    public outerCategoryOrderGetter(x: TwoNestedCategoryResource) {
        return x.itemOrder;
    }

    public innerCategoryOrderGetter(x: TwoNestedCategoryResource) {
        return x.rowOrder;
    }

    validateTable(tableData: TwoNestedCategoryResource[]) {
        const tableRows = tableData;
        const isValid = true;

        return { isValid, tableRows };
    }
}
