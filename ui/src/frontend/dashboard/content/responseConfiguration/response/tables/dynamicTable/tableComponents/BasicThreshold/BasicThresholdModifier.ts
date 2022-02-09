import { cloneDeep, findIndex } from "lodash";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { BasicThresholdData, Modifier } from "@Palavyr-Types";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { sortByPropertyNumeric } from "@common/utils/sorting";

export class BasicThresholdModifier implements Modifier {
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

    async addThreshold(tableData: BasicThresholdData[], areaIdentifier: string, tableId: string, repository: PalavyrRepository) {
        const newRowTemplate = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate<BasicThresholdData>(areaIdentifier, this.tableType, tableId);
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

    checkTriggerFallbackChange(tableData: BasicThresholdData[], row: BasicThresholdData, checked: boolean) {
        tableData.forEach((x: BasicThresholdData) => {
            const index = findIndex(tableData, (x: BasicThresholdData) => x.rowId === row.rowId);
            if (x.rowId == row.rowId) {
                tableData[index].triggerFallback = checked;
            } else {
                tableData[index].triggerFallback = false;
            }
        });
        this.setTables(tableData);
    }

    public reorderThresholdData(tableData: BasicThresholdData[]) {
        const getter = (x: BasicThresholdData) => x.threshold;
        const sortedByThreshold = sortByPropertyNumeric(getter, tableData);

        const reOrdered: BasicThresholdData[] = [];
        let shouldReassignTriggerFallback = false;
        sortedByThreshold.forEach((row: BasicThresholdData, newRowNumber: number) => {
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

    public validateTable(tableData: BasicThresholdData[]) {
        const tableRows = this.reorderThresholdData(tableData);
        const isValid = true;

        return { isValid, tableRows };
    }
}
