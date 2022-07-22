import { StaticTableMetaResources, StaticTableRowResources, StaticTableRowResource, StaticTableMetaResource, AnyVoidFunction, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";

export class StaticTablesModifier {
    onClick: SetState<StaticTableMetaResources> | AnyVoidFunction;
    repository: PalavyrRepository;

    constructor(onClick: SetState<StaticTableMetaResources> | AnyVoidFunction, repository: PalavyrRepository) {
        this.onClick = onClick;
        this.repository = repository;
    }

    setTableMetas(newState: StaticTableMetaResources) {
        this.onClick(cloneDeep(newState));
    }

    _getIDs_(metas: StaticTableMetaResources) {
        return metas.map(meta => meta.tableOrder).sort();
    }

    _getrowOrders_(list: StaticTableRowResources) {
        return list.map((row: StaticTableRowResource) => row.rowOrder).sort();
    }

    _generateNextId_(ids: Array<number>) {
        return ids.length;
    }

    _rectifyIDs_(list: StaticTableMetaResources): StaticTableMetaResources {
        const rectifiedList: StaticTableMetaResources = [];
        list.forEach((table, newIdx) => {
            table.tableOrder = newIdx;
            rectifiedList.push(table);
        });
        return rectifiedList;
    }

    _rectifyrowOrders_(list: StaticTableRowResources): StaticTableRowResources {
        const rectifiedList: StaticTableRowResources = [];
        list.forEach((table, newIdx) => {
            table.rowOrder = newIdx;
            rectifiedList.push(table);
        });
        return rectifiedList;
    }

    setTableDescription(staticTableMetas: StaticTableMetaResources, tableOrder: number, description: string) {
        staticTableMetas[tableOrder].description = description;
        this.setTableMetas(staticTableMetas);
    }

    setRowDescription(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number, description: string) {
        staticTableMetas[tableOrder].staticTableRowResources[rowOrder].description = description;
        this.setTableMetas(staticTableMetas);
    }

    moveTableDown(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        const ids = this._getIDs_(staticTableMetas);
        const lastID = ids[ids.length - 1];

        if (lastID === tableOrder) {
            return false;
        } else {
            staticTableMetas[tableOrder].tableOrder++;

            const nextTableIndex = tableOrder + 1;
            staticTableMetas[nextTableIndex].tableOrder--;

            staticTableMetas = staticTableMetas.sort((a, b) => a.tableOrder - b.tableOrder);

            this.setTableMetas(staticTableMetas);
            console.log("Moving down.");
        }
    }

    moveTableUp(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        if (tableOrder === 0) {
            return false;
        } else {
            staticTableMetas[tableOrder].tableOrder--;

            const nextTableIndex = tableOrder - 1;
            staticTableMetas[nextTableIndex].tableOrder++;

            staticTableMetas = staticTableMetas.sort((a, b) => a.tableOrder - b.tableOrder);
            this.setTableMetas(staticTableMetas);
        }
    }

    async addTable(staticTableMetas: StaticTableMetaResources, repository: PalavyrRepository, intentId: string) {
        const tableOrders = this._getIDs_(staticTableMetas);
        const newtableOrder = this._generateNextId_(tableOrders);

        const newTableTemplate = await repository.Configuration.Tables.Static.GetStaticTablesMetaTemplate();
        const newTable = ((): StaticTableMetaResource => ({
            ...newTableTemplate,
            tableOrder: newtableOrder,
            intentId: intentId,
        }))();

        staticTableMetas.push(newTable);
        this.setTableMetas(staticTableMetas);
    }

    delTable(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        staticTableMetas = staticTableMetas.filter(t => t.tableOrder !== tableOrder);
        staticTableMetas = this._rectifyIDs_(staticTableMetas);
        this.setTableMetas(staticTableMetas);
    }

    async addRow(staticTableMetas: StaticTableMetaResources, tableId: string) {
        const table = staticTableMetas.filter(t => t.tableId === tableId)[0];
        const tableRows = table.staticTableRowResources;

        const rowOrders = this._getrowOrders_(tableRows);
        const nextrowOrder = this._generateNextId_(rowOrders);

        const curTableId = table.tableId;

        const newRow = await this.repository.Configuration.Tables.Static.GetStaticTableRowTemplate(table.intentId, curTableId);

        newRow.intentId = table.intentId;
        newRow.rowOrder = nextrowOrder;
        newRow.tableOrder = table.tableOrder;

        table.staticTableRowResources.push(newRow);

        this.setTableMetas(staticTableMetas);
    }

    delRow(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        staticTableMetas[tableOrder].staticTableRowResources = this._rectifyrowOrders_(staticTableMetas[tableOrder].staticTableRowResources.filter(r => r.rowOrder !== rowOrder));

        this.setTableMetas(staticTableMetas);
    }

    shiftRowUp(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        if (rowOrder === 0) {
            return false;
        } else {
            staticTableMetas[tableOrder].staticTableRowResources[rowOrder].rowOrder--;

            const nextRowIndex = rowOrder - 1;
            staticTableMetas[tableOrder].staticTableRowResources[nextRowIndex].rowOrder++;

            staticTableMetas[tableOrder].staticTableRowResources = staticTableMetas[tableOrder].staticTableRowResources.sort((a, b) => a.rowOrder - b.rowOrder);

            this.setTableMetas(staticTableMetas);
        }
    }

    shiftRowDown(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const ids = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRowResources);

        const lastID = ids[ids.length - 1];
        if (lastID === rowOrder) {
            return false;
        } else {
            staticTableMetas[tableOrder].staticTableRowResources[rowOrder].rowOrder++;

            const nextRowIndex = rowOrder + 1;
            staticTableMetas[tableOrder].staticTableRowResources[nextRowIndex].rowOrder--;

            staticTableMetas[tableOrder].staticTableRowResources = staticTableMetas[tableOrder].staticTableRowResources.sort((a, b) => a.rowOrder - b.rowOrder);
        }
        this.setTableMetas(staticTableMetas);
    }

    changePer(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const perState = staticTableMetas[tableOrder].staticTableRowResources[rowOrder].perPerson;
        staticTableMetas[tableOrder].staticTableRowResources[rowOrder].perPerson = !perState;
        this.setTableMetas(staticTableMetas);
    }

    changeRange(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const rangeState = staticTableMetas[tableOrder].staticTableRowResources[rowOrder].range;
        staticTableMetas[tableOrder].staticTableRowResources[rowOrder].range = !rangeState;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMin(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRowResources[rowOrder].fee.min = val;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMax(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRowResources[rowOrder].fee.max = val;
        this.setTableMetas(staticTableMetas);
    }

    isRowFirstPosition(rowOrder: number) {
        return rowOrder === 0;
    }
    isRowLastPosition(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const ids = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRowResources);
        const lastID = ids[ids.length - 1];
        return rowOrder === lastID;
    }

    isTableFirstPosition(tableOrder: number) {
        return tableOrder === 0;
    }

    isTableLastPosition(staticTableMetas: StaticTableMetaResources, tableOrder: number): boolean {
        const ids = this._getIDs_(staticTableMetas);
        const lastID = ids[ids.length - 1];
        return tableOrder === lastID;
    }

    togglePerPersonRequired(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        const currentValue = staticTableMetas[tableOrder].perPersonInputRequired;
        staticTableMetas[tableOrder].perPersonInputRequired = !currentValue;
        this.setTableMetas(staticTableMetas);
    }

    setPerPersonRequired(staticTableMetas: StaticTableMetaResources, tableOrder: number, value: boolean) {
        staticTableMetas[tableOrder].perPersonInputRequired = value;
        // this.setTableMetas(staticTableMetas);
    }

    toggleShowTotals(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        staticTableMetas[tableOrder].includeTotals = !staticTableMetas[tableOrder].includeTotals;
        this.setTableMetas(staticTableMetas);
    }
}
