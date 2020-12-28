import { Dispatch, SetStateAction } from "react";
import { DynamicTableTypes, PercentOfThresholdData, TableData } from "../../DynamicTableTypes";
import { cloneDeep, findIndex } from "lodash";
import { ApiClient } from "@api-client/Client";

export class PercentOfThresholdModifier {
    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.SelectOneFlat;
    }

    setTables(newState: TableData) {
        this.onClick(cloneDeep(newState));
    }

    async addItem(tableData: PercentOfThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        const response = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const newItemInitialrow = response.data as PercentOfThresholdData;

        tableData.push(newItemInitialrow);
        this.setTables(tableData);
    }

    async addRow(tableData: PercentOfThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string, itemId: string) {
        const response  = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);

        const newRow = response.data as PercentOfThresholdData;
        newRow.itemId = itemId;

        tableData.push(newRow);
        this.setTables(tableData);
    }

    removeRow(tableData: PercentOfThresholdData[], rowId: string) {
        const curRow = tableData.filter(x => x.rowId === rowId)[0];
        const itemId = curRow.itemId;

        if (tableData.filter(x => x.itemId === itemId).length > 1) {
            const rows = tableData.filter((x) => x.rowId !== rowId);
            this.setTables(rows);
        } else {
            alert("Table must have at least one option.");
        }
    }

    setThresholdValue(tableData: PercentOfThresholdData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].threshold = newValue;
        this.setTables(tableData);
    }

    setValueMin(tableData: PercentOfThresholdData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setValueMax(tableData: PercentOfThresholdData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setPercentToModify(tableData: PercentOfThresholdData[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].modifier = newValue;
        this.setTables(tableData);
    }

    setAddOrSubtract(tableData: PercentOfThresholdData[], rowId: string) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].posNeg = !tableData[index].posNeg;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: PercentOfThresholdData[], rowId: string) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }
}
