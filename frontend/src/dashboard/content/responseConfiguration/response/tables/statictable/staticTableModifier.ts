import { StaticTableMetas, StaticTableRows, StaticTableRow, StaticTableMeta, AnyVoidFunction, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { v4 as uuid } from "uuid";

export class StaticTablesModifier {
    onClick: SetState<StaticTableMetas> | AnyVoidFunction;
    repository: PalavyrRepository;

    constructor(onClick: SetState<StaticTableMetas> | AnyVoidFunction) {
        this.onClick = onClick;
        this.repository = new PalavyrRepository();
    }

    setTableMetas(newState: StaticTableMetas) {
        this.onClick(cloneDeep(newState));
    }

    _getIDs_(metas: StaticTableMetas) {
        return metas.map((meta) => meta.tableOrder).sort();
    }

    _getrowOrders_(list: StaticTableRows) {
        return list.map((row: StaticTableRow) => row.rowOrder).sort();
    }

    _generateNextId_(ids: Array<number>) {
        return ids.length;
    }

    _rectifyIDs_(list: StaticTableMetas): StaticTableMetas {
        const rectifiedList: StaticTableMetas = [];
        list.forEach((table, newIdx) => {
            table.tableOrder = newIdx;
            rectifiedList.push(table);
        });
        return rectifiedList;
    }

    _rectifyrowOrders_(list: StaticTableRows): StaticTableRows {
        const rectifiedList: StaticTableRows = [];
        list.forEach((table, newIdx) => {
            table.rowOrder = newIdx;
            rectifiedList.push(table);
        });
        return rectifiedList;
    }

    setTableDescription(staticTableMetas: StaticTableMetas, tableOrder: number, description: string) {
        staticTableMetas[tableOrder].description = description;
        this.setTableMetas(staticTableMetas);
    }

    setRowDescription(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number, description: string) {
        staticTableMetas[tableOrder].staticTableRows[rowOrder].description = description;
        this.setTableMetas(staticTableMetas);
    }

    moveTableDown(staticTableMetas: StaticTableMetas, tableOrder: number) {
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

    moveTableUp(staticTableMetas: StaticTableMetas, tableOrder: number) {
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

    async addTable(staticTableMetas: StaticTableMetas, repository: PalavyrRepository, areaIdentifier: string) {
        const tableOrders = this._getIDs_(staticTableMetas);
        const newtableOrder = this._generateNextId_(tableOrders);

        const newTableTemplate = await repository.Configuration.Tables.Static.getStaticTablesMetaTemplate(areaIdentifier);
        const newTable = ((): StaticTableMeta => ({
            ...newTableTemplate,
            tableOrder: newtableOrder,
        }))();

        staticTableMetas.push(newTable);
        this.setTableMetas(staticTableMetas);
    }

    delTable(staticTableMetas: StaticTableMetas, tableOrder: number) {
        staticTableMetas = staticTableMetas.filter((t) => t.tableOrder !== tableOrder);
        staticTableMetas = this._rectifyIDs_(staticTableMetas);
        this.setTableMetas(staticTableMetas);
    }

    async addRow(staticTableMetas: StaticTableMetas, tableOrder: number) {
        const rowOrders = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRows);
        const nextrowOrder = this._generateNextId_(rowOrders);
        const curTableOrder = staticTableMetas[0].tableOrder;
        const curAreaIdentifier = staticTableMetas[0].areaIdentifier;

        const newRow = await this.repository.Configuration.Tables.Static.getStaticTableRowTemplate(curAreaIdentifier, curTableOrder);

        newRow.rowOrder = nextrowOrder;

        staticTableMetas[tableOrder].staticTableRows.push(newRow);

        this.setTableMetas(staticTableMetas);
    }

    delRow(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
        staticTableMetas[tableOrder].staticTableRows = this._rectifyrowOrders_(staticTableMetas[tableOrder].staticTableRows.filter((r) => r.rowOrder !== rowOrder));

        this.setTableMetas(staticTableMetas);
    }

    shiftRowUp(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
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

    shiftRowDown(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
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

    changePer(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
        const perState = staticTableMetas[tableOrder].staticTableRows[rowOrder].perPerson;
        staticTableMetas[tableOrder].staticTableRows[rowOrder].perPerson = !perState;
        this.setTableMetas(staticTableMetas);
    }

    changeRange(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
        const rangeState = staticTableMetas[tableOrder].staticTableRows[rowOrder].range;
        staticTableMetas[tableOrder].staticTableRows[rowOrder].range = !rangeState;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMin(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRows[rowOrder].fee.min = val;
        this.setTableMetas(staticTableMetas);
    }

    setFeeMax(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number, val: number) {
        staticTableMetas[tableOrder].staticTableRows[rowOrder].fee.max = val;
        this.setTableMetas(staticTableMetas);
    }

    isRowFirstPosition(rowOrder: number) {
        return rowOrder === 0;
    }
    isRowLastPosition(staticTableMetas: StaticTableMetas, tableOrder: number, rowOrder: number) {
        const ids = this._getrowOrders_(staticTableMetas[tableOrder].staticTableRows);
        const lastID = ids[ids.length - 1];
        return rowOrder === lastID;
    }

    isTableFirstPosition(tableOrder: number) {
        return tableOrder === 0;
    }

    isTableLastPosition(staticTableMetas: StaticTableMetas, tableOrder: number): boolean {
        const ids = this._getIDs_(staticTableMetas);
        const lastID = ids[ids.length - 1];
        return tableOrder === lastID;
    }

    togglePerPersonRequired(staticTableMetas: StaticTableMetas, tableOrder: number) {
        const currentValue = staticTableMetas[tableOrder].perPersonInputRequired;
        staticTableMetas[tableOrder].perPersonInputRequired = !currentValue;
        this.setTableMetas(staticTableMetas);
    }

    setPerPersonRequired(staticTableMetas: StaticTableMetas, tableOrder: number, value: boolean) {
        staticTableMetas[tableOrder].perPersonInputRequired = value;
        // this.setTableMetas(staticTableMetas);
    }
}
