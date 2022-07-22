import { Dispatch, SetStateAction } from "react";
import { Modifier, PercentOfThresholdResource, SetState } from "@Palavyr-Types";
import { cloneDeep, findIndex, uniq, uniqBy } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { sortByPropertyNumeric } from "@common/utils/sorting";

export class PercentOfThresholdModifier implements Modifier {
    onClick: SetState<PercentOfThresholdResource[]>;
    tableType: string;

    constructor(onClick: SetState<PercentOfThresholdResource[]>) {
        this.onClick = onClick;
        this.tableType = PricingStrategyTypes.PercentOfThreshold;
    }

    setTables(newState: PercentOfThresholdResource[]) {
        this.onClick(cloneDeep(newState));
    }

    getItemRows(tableData: PercentOfThresholdResource[], rowId: String) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        return tableData[index];
    }

    async addItem(tableData: PercentOfThresholdResource[], repository: PalavyrRepository, intentId: string, tableId: string) {
        const newItemInitialrow = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<PercentOfThresholdResource>(intentId, this.tableType, tableId);
        newItemInitialrow.itemOrder = this._getOrderedUniqItemIds(tableData).length;
        tableData.push(newItemInitialrow);
        this.setTables(tableData);
    }

    async addRow(tableData: PercentOfThresholdResource[], repository: PalavyrRepository, intentId: string, tableId: string, itemId: string) {
        const newRowTemplate = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<PercentOfThresholdResource>(intentId, this.tableType, tableId);
        newRowTemplate.itemId = itemId;
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    removeRow(tableData: PercentOfThresholdResource[], rowId: string) {
        const curRow = tableData.filter(x => x.rowId === rowId)[0];
        const itemId = curRow.itemId;

        if (tableData.filter(x => x.itemId === itemId).length > 1) {
            const rows = tableData.filter(x => x.rowId !== rowId);
            this.setTables(rows);
        } else {
            alert("Table must have at least one option.");
        }
    }

    setThresholdValue(tableData: PercentOfThresholdResource[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].threshold = newValue;
        this.setTables(tableData);
    }

    setValueMin(tableData: PercentOfThresholdResource[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setValueMax(tableData: PercentOfThresholdResource[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setPercentToModify(tableData: PercentOfThresholdResource[], rowId: string, newValue: number) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].modifier = newValue;
        this.setTables(tableData);
    }

    setAddOrSubtract(tableData: PercentOfThresholdResource[], rowId: string) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].posNeg = !tableData[index].posNeg;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: PercentOfThresholdResource[], rowId: string) {
        const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === rowId);
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    setItemName(tableData: PercentOfThresholdResource[], itemId: string, newName: string) {
        const itemData = tableData.filter((x: PercentOfThresholdResource) => x.itemId === itemId);
        let indices: number[] = [];
        itemData.forEach((item: PercentOfThresholdResource) => {
            const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === item.rowId);
            indices.push(index);
        });

        indices.forEach((idx: number) => {
            tableData[idx].itemName = newName;
        });
        this.setTables(tableData);
    }

    removeItem(tableData: PercentOfThresholdResource[], itemId: string) {
        const itemIds: string[] = [];
        tableData.forEach(x => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1) {
            const updatedTable = tableData.filter((x: PercentOfThresholdResource) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one item.");
        }
    }

    checkTriggerFallbackChange(tableData: PercentOfThresholdResource[], itemData: PercentOfThresholdResource[], row: PercentOfThresholdResource, checked: boolean) {
        itemData.forEach((x: PercentOfThresholdResource) => {
            const index = findIndex(tableData, (x: PercentOfThresholdResource) => x.rowId === row.rowId);
            if (x.rowId == row.rowId) {
                tableData[index].triggerFallback = checked;
            } else {
                tableData[index].triggerFallback = false;
            }
        });

        this.setTables(tableData);
    }

    public itemOrderGetter(x: PercentOfThresholdResource) {
        return x.itemOrder;
    }

    _getOrderedUniqItemIds(tableData: PercentOfThresholdResource[]) {
        return sortByPropertyNumeric(this.itemOrderGetter, uniq(tableData.map((x: PercentOfThresholdResource) => x.itemId)));
    }

    _getRowsByItemId(tableData: PercentOfThresholdResource[], itemId: string) {
        return tableData.filter((x: PercentOfThresholdResource) => x.itemId === itemId);
    }

    public reorderThresholdData(tableData: PercentOfThresholdResource[]) {
        const itemIds: string[] = this._getOrderedUniqItemIds(tableData);
        const reOrderedData: PercentOfThresholdResource[] = [];

        for (let index = 0; index < itemIds.length; index++) {
            const itemId = itemIds[index];
            const itemRows = this._getRowsByItemId(tableData, itemId);
            const reorderedItem = this._reorderSingleItemThresholdData(itemRows);
            reOrderedData.push(...reorderedItem);
        }
        return reOrderedData;
    }

    _thresholdGetter(row: PercentOfThresholdResource) {
        return row.threshold;
    }
    _reorderSingleItemThresholdData(itemRows: PercentOfThresholdResource[]) {
        const sortedByThreshold = sortByPropertyNumeric(this._thresholdGetter, itemRows) as PercentOfThresholdResource[];

        const reOrdered: PercentOfThresholdResource[] = [];
        let shouldReassignTriggerFallback = false;
        sortedByThreshold.forEach((row: PercentOfThresholdResource, newRowNumber: number) => {
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

    validateTable(tableData: PercentOfThresholdResource[]) {
        const tableRows = this.reorderThresholdData(tableData);
        const isValid = true;

        return { isValid, tableRows };
    }
}
