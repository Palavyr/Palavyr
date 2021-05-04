import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { SetState, TableGroup } from "@Palavyr-Types";
import { cloneDeep, findIndex, groupBy, uniq } from "lodash";
import { uuid } from "uuidv4";
import { CategoryNestedThresholdData, TableData } from "@Palavyr-Types";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

export class CategoryNestedThresholdModifier {
    onClick: SetState<TableData>;
    tableType: string = DynamicTableTypes.CategoryNestedThreshold;

    constructor(onClick: SetState<TableData>) {
        this.onClick = onClick;
    }

    setTables(newState: CategoryNestedThresholdData[]) {
        this.onClick(cloneDeep(newState));
    }

    groupByOuterCategory(tableData: CategoryNestedThresholdData[]): TableGroup<CategoryNestedThresholdData[]> {
        return groupBy(tableData, (x) => x.itemId);
    }

    async addCategory(tableData: CategoryNestedThresholdData[], repository: PalavyrRepository, areaIdentifier: string, tableId: string) {
        const template = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);

        const categoryIds = uniq(tableData.map((x: CategoryNestedThresholdData) => x.itemId));
        template.itemOrder = categoryIds.length;

        template.itemId = uuid();
        template.rowId = uuid();
        template.rowOrder = 0;

        tableData.push(template);
        this.setTables(tableData);
    }

    async addThreshold(tableData: CategoryNestedThresholdData[], categoryId: string, repository: PalavyrRepository, areaIdentifier: string, tableId: string) {
        const template = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);

        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        template.rowOrder = categoryRows.length;
        template.itemOrder = uniq(categoryRows.map((x: CategoryNestedThresholdData) => x.itemOrder))[0];
        template.itemId = categoryId;
        template.rowId = uuid();
        template.category = categoryRows[0].category;
        tableData.push(template);

        this.setTables(tableData);
    }

    setCategoryName(tableData: CategoryNestedThresholdData[], categoryId: string, value: string) {
        tableData.forEach((item: CategoryNestedThresholdData, index: number) => {
            if (item.itemId === categoryId) {
                tableData[index].category = value;
            }
        });
        this.setTables(tableData);
    }

    setThreshold(tableData: CategoryNestedThresholdData[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdData) => x.rowId === rowId);
        tableData[index].threshold = value;
        this.setTables(tableData);
    }

    setValueMin(tableData: CategoryNestedThresholdData[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdData) => x.rowId === rowId);
        tableData[index].valueMin = value;
        this.setTables(tableData);
    }

    setValueMax(tableData: CategoryNestedThresholdData[], rowId: string, value: number) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdData) => x.rowId === rowId);
        tableData[index].valueMax = value;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: CategoryNestedThresholdData[], rowId: string) {
        const index = findIndex(tableData, (x: CategoryNestedThresholdData) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    removeThreshold(tableData: CategoryNestedThresholdData[], rowId: string) {
        if (this._categoryHasMorethanOneRow(tableData, rowId)) {
            const updatedTable = tableData.filter((x: CategoryNestedThresholdData) => x.rowId !== rowId);
            const categoryId = this._getCategoryIdFromRowId(tableData, rowId);
            const updatedCategoryRows = sortByPropertyNumeric(this.rowOrderGetter, this._getRowsByCategoryId(updatedTable, categoryId));

            updatedCategoryRows.forEach((x: CategoryNestedThresholdData, order: number) => {
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
    checkTriggerFallbackChange(tableData: CategoryNestedThresholdData[], row: CategoryNestedThresholdData, categoryId: string, checked: boolean) {
        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        categoryRows.forEach((x: CategoryNestedThresholdData) => {
            const rowIndex = this._getRowIndexById(tableData, x.rowId);
            if (x.rowId === row.rowId) {
                tableData[rowIndex].triggerFallback = checked;
            } else {
                tableData[rowIndex].triggerFallback = false;
            }
        });
        this.setTables(tableData);
    }

    _categoryHasMorethanOneRow(tableData: CategoryNestedThresholdData[], rowId: string): boolean {
        const categoryId = this._getCategoryIdFromRowId(tableData, rowId);
        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        return categoryRows.length > 1;
    }

    _getCategoryIdFromRowId(tableData: CategoryNestedThresholdData[], rowId: string) {
        const row = this._getRowById(tableData, rowId);
        return row.itemId;
    }

    removeCategory(tableData: CategoryNestedThresholdData[], categoryId: string) {
        const itemIds: string[] = this._getOrderedUniqItemIds(tableData);
        if (itemIds.length > 1) {
            const updatedTable = tableData.filter((x: CategoryNestedThresholdData) => x.itemId !== categoryId);

            const sortedItemIds: string[] = this._getOrderedUniqItemIds(tableData);
            sortedItemIds.forEach((categoryId: string, newItemOrder: number) => {
                const rows = this._getRowsByCategoryId(updatedTable, categoryId);
                rows.forEach((x: CategoryNestedThresholdData) => {
                    const index = this._getRowIndexById(updatedTable, x.rowId);
                    updatedTable[index].itemOrder = newItemOrder;
                });
            });

            this.setTables(updatedTable);
        } else {
            alert("Table must have at lease one category.");
        }
    }

    public rowOrderGetter(x: CategoryNestedThresholdData) {
        return x.rowOrder;
    }

    public categoryIdGetter(x: CategoryNestedThresholdData) {
        return x.itemOrder;
    }

    public validateTable(tableData: CategoryNestedThresholdData[]) {
        return true; // TODO: going to need to check things like row orders.
    }

    _getOrderedUniqItemIds(tableData: CategoryNestedThresholdData[]) {
        return sortByPropertyNumeric(this.categoryIdGetter, uniq(tableData.map((x: CategoryNestedThresholdData) => x.itemId)));
    }

    _getRowsByCategoryId(tableData: CategoryNestedThresholdData[], categoryId: string) {
        return tableData.filter((x: CategoryNestedThresholdData) => x.itemId === categoryId);
    }

    _getRowIndexById(tableData: CategoryNestedThresholdData[], rowId: string): number {
        return findIndex(tableData, (x: CategoryNestedThresholdData) => x.rowId === rowId);
    }
    _getRowById(tableData: CategoryNestedThresholdData[], rowId: string): CategoryNestedThresholdData {
        const index = this._getRowIndexById(tableData, rowId);
        const row = tableData[index];
        return row;
    }
}
