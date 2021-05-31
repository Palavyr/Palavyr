import { PalavyrRepository } from "@api-client/PalavyrRepository";
import React, { useState, useCallback, useEffect, Suspense, useContext } from "react";
import { DynamicTableMetas, TableNameMap } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Typography, Button, Tooltip } from "@material-ui/core";
import { SingleDynamicFeeTable } from "./SingleDynamicFeeTable";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { isDevelopmentStage } from "@api-client/clientUtils";
import { OsTypeToggle } from "dashboard/content/responseConfiguration/areaSettings/enableAreas/OsTypeToggle";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

export interface IDynamicTable {
    title: string;
    areaIdentifier: string;
    children: React.ReactNode;
}

export const DynamicTableConfiguration = ({ title, areaIdentifier, children }: IDynamicTable) => {
    const repository = new PalavyrRepository();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [parentState, changeParentState] = useState<boolean>(false);
    const [showDebug, setShowDebug] = useState<boolean>(isDevelopmentStage() ? true : false);
    const [tableMetas, setTableMetas] = useState<DynamicTableMetas>([]);
    const [availableTables, setAvailableTables] = useState<Array<string>>([]);
    const [tableNameMap, setTableNameMap] = useState<TableNameMap>({});
    const { planTypeMeta } = useContext(DashboardContext);

    const loadTableData = useCallback(async () => {
        const dynamicTableMetas = await repository.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);
        const tableNameMap = await repository.Configuration.Tables.Dynamic.getDynamicTableTypes();

        // map that provides e.g. Select One Flat: SelectOneFlat. used to derive the pretty names
        const availableTablePrettyNames = Object.keys(tableNameMap);

        setTableMetas(cloneDeep(dynamicTableMetas));
        setAvailableTables(availableTablePrettyNames);
        setTableNameMap(tableNameMap);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const addDynamicTable = async () => {
        // We always add the default dynamic table - the Select One Flat table
        var newMeta = await repository.Configuration.Tables.Dynamic.createDynamicTable(areaIdentifier);
        tableMetas.push(newMeta);
        setTableMetas(cloneDeep(tableMetas));
        changeParentState(!parentState); // TODO: Reload the tables...
    };

    useEffect(() => {
        if (!loaded) {
            loadTableData();
            setLoaded(true);
        }
        return () => {
            return setLoaded(false);
        };
    }, [areaIdentifier, loadTableData]);

    const actions =
        planTypeMeta && tableMetas.length >= planTypeMeta.allowedDynamicTables ? (
            <>
                <Typography display="inline">
                    <strong>Upgrade your subscription to add more dynamic tables</strong>
                </Typography>
                <Button disabled={true} startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                    <Typography>Add Dynamic Table</Typography>
                </Button>
            </>
        ) : (
            <Button startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                <Typography>Add Dynamic Table</Typography>
            </Button>
        );

    return (
        <PalavyrAccordian title={title} initialState={true} actions={actions}>
            {children}
            {isDevelopmentStage() && <OsTypeToggle controlledState={showDebug} onChange={() => setShowDebug(!showDebug)} enabledLabel="Show Debug" disabledLabel="Show Debug" />}
            <Suspense fallback={<h1>Loading Dynamic Tables...</h1>}>
                {tableMetas.length === 0 && (
                    <Typography align="center" color="secondary" style={{ padding: "0.8rem" }} variant="h5">
                        No dynamic tables configured for this area.
                    </Typography>
                )}

                {tableMetas.map((tableMeta, index) => {
                    return (
                        <SingleDynamicFeeTable
                            key={index}
                            tableNumber={index}
                            setLoaded={setLoaded}
                            tableMetas={tableMetas}
                            setTableMetas={setTableMetas}
                            tableMetaIndex={index}
                            defaultTableMeta={tableMeta}
                            availablDynamicTableOptions={availableTables}
                            tableNameMap={tableNameMap}
                            parentState={parentState}
                            changeParentState={changeParentState}
                            areaIdentifier={areaIdentifier}
                            showDebug={showDebug}
                        />
                    );
                })}
            </Suspense>
        </PalavyrAccordian>
    );
};
