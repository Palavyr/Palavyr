import { cloneDeep, findIndex } from "lodash";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { BasicThresholdData } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

export class BasicThresholdModifier {
    onClick: Dispatch<SetStateAction<BasicThresholdData[]>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<BasicThresholdData[]>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.BasicThreshold;
    }

    setTables(newState: BasicThresholdData[]) {
        this.onClick(cloneDeep(newState));
    }

    removeRow(tableData: BasicThresholdData[], rowId: number) {
        const filteredRows = tableData.filter((x: BasicThresholdData) => x.rowId !== rowId);
        if (filteredRows.length < 1) {
            alert("Basic Threshold tables must contain at least one row.");
        } else {
            this.setTables(filteredRows);
        }
    }

    async addThreshold(tableData: BasicThresholdData[], areaIdentifier: string, tableId: string, client: ApiClient) {
        const { data: newRowTemplate } = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    setThresholdValue(tableData: BasicThresholdData[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdData) => x.rowId === rowId);
        tableData[index].threshold = newValue;
        this.setTables(tableData);
    }

    setValueMin(tableData: BasicThresholdData[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdData) => x.rowId === rowId);
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setValueMax(tableData: BasicThresholdData[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdData) => x.rowId === rowId);
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: BasicThresholdData[], rowId: number) {
        const index = findIndex(tableData, (x: BasicThresholdData) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    setItemName(tableData: BasicThresholdData[], value: string) {
        for (var i: number = 0; i < tableData.length; i++) {
            tableData[i].itemName = value;
        }
        this.setTables(tableData);
    }

    public validateTable(tableData: BasicThresholdData[]){
        return true; // TODO: Validate this table
    }
}
