import React, { useContext } from "react";
import { DynamicTable, DynamicTableProps, Modifier, QuantUnitDefinition, TableNameMap } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { dynamicTableComponentMap, DynamicTableTypes } from "./DynamicTableRegistry";

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
    tableMetaIndex: number;
    showDebug: boolean;
    unitTypes: QuantUnitDefinition[];
    table: DynamicTable;
    tables: DynamicTable[];
    inUse: boolean;
    setTables: any;
    deleteAction: () => Promise<void>;
}

export const PricingStrategyTable = ({
    table,
    tables,
    setTables,
    inUse,
    showDebug,
    tableMetaIndex,
    availableDynamicTableOptions,
    tableNameMap,
    areaIdentifier,
    unitTypes,
    deleteAction,
}: PricingStrategyTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const [tableTag, setTableTag] = useState<string>("");

    const [localTable, setLocalTable] = useState<DynamicTable>();

    const onSaveFactory = <T,>(modifier: Modifier, saveType: DynamicTableTypes, propertySetter: () => void) => async (tableRows: T) => {
        const result = modifier.validateTable(tableRows);

        if (result) {
            const currentMeta = tables[tableMetaIndex].tableMeta;
            propertySetter();

            const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(currentMeta);
            const updatedRows = await repository.Configuration.Tables.Dynamic.saveDynamicTable<T>(areaIdentifier, saveType, tableRows, localTable!.tableMeta.tableId, tableTag);
            tables[tableMetaIndex].tableRows = updatedRows;
            tables[tableMetaIndex].tableMeta = newTableMeta;
            setTables(cloneDeep(tables));

            return true;
        } else {
            return false;
        }
    };

    const tableProps: DynamicTableProps = {
        setLocalTable,
        localTable: table,
        availableDynamicTableOptions,
        tableNameMap,
        unitTypes,
        inUse,
        tableTag,
        setTableTag,
        tableId: table.tableMeta.tableId,
        tables,
        areaIdentifier,
        showDebug,
        tableMetaIndex,
        setTables,
        onSaveFactory,
        deleteAction,
    };

    const DynamicTableComponent = dynamicTableComponentMap[table.tableMeta.tableType];

    return (
        <section className={cls.section}>
            <DynamicTableComponent {...tableProps} />
        </section>
    );
};
