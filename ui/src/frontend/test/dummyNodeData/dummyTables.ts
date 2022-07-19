import { StaticTableRowResource, StaticTableMetaResource } from "@Palavyr-Types"
import { cloneDeep } from "lodash";

export const MockStaticFee = {
    id: 0,
    feeId: "0as",
    min: 0.00,
    max: 12.00
}

export const MockStaticTableRow: StaticTableRowResource = {
    id: 0,
    rowOrder: 0,
    description: "Damn Fee 1!",
    fee: MockStaticFee,
    range: true,
    perPerson: true,
    tableOrder: 0,
    intentId: "abc-123",
    includeTotals: true

}

const row1 = cloneDeep(MockStaticTableRow);
row1.rowOrder = 0;
row1.perPerson = false;
row1.range = false;

const row2 = cloneDeep(MockStaticTableRow);
row2.rowOrder = 1;

const row3 = cloneDeep(MockStaticTableRow);
row3.rowOrder = 2;

export const MockStaticTablesMeta: StaticTableMetaResource = {
    id: 0,
    perPersonInputRequired: false,
    intentId: "abc-123",
    description: "This is a good first table.",
    tableOrder: 0,
    includeTotals: true,
    staticTableRows: [
        row1,
        row2,
        row3
    ]
}

export const MockStaticTablesMetas: Array<StaticTableMetaResource> = [
    MockStaticTablesMeta
]