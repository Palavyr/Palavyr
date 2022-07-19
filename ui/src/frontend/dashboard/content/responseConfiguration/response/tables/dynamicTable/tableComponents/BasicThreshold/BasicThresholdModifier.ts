import { cloneDeep, findIndex } from "lodash";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { BasicThresholdResource, Modifier } from "@Palavyr-Types";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { sortByPropertyNumeric } from "@common/utils/sorting";

export class BasicThresholdModifier implements Modifier {
    onClick: Dispatch<SetStateAction<BasicThresholdResource[]>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<BasicThresholdResource[]>>) {
        this.onClick = onClick;
        this.tableType = PricingStrategyTypes.BasicThreshold;
    }

    setTables(newState: BasicThresholdResource[]) {
        this.onClick(cloneDeep(newState));
    }

    removeRow(tableData: BasicThresholdResource[], rowId: number) {
        const filteredRows = tableData.filter((x: BasicThresholdResource) => x.rowId !== rowId);
        if (filteredRows.length < 1) {
            alert("Basic Threshold tables must contain at least one row.");
        } else {
            this.setTables(filteredRows);
        }
    }

    async addThreshold(tableData: BasicThresholdResource[], intentId: string, tableId: string, repository: PalavyrRepository) {
        const newRowTemplate = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<BasicThresholdResource>(intentId, this.tableType, tableId);
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    setThresholdValue(tableData: BasicThresholdResource[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdResource) => x.rowId === rowId);
        tableData[index].threshold = newValue;
        this.setTables(tableData);
    }

    setValueMin(tableData: BasicThresholdResource[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdResource) => x.rowId === rowId);
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setValueMax(tableData: BasicThresholdResource[], rowId: number, newValue: number) {
        const index = findIndex(tableData, (x: BasicThresholdResource) => x.rowId === rowId);
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: BasicThresholdResource[], rowId: number) {
        const index = findIndex(tableData, (x: BasicThresholdResource) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    setItemName(tableData: BasicThresholdResource[], value: string) {
        for (var i: number = 0; i < tableData.length; i++) {
            tableData[i].itemName = value;
        }
        this.setTables(tableData);
    }

    checkTriggerFallbackChange(tableData: BasicThresholdResource[], row: BasicThresholdResource, checked: boolean) {
        tableData.forEach((x: BasicThresholdResource) => {
            const index = findIndex(tableData, (x: BasicThresholdResource) => x.rowId === row.rowId);
            if (x.rowId == row.rowId) {
                tableData[index].triggerFallback = checked;
            } else {
                tableData[index].triggerFallback = false;
            }
        });
        this.setTables(tableData);
    }

    public reorderThresholdData(tableData: BasicThresholdResource[]) {
        const getter = (x: BasicThresholdResource) => x.threshold;
        const sortedByThreshold = sortByPropertyNumeric(getter, tableData);

        const reOrdered: BasicThresholdResource[] = [];
        let shouldReassignTriggerFallback = false;
        sortedByThreshold.forEach((row: BasicThresholdResource, newRowNumber: number) => {
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

    public validateTable(tableData: BasicThresholdResource[]) {
        const tableRows = this.reorderThresholdData(tableData);
        const isValid = true;

        return { isValid, tableRows };
    }
}
