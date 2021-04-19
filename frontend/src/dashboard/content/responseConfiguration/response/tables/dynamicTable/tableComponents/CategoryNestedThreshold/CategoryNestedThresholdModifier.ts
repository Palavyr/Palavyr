import { ApiClient } from "@api-client/Client";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { SetState, TableGroup } from "@Palavyr-Types";
import { cloneDeep, findIndex, groupBy, uniq } from "lodash";
import { uuid } from "uuidv4";
import { CategoryNestedThresholdData, DynamicTableTypes, TableData } from "../../DynamicTableTypes";

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

    async addCategory(tableData: CategoryNestedThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        const res = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const template = res.data as CategoryNestedThresholdData;

        const categoryIds = uniq(tableData.map((x: CategoryNestedThresholdData) => x.itemId));
        template.itemOrder = categoryIds.length;

        template.itemId = uuid();
        template.rowId = uuid();
        template.rowOrder = 0;

        tableData.push(template);
        this.setTables(tableData);
    }

    async addThreshold(tableData: CategoryNestedThresholdData[], categoryId: string, client: ApiClient, areaIdentifier: string, tableId: string) {
        const res = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const template = res.data as CategoryNestedThresholdData;

        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);
        template.rowOrder = categoryRows.length;
        template.itemOrder = uniq(categoryRows.map((x: CategoryNestedThresholdData) => x.itemOrder))[0];
        template.itemId = categoryId;
        template.rowId = uuid();

        tableData.push(template);

        this.setTables(tableData);
    }

    setCategoryName(tableData: CategoryNestedThresholdData[], categoryId: string, value: string) {
        tableData.forEach((item: CategoryNestedThresholdData, index: number) => {
            if (item.itemId === categoryId){
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
        const row = this._getRowById(tableData, rowId);
        const categoryId = row.itemId;
        const categoryRows = this._getRowsByCategoryId(tableData, categoryId);

        if (categoryRows.length > 1) {
            const updatedTable = tableData.filter((x: CategoryNestedThresholdData) => x.rowId !== rowId);

            const sortGetter = (x: CategoryNestedThresholdData) => x.rowOrder;
            const categoryRows = sortByPropertyNumeric(sortGetter, this._getRowsByCategoryId(updatedTable, categoryId));
            categoryRows.forEach((x: CategoryNestedThresholdData, order: number) => {
                const rowIndex = this._getRowIndexById(tableData, rowId);
                updatedTable[rowIndex].rowOrder = order;
            });
            this.setTables(updatedTable);
        } else {
            alert("Category must have at least one threshold value.");
        }
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

    __itemIdGetter(x: CategoryNestedThresholdData) {
        return x.itemOrder;
    }
    _getOrderedUniqItemIds(tableData: CategoryNestedThresholdData[]) {
        return sortByPropertyNumeric(this.__itemIdGetter, uniq(tableData.map((x: CategoryNestedThresholdData) => x.itemId)));
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
