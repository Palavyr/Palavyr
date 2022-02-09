import React from "react";
import { DynamicTable, DynamicTableProps, QuantUnitDefinition, TableNameMap } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { cloneDeep } from "lodash";
import { dynamicTableComponentMap } from "./DynamicTableRegistry";

export const useStyles = makeStyles(theme => ({
    headerCol: {
        textAlign: "center",
        fontSize: "16pt",
    },
    centerText: {
        textAlign: "center",
    },
    header: {
        paddingTop: ".3rem",
        paddingBottom: ".3rem",
        backgroundColor: theme.palette.secondary.light,
    },
    section: {
        border: "none",
        margin: "1.2rem",
        borderRadius: "15px",
        background: theme.palette.secondary.light,
        boxShadow: "none",
    },
    table: {
        border: "none",
        background: theme.palette.secondary.light,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        borderRadius: "10px",
    },
    textinput: {
        margin: "0.9rem",
        width: "50ch",
    },
}));

export const includesUnit = (localTable: DynamicTable) => {
    const allowedDynamicTypes = ["CategoryNestedThreshold", "BasicThreshold", "PercentOfThreshold"]; // Sorry - TODO / Add identifier from server
    return allowedDynamicTypes.includes(localTable.tableMeta.tableType);
};

export interface PricingStrategyTableProps {
    availableDynamicTableOptions: Array<string>;
    tableNameMap: TableNameMap;
    areaIdentifier: string;
    showDebug: boolean;
    unitTypes: QuantUnitDefinition[];
    table: DynamicTable;
    tables: DynamicTable[];
    inUse: boolean;
    setTables: any;
    deleteAction: () => Promise<void>;
    tableIndex: number;
}

export const PricingStrategyTable = ({
    table,
    tables,
    setTables,
    inUse,
    showDebug,
    tableIndex,
    availableDynamicTableOptions,
    tableNameMap,
    areaIdentifier,
    unitTypes,
    deleteAction,
}: PricingStrategyTableProps) => {
    const cls = useStyles();

    const tableProps: DynamicTableProps = {
        table,
        tableIndex, // required for save action
        tables, // required for save action
        setTables, // required for save action
        availableDynamicTableOptions,
        tableNameMap,
        unitTypes,
        inUse,
        tableId: table.tableMeta.tableId,
        areaIdentifier,
        showDebug,
        deleteAction,
    };

    const DynamicTableComponent = dynamicTableComponentMap[table.tableMeta.tableType];

    return (
        <section className={cls.section}>
            <DynamicTableComponent {...tableProps} />
        </section>
    );
};
