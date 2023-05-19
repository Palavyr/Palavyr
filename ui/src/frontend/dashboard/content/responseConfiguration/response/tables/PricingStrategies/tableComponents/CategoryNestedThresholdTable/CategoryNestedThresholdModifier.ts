import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { CategoryNestedThresholdResource, TableData } from "@common/types/api/EntityResources";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Modifier, SetState, TableGroup } from "@Palavyr-Types";
import { cloneDeep, findIndex, groupBy, uniq } from "lodash";
import { uuid } from "uuidv4";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";

export class CategoryNestedThresholdModifier implements Modifier {
    onClick: SetState<TableData>;
    tableType: string = PricingStrategyTypes.CategoryNestedThreshold;

    constructor(onClick: SetState<TableData>) {
        this.onClick = onClick;
    }

    setTables(newState: CategoryNestedThresholdResource[]) {
        this.onClick(cloneDeep(newState));
    }

    groupByOuterCategory(tableData: CategoryNestedThresholdResource[]): TableGroup<CategoryNestedThresholdResource[]> {
        return groupBy(tableData, x => x.itemId);
    }

    async addCategory(tableData: CategoryNestedThresholdResource[], repository: PalavyrRepository, intentId: string, tableId: string) {
        const template = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<CategoryNestedThresholdResource>(intentId, this.tableType, tableId);

        const categoryIds = uniq(tableData.map((x: CategoryNestedThresholdResource) => x.itemId));
        template.itemOrder = categoryIds.length;

        template.itemId = uuid();
        template.rowId = uuid();
        template.rowOrder = 0;

        tableData.push(template);
        this.setTables(tableData);
    }

    async addThreshold(tableData: CategoryNestedThresholdResource[], categoryId: string, repository: PalavyrRepository, intentId: string, tableId: string) {
        const template = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<CategoryNestedThresholdResource>(intentId, this.tableType, tableId);

        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        template.rowOrder = 0;
        template.itemOrder = uniq(categoryRows.map((x: CategoryNestedThresholdResource) => x.itemOrder))[0];
        template.itemId = categoryId;
        template.rowId = uuid();
        template.itemName = categoryRows[0].itemName;
        tableData.push(template);

        this.setTables(tableData);
    }

    setCategoryName(tableData: CategoryNestedThresholdResource[], categoryId: string, value: string) {
        tableData.forEach((item: CategoryNestedThresholdResource, index: number) => {
            if (item.itemId === categoryId) {
                tableData[index].itemName = value;
            }
        });
        this.setTables(tableData);
    }

