import React from "react";
import { PricingStrategy, PricingStrategyProps, PricingStrategyTableTypeResource, TableNameMap } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { PricingStrategyComponentMap } from "./PricingStrategyRegistry";
import { QuantUnitDefinition } from "@common/types/api/ApiContracts";

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

export const includesUnit = (localTable: PricingStrategy) => {
    const allowedDynamicTypes = ["CategoryNestedThreshold", "BasicThreshold", "PercentOfThreshold"]; // Sorry - TODO / Add identifier from server
    return allowedDynamicTypes.includes(localTable.tableMeta.tableType);
};

export interface PricingStrategyTableProps {
    availablePricingStrategyOptions: PricingStrategyTableTypeResource[];
    tableNameMap: TableNameMap;
    intentId: string;
    showDebug: boolean;
    unitTypes: QuantUnitDefinition[];
    table: PricingStrategy;
    tables: PricingStrategy[];
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
    availablePricingStrategyOptions,
    tableNameMap,
    intentId,
    unitTypes,
    deleteAction,
}: PricingStrategyTableProps) => {
    const cls = useStyles();

    const tableProps: PricingStrategyProps = {
        table,
        tableIndex, // required for save action
        tables, // required for save action
        setTables, // required for save action
        availablePricingStrategyOptions,
        tableNameMap,
        unitTypes,
        inUse,
        tableId: table.tableMeta.tableId,
        intentId,
        showDebug,
        deleteAction,
    };

    const PricingStrategyComponent = PricingStrategyComponentMap[table.tableMeta.tableType];

    return (
        <section className={cls.section}>
            <PricingStrategyComponent {...tableProps} />
        </section>
    );
};
