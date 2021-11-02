import { StaticTableRow, StaticTableMeta } from "@Palavyr-Types"
import { cloneDeep } from "lodash";

export const MockStaticFee = {
    id: 0,
    feeId: "0as",
    min: 0.00,
    max: 12.00
}

export const MockStaticTableRow: StaticTableRow = {
    id: 0,
    rowOrder: 0,
    description: "Damn Fee 1!",
    fee: MockStaticFee,
    range: true,
    perPerson: true,
    tableOrder: 0,
    areaIdentifier: "abc-123",
}

const row1 = cloneDeep(MockStaticTableRow);
row1.rowOrder = 0;
row1.perPerson = false;
row1.range = false;

const row2 = cloneDeep(MockStaticTableRow);
row2.rowOrder = 1;

const row3 = cloneDeep(MockStaticTableRow);
row3.rowOrder = 2;

export const MockStaticTablesMeta: StaticTableMeta = {
    id: 0,
    perPersonInputRequired: false,
    areaIdentifier: "abc-123",
    description: "This is a good first table.",
    tableOrder: 0,
    staticTableRows: [
        row1,
        row2,
        row3
    ]
}

export const MockStaticTablesMetas: Array<StaticTableMeta> = [
    MockStaticTablesMeta
]