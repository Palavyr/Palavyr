import { StaticTableMetaResources, StaticTableRowResources, StaticTableRowResource, StaticTableMetaResource, AnyVoidFunction, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { v4 as uuid } from "uuid";

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
        staticTableMetas[tableOrder].staticTableRows[rowOrder].description = description;
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

        const newTableTemplate = await repository.Configuration.Tables.Static.GetStaticTablesMetaTemplate(intentId);
        const newTable = ((): StaticTableMetaResource => ({
            ...newTableTemplate,
            tableOrder: newtableOrder,
        }))();

        staticTableMetas.push(newTable);
        this.setTableMetas(staticTableMetas);
    }

    delTable(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        staticTableMetas = staticTableMetas.filter(t => t.tableOrder !== tableOrder);
        staticTableMetas = this._rectifyIDs_(staticTableMetas);
        this.setTableMetas(staticTableMetas);
    }

    async addRow(staticTableMetas: StaticTableMetaResources, tableOrder: number) {
        const rowOrders = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRows);
        const nextrowOrder = this._generateNextId_(rowOrders);
        const curTableOrder = staticTableMetas[0].tableOrder;
        const curintentId = staticTableMetas[0].intentId;

        const newRow = await this.repository.Configuration.Tables.Static.GetStaticTableRowTemplate(curintentId, curTableOrder);

        newRow.rowOrder = nextrowOrder;

        staticTableMetas[tableOrder].staticTableRows.push(newRow);

        this.setTableMetas(staticTableMetas);
    }

    delRow(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        staticTableMetas[tableOrder].staticTableRows = this._rectifyrowOrders_(staticTableMetas[tableOrder].staticTableRows.filter(r => r.rowOrder !== rowOrder));

        this.setTableMetas(staticTableMetas);
    }

    shiftRowUp(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        if (rowOrder === 0) {
            return false;
        } else {
            staticTableMetas[tableOrder].staticTableRows[rowOrder].rowOrder--;

            const nextRowIndex = rowOrder - 1;
            staticTableMetas[tableOrder].staticTableRows[nextRowIndex].rowOrder++;

            staticTableMetas[tableOrder].staticTableRows = staticTableMetas[tableOrder].staticTableRows.sort((a, b) => a.rowOrder - b.rowOrder);

            this.setTableMetas(staticTableMetas);
        }
    }

    shiftRowDown(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const ids = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRows);

        const lastID = ids[ids.length - 1];
        if (lastID === rowOrder) {
            return false;
        } else {
            staticTableMetas[tableOrder].staticTableRows[rowOrder].rowOrder++;

            const nextRowIndex = rowOrder + 1;
            staticTableMetas[tableOrder].staticTableRows[nextRowIndex].rowOrder--;

            staticTableMetas[tableOrder].staticTableRows = staticTableMetas[tableOrder].staticTableRows.sort((a, b) => a.rowOrder - b.rowOrder);
        }
        this.setTableMetas(staticTableMetas);
    }

    changePer(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const perState = staticTableMetas[tableOrder].staticTableRows[rowOrder].perPerson;
        staticTableMetas[tableOrder].staticTableRows[rowOrder].perPerson = !perState;
        this.setTableMetas(staticTableMetas);
    }

    changeRange(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const rangeState = staticTableMetas[tableOrder].staticTableRows[rowOrder].range;
        staticTableMetas[tableOrder].staticTableRows[rowOrder].range = !rangeState;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMin(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRows[rowOrder].fee.min = val;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMax(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRows[rowOrder].fee.max = val;
        this.setTableMetas(staticTableMetas);
    }

    isRowFirstPosition(rowOrder: number) {
        return rowOrder === 0;
    }
    isRowLastPosition(staticTableMetas: StaticTableMetaResources, tableOrder: number, rowOrder: number) {
        const ids = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRows);
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
