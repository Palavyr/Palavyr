import * as React from "react";
import { Meta } from "@storybook/react";
import { StaticRow, IStaticRow } from "./StaticRow";
import { StaticTablesModifier } from "./staticTableModifier";
import { StaticTableRowResource, StaticTableMetaResource } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";

export default {
    title: "Dashboard/Tables/StaticRow",
    component: StaticRow,
} as Meta;

const Template = (args: IStaticRow) => (
    <div style={{ border: "1px solid black", borderRadius: "6px" }}>
        <StaticRow {...args} />
    </div>
);

const modifier = new StaticTablesModifier(() => {}, {} as PalavyrRepository);

const MockStaticTableRow: StaticTableRowResource = {
    id: 0,
    rowOrder: 0,
    description: "Damn Fee 1!",
    fee: { id: 0, feeId: "0as", min: 0.0, max: 12.0, intentId: "1234" },
    range: false,
    perPerson: false,
    tableOrder: 0,
    intentId: "abc-123",
};

const MockStaticTablesMeta: StaticTableMetaResource = {
    id: 0,
    intentId: "abc-123",
    description: "This is a good first table.",
    tableOrder: 0,
    staticTableRowResources: [MockStaticTableRow, MockStaticTableRow],
    perPersonInputRequired: false,
    includeTotals: true,
    tableId: "12345"
};

// TODO: Mock api call to get data
export const SingleRowFalse = Template.bind({});
SingleRowFalse.args = {
    staticTableMetas: [MockStaticTablesMeta],
    tableOrder: MockStaticTablesMeta.tableOrder,
    rowOrder: 0,
    modifier: modifier,
    minFee: MockStaticTableRow.fee.min,
    maxFee: MockStaticTableRow.fee.max,
    rangeState: MockStaticTableRow.range,
    perState: MockStaticTableRow.perPerson,
    description: MockStaticTableRow.description,
};

export const SingleRowTrue = Template.bind({});
SingleRowTrue.args = {
    ...SingleRowFalse.args,
    rangeState: true,
    perState: false,
};
