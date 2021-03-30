import { Dispatch, SetStateAction } from "react";
import { DynamicTableTypes, TableData, TwoNestedCategoryData } from "../../DynamicTableTypes";
import { cloneDeep, findIndex, uniq } from "lodash";
import { ApiClient } from "@api-client/Client";


export class TwoNestedCategoriesModifier {

    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.SelectOneFlat;
    }
    setTables(newState: TwoNestedCategoryData[]) {
        this.onClick(cloneDeep(newState));
    }

    getItemRows(tableData: TwoNestedCategoryData[], rowId: String){
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        return tableData[index]
    }

    async addItem(tableData: TwoNestedCategoryData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        const response = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const newItemInitialrow = response.data as TwoNestedCategoryData;

        tableData.push(newItemInitialrow);
        this.setTables(tableData);
    }

    async addRow(tableData: TwoNestedCategoryData[], client: ApiClient, areaIdentifier: string, tableId: string, itemId: string) {

        const {data: newRowTemplate} = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        newRowTemplate.itemId = itemId;
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    removeRow(tableData: TwoNestedCategoryData[], rowId: string) {
        const curRow = tableData.filter((x) => x.rowId === rowId)[0];
        const itemId = curRow.itemId;

        if (tableData.filter((x) => x.itemId === itemId).length > 1) {
            const rows = tableData.filter((x) => x.rowId !== rowId);
            this.setTables(rows);
        } else {
            alert("Table must have at least one option.");
        }
    }

    setSubCategoryName(tableData: TwoNestedCategoryData[], rowId: string, newValue: string) {
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        tableData[index].subCategory = newValue;
        this.setTables(tableData);
    }

    setValueMin(tableData: TwoNestedCategoryData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setValueMax(tableData: TwoNestedCategoryData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: TwoNestedCategoryData[], rowId: string) {
        const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    setOuterCategoryName(tableData: TwoNestedCategoryData[], itemId: string, newName: string) {
        const itemData = tableData.filter((x: TwoNestedCategoryData) => x.itemId === itemId);
        let indices: number[] = [];
        itemData.forEach((item: TwoNestedCategoryData) => {
            const index = findIndex(tableData, (x: TwoNestedCategoryData) => x.rowId === item.rowId );
            indices.push(index);
        })

        indices.forEach((idx: number) => {
            tableData[idx].category = newName;
        })
        this.setTables(tableData);
    }

    removeOuterCategory(tableData: TwoNestedCategoryData[], itemId: string){

        const itemIds: string[] = [];
        tableData.forEach(x => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1){
            const updatedTable = tableData.filter((x: TwoNestedCategoryData) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one item.")
        }

    }
}