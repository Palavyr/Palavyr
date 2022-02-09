import { Dispatch, SetStateAction } from "react";
import { Modifier, PercentOfThresholdData, SetState } from "@Palavyr-Types";
import { cloneDeep, findIndex, uniq, uniqBy } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { sortByPropertyNumeric } from "@common/utils/sorting";

export class PercentOfThresholdModifier implements Modifier {
    onClick: SetState<PercentOfThresholdData[]>;
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

    async addItem(tableData: PercentOfThresholdData[], repository: PalavyrRepository, areaIdentifier: string, tableId: string) {
        const newItemInitialrow = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate<PercentOfThresholdData>(areaIdentifier, this.tableType, tableId);
        newItemInitialrow.itemOrder = this._getOrderedUniqItemIds(tableData).length;
        tableData.push(newItemInitialrow);
        this.setTables(tableData);
    }

    async addRow(tableData: PercentOfThresholdData[], repository: PalavyrRepository, areaIdentifier: string, tableId: string, itemId: string) {
        const newRowTemplate = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate<PercentOfThresholdData>(areaIdentifier, this.tableType, tableId);
        newRowTemplate.itemId = itemId;
        tableData.push(newRowTemplate);
        this.setTables(tableData);
    }

    removeRow(tableData: PercentOfThresholdData[], rowId: string) {
        const curRow = tableData.filter(x => x.rowId === rowId)[0];
        const itemId = curRow.itemId;

        if (tableData.filter(x => x.itemId === itemId).length > 1) {
            const rows = tableData.filter(x => x.rowId !== rowId);
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
        tableData.forEach(x => itemIds.push(x.itemId));

        const unique = uniq(itemIds);
        if (unique.length > 1) {
            const updatedTable = tableData.filter((x: PercentOfThresholdData) => x.itemId !== itemId);
            this.setTables(updatedTable);
        } else {
            alert("Table must have at least one item.");
        }
    }

    checkTriggerFallbackChange(tableData: PercentOfThresholdData[], itemData: PercentOfThresholdData[], row: PercentOfThresholdData, checked: boolean) {
        itemData.forEach((x: PercentOfThresholdData) => {
            const index = findIndex(tableData, (x: PercentOfThresholdData) => x.rowId === row.rowId);
            if (x.rowId == row.rowId) {
                tableData[index].triggerFallback = checked;
            } else {
                tableData[index].triggerFallback = false;
            }
        });

        this.setTables(tableData);
    }

    public itemOrderGetter(x: PercentOfThresholdData) {
        return x.itemOrder;
    }

    _getOrderedUniqItemIds(tableData: PercentOfThresholdData[]) {
        return sortByPropertyNumeric(this.itemOrderGetter, uniq(tableData.map((x: PercentOfThresholdData) => x.itemId)));
    }

    _getRowsByItemId(tableData: PercentOfThresholdData[], itemId: string) {
        return tableData.filter((x: PercentOfThresholdData) => x.itemId === itemId);
    }

    public reorderThresholdData(tableData: PercentOfThresholdData[]) {
        const itemIds: string[] = this._getOrderedUniqItemIds(tableData);
        const reOrderedData: PercentOfThresholdData[] = [];

        for (let index = 0; index < itemIds.length; index++) {
            const itemId = itemIds[index];
            const itemRows = this._getRowsByItemId(tableData, itemId);
            const reorderedItem = this._reorderSingleItemThresholdData(itemRows);
            reOrderedData.push(...reorderedItem);
        }
        return reOrderedData;
    }

    _thresholdGetter(row: PercentOfThresholdData) {
        return row.threshold;
    }
    _reorderSingleItemThresholdData(itemRows: PercentOfThresholdData[]) {
        const sortedByThreshold = sortByPropertyNumeric(this._thresholdGetter, itemRows) as PercentOfThresholdData[];

        const reOrdered: PercentOfThresholdData[] = [];
        let shouldReassignTriggerFallback = false;
        sortedByThreshold.forEach((row: PercentOfThresholdData, newRowNumber: number) => {
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

    validateTable(tableData: PercentOfThresholdData[]) {
        const tableRows = this.reorderThresholdData(tableData);
        const isValid = true;

        return { isValid, tableRows };
    }
}
