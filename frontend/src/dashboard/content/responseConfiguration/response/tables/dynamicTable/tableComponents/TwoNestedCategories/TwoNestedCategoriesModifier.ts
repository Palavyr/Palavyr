import { Dispatch, SetStateAction } from "react";
import { DynamicTableTypes, TableData, TwoNestedCategoryData } from "../../DynamicTableTypes";
import { cloneDeep, findIndex, groupBy, max, uniq } from "lodash";
import { ApiClient } from "@api-client/Client";
import { TableGroup } from "@Palavyr-Types";
import { uuid } from "uuidv4";

export class TwoNestedCategoriesModifier {
    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.TwoNestedCategory;
    }
    setTables(newState: TwoNestedCategoryData[]) {
        this.onClick(cloneDeep(newState));
    }

    groupByOuterCategory(tableData: TwoNestedCategoryData[]): TableGroup<TwoNestedCategoryData[]> {
        return groupBy(tableData, (x) => x.itemId);
    }

    getItemRows(tableData: TwoNestedCategoryData[], rowId: String) {
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        return tableData[index];
    }

    async addOuterCategory(tableData: TwoNestedCategoryData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        let { data: template } = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        template = template as TwoNestedCategoryData;

        // get all current inner categories from the first category and assign them to the new one
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.

        const firstCategory = Object.keys(outerCategoryGroups)[0];
        const firstCategoryRows = outerCategoryGroups[firstCategory];
        const maxOrder = Object.keys(outerCategoryGroups).length - 1;

        const newOuterCategory = cloneDeep(firstCategoryRows);
        newOuterCategory.forEach((newRow: TwoNestedCategoryData) => {
            newRow.category = "";
            newRow.itemId = template.itemId;
            newRow.itemOrder = maxOrder;
            newRow.rowId = uuid();
        });

        const newTableData = [...tableData, ...newOuterCategory];
        this.setTables(newTableData);
    }

    async addInnerCategory(tableData: TwoNestedCategoryData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        const data = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const template = data.data as TwoNestedCategoryData;

        // need to copy across all outer categories...
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        const newTableData: TwoNestedCategoryData[] = [...tableData];
        const nextRowOrder = Object.values(outerCategoryGroups)[0].length;
        itemIds.forEach((itemId: string) => {
            const outerCategoryName = outerCategoryGroups[itemId][0].category;
            const newRowId = uuid();
            const templateCopy = cloneDeep(template);
            templateCopy.rowId = newRowId;
            templateCopy.itemId = itemId;
            templateCopy.rowOrder = nextRowOrder;
            templateCopy.category = outerCategoryName;

            newTableData.push(templateCopy);
        });
        this.setTables(newTableData);
    }

    removeInnerCategory(tableData: TwoNestedCategoryData[], rowOrder: number) {
        // get row order, delete from all groups, then rewrite rowOrder for each group.
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryData[] = [];

        const firstGroup = outerCategoryGroups[Object.keys(outerCategoryGroups)[0]]; // TODO: Get legit first group by filtering on itemOrder === 0
        if (firstGroup.length > 1) {
            itemIds.forEach((itemId: string) => {
                let group = outerCategoryGroups[itemId];
                delete group[rowOrder];
                group = group.filter((x: TwoNestedCategoryData) => x)
                group.forEach((x: TwoNestedCategoryData, newRowOrder: number) => {
                    x.rowOrder = newRowOrder;
                });

                updated = [...updated, ...group];
            });
            this.setTables(updated);
        } else {
            alert("Table must have at least one option.");
        }
    }

    findIndices(tableData: TwoNestedCategoryData[], value: string) {
        const indices: number[] = [];
        tableData.forEach((x: TwoNestedCategoryData, index: number) => {
            if (x.rowId === value) {
                indices.push(index);
            }
        });
        return indices;
    }

    setInnerCategoryName(tableData: TwoNestedCategoryData[], rowOrder: number, newValue: string) {
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryData[] = [];
        itemIds.forEach((itemId: string) => {
            const group = outerCategoryGroups[itemId];
            group[rowOrder].subCategory = newValue;
            updated = [...updated, ...group];
        });
        this.setTables(updated);
    }

    setValueMin(tableData: TwoNestedCategoryData[], rowId: string, newValue: number) {
        const indices = this.findIndices(tableData, rowId);
        indices.forEach((index: number) => {
            tableData[index].valueMin = newValue;
        });
        this.setTables(tableData);
    }

    setValueMax(tableData: TwoNestedCategoryData[], rowId: string, newValue: number) {
        const indices = this.findIndices(tableData, rowId);
        indices.forEach((index: number) => {
            tableData[index].valueMax = newValue;
        });
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: TwoNestedCategoryData[], rowOrder: number) {
        const outerCategoryGroups = this.groupByOuterCategory(tableData); // use this groupby method in the modifier.
        const itemIds = Object.keys(outerCategoryGroups);

        let updated: TwoNestedCategoryData[] = [];
        itemIds.forEach((itemId: string) => {
            const group = outerCategoryGroups[itemId];
            group[rowOrder].range = !group[rowOrder].range;
            updated = [...updated, ...group];
        });
        this.setTables(updated);
    }

    setOuterCategoryName(tableData: TwoNestedCategoryData[], itemId: string, newName: string) {
        const itemData = tableData.filter((x: TwoNestedCategoryData) => x.itemId === itemId);
        let indices: number[] = [];
        itemData.forEach((item: TwoNestedCategoryData) => {
            const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === item.rowId);
            indices.push(index);
        });

        indices.forEach((idx: number) => {
            tableData[idx].category = newName;
        });
        this.setTables(tableData);
    }

    removeOuterCategory(tableData: TwoNestedCategoryData[], itemId: string) {
        const itemIds: string[] = [];
        tableData.forEach((x) => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1) {
            const updatedTable = tableData.filter((x: TwoNestedCategoryData) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one item.");
        }
    }
}