    setThreshold(tableData: CategoryNestedThresholdResource[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdResource) => x.rowId === rowId);
        tableData[index].threshold = value;
        this.setTables(tableData);
    }

    setValueMin(tableData: CategoryNestedThresholdResource[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdResource) => x.rowId === rowId);
        tableData[index].valueMin = value;
        this.setTables(tableData);
    }

    setValueMax(tableData: CategoryNestedThresholdResource[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdResource) => x.rowId === rowId);
        tableData[index].valueMax = value;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: CategoryNestedThresholdResource[], rowId: string) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdResource) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    removeThreshold(tableData: CategoryNestedThresholdResource[], rowId: string) {
        if (this._categoryHasMorethanOneRow(tableData, rowId)) {
            const updatedTable = tableData.filter((x: CategoryNestedThresholdResource) => x.rowId !== rowId);
            const categoryId = this._getCategoryIdFromRowId(tableData, rowId);
            const updatedCategoryRows = sortByPropertyNumeric(this.rowOrderGetter, this._getRowsByCategoryId(updatedTable, categoryId));

            updatedCategoryRows.forEach((x: CategoryNestedThresholdResource, order: number) => {
                const rowIndex = this._getRowIndexById(updatedTable, x.rowId);
                updatedTable[rowIndex].rowOrder = order;
                if (updatedCategoryRows.length === 1) {
                    updatedTable[rowIndex].triggerFallback = false;
                }
            });
            this.setTables(updatedTable);
        } else {
            alert("Category must have at least one threshold value.");
        }
    }

    // onchange has to set this rowId to true, and all other category row Ids to false.
    checkTriggerFallbackChange(tableData: CategoryNestedThresholdResource[], row: CategoryNestedThresholdResource, categoryId: string, checked: boolean) {
        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        categoryRows.forEach((x: CategoryNestedThresholdResource) => {
            const rowIndex = this._getRowIndexById(tableData, x.rowId);
            if (x.rowId === row.rowId) {
                tableData[rowIndex].triggerFallback = checked;
            } else {
                tableData[rowIndex].triggerFallback = false;
            }
        });
        this.setTables(tableData);
    }

    _categoryHasMorethanOneRow(tableData: CategoryNestedThresholdResource[], rowId: string): boolean {
        const categoryId = this._getCategoryIdFromRowId(tableData, rowId);
        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        return categoryRows.length > 1;
    }

    _getCategoryIdFromRowId(tableData: CategoryNestedThresholdResource[], rowId: string) {
        const row = this._getRowById(tableData, rowId);
        return row.itemId;
    }

    removeCategory(tableData: CategoryNestedThresholdResource[], categoryId: string) {
        const itemIds: string[] = this._getOrderedUniqItemIds(tableData);
        if (itemIds.length > 1) {
            const updatedTable = tableData.filter((x: CategoryNestedThresholdResource) => x.itemId !== categoryId);

            const sortedItemIds: string[] = this._getOrderedUniqItemIds(tableData);
            sortedItemIds.forEach((categoryId: string, newItemOrder: number) => {
                const rows = this._getRowsByCategoryId(updatedTable, categoryId);
                rows.forEach((x: CategoryNestedThresholdResource) => {
                    const index = this._getRowIndexById(updatedTable, x.rowId);
                    updatedTable[index].itemOrder = newItemOrder;
                });
            });

            this.setTables(updatedTable);
        } else {
            alert("Table must have at lease one category.");
        }
    }

    public rowOrderGetter(x: CategoryNestedThresholdResource) {
        return x.rowOrder;
    }

    public itemOrderGetter(x: CategoryNestedThresholdResource) {
        return x.itemOrder;
    }

    public validateTable(tableData: CategoryNestedThresholdResource[]) {
        const tableRows = this.reorderThresholdData(tableData);
        const isValid = true;

        return { isValid, tableRows };
    }

    public reorderThresholdData(tableData: CategoryNestedThresholdResource[]) {
        // reorders all threshold data for all items (categories)
        const itemIds: string[] = this._getOrderedUniqItemIds(tableData);
        const reorderedData: CategoryNestedThresholdResource[] = [];
        for (let index = 0; index < itemIds.length; index++) {
            const itemId = itemIds[index];

            const itemRows = this._getRowsByCategoryId(tableData, itemId);
            const reorderedItem = this._reorderSingleItemThresholdData(itemRows);
            reorderedData.push(...reorderedItem);
        }

        return reorderedData;
    }

    _reorderSingleItemThresholdData(itemRows: CategoryNestedThresholdResource[]) {
        // reorders the threshold data for a single item (category)
        const getter = (x: CategoryNestedThresholdResource) => x.threshold;
        const sortedByThreshold = sortByPropertyNumeric(getter, itemRows) as CategoryNestedThresholdResource[];

        const reOrdered: CategoryNestedThresholdResource[] = [];
        let shouldReassignTriggerFallback = false;
        sortedByThreshold.forEach((row: CategoryNestedThresholdResource, newRowNumber: number) => {
            row.rowOrder = newRowNumber;
            if (newRowNumber + 1 !== sortedByThreshold.length && row.triggerFallback) {
                row.triggerFallback = false;
                shouldReassignTriggerFallback = true;
            }

            if (newRowNumber + 1 === sortedByThreshold.length && shouldReassignTriggerFallback) {
                row.triggerFallback = true;
            }
            reOrdered.push(row);
        });
        return reOrdered;
    }

    _getOrderedUniqItemIds(tableData: CategoryNestedThresholdResource[]) {
        return sortByPropertyNumeric(this.itemOrderGetter, uniq(tableData.map((x: CategoryNestedThresholdResource) => x.itemId)));
    }

    _getRowsByCategoryId(tableData: CategoryNestedThresholdResource[], categoryId: string) {
        return tableData.filter((x: CategoryNestedThresholdResource) => x.itemId === categoryId);
    }

    _getRowIndexById(tableData: CategoryNestedThresholdResource[], rowId: string): number {
        return findIndex(tableData, (x: CategoryNestedThresholdResource) => x.rowId === rowId);
    }
    _getRowById(tableData: CategoryNestedThresholdResource[], rowId: string): CategoryNestedThresholdResource {
        const index = this._getRowIndexById(tableData, rowId);
        const row = tableData[index];
        return row;
    }
}
