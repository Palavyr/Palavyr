import { Dispatch, SetStateAction } from "react";
import { PercentOfThresholdData, SetState } from "@Palavyr-Types";
import { cloneDeep, findIndex, uniq } from "lodash";
import { ApiClient } from "@api-client/Client";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

export class PercentOfThresholdModifier {
    onClick: Dispatch<SetStateAction<PercentOfThresholdData[]>>;
    tableType: string;

    constructor(onClick: SetState<PercentOfThresholdData[]>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.PercentOfThreshold;
    }

    setTables(newState: PercentOfThresholdData[]) {
        this.onClick(cloneDeep(newState));
    }

    getItemRows(tableData: PercentOfThresholdData[], rowId: String) {
        const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === rowId);
        return tableData[index];
    }

    async addItem(tableData: PercentOfThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        const response = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        const newItemInitialrow = response.data as PercentOfThresholdData;

        tableData.push(newItemInitialrow);
        this.setTables(tableData);
    }

    async addRow(tableData: PercentOfThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string, itemId: string) {
        const { data: newRowTemplate } = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        newRowTemplate.itemId = itemId;
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    removeRow(tableData: PercentOfThresholdData[], rowId: string) {
        const curRow = tableData.filter((x) => x.rowId === rowId)[0];
        const itemId = curRow.itemId;

        if (tableData.filter((x) => x.itemId === itemId).length > 1) {
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

    setItemName(tableData: PercentOfThresholdData[], itemId: string, newName: string) {
        const itemData = tableData.filter((x: PercentOfThresholdData) => x.itemId === itemId);
        let indices: number[] = [];
        itemData.forEach((item: PercentOfThresholdData) => {
            const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === item.rowId);
            indices.push(index);
        });

        indices.forEach((idx: number) => {
            tableData[idx].itemName = newName;
        });
        this.setTables(tableData);
    }

    removeItem(tableData: PercentOfThresholdData[], itemId: string) {
        const itemIds: string[] = [];
        tableData.forEach((x) => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1) {
            const updatedTable = tableData.filter((x: PercentOfThresholdData) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one item.");
        }
    }

    validateTable(tableData: PercentOfThresholdData[]) {
        return true; // TODO: validation logic.
    }
}
