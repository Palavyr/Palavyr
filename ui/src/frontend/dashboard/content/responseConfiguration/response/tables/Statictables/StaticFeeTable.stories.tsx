import * as React from "react";
import { Meta } from "@storybook/react";
import { StaticFeeTable, IStaticFeeTable } from "./StaticFeeTable";
import { StaticTablesModifier } from "./staticTableModifier";
import { MockStaticTablesMetas, MockStaticTablesMeta } from "frontend/test/dummyNodeData/dummyTables";
import { PalavyrRepository } from "@common/client/PalavyrRepository";

export default {
    title: "Dashboard/Tables/StaticFeeTable",
    component: StaticFeeTable,
} as Meta;

const Template = (args: IStaticFeeTable) => (
    <div style={{ border: "1px solid black", borderRadius: "6px" }}>
        <StaticFeeTable {...args} />
    </div>
);

const modifier = new StaticTablesModifier(() => {}, {} as PalavyrRepository);

// TODO: Mock api call to get data
export const TwoRows = Template.bind({});
TwoRows.args = {
    staticTableMetas: MockStaticTablesMetas,
    staticTableMeta: MockStaticTablesMeta,
    tableModifier: modifier,
};
