import React, { useState, useCallback, useEffect, Suspense, useContext } from "react";
import { DynamicTableMetas, QuantUnitDefinition, TableNameMap } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Typography, Button, FormControlLabel, Checkbox } from "@material-ui/core";
import { SingleDynamicFeeTable } from "./SingleDynamicFeeTable";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { isDevelopmentStage } from "@common/client/clientUtils";
import { OsTypeToggle } from "frontend/dashboard/content/responseConfiguration/areaSettings/enableAreas/OsTypeToggle";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import Fade from "react-reveal/Fade";

export interface IDynamicTable {
    title: string;
    areaIdentifier: string;
    children: React.ReactNode;
}

export const DynamicTableConfiguration = ({ title, areaIdentifier, children }: IDynamicTable) => {
    const { repository, planTypeMeta, setSuccessOpen } = useContext(DashboardContext);

    const [loaded, setLoaded] = useState<boolean>(false);
    const [parentState, changeParentState] = useState<boolean>(false);
    const [showDebug, setShowDebug] = useState<boolean>(isDevelopmentStage() ? true : false);
    const [tableMetas, setTableMetas] = useState<DynamicTableMetas>([]);
    const [availableTables, setAvailableTables] = useState<Array<string>>([]);
    const [tableNameMap, setTableNameMap] = useState<TableNameMap>({});
    const [showTotals, setShowTotals] = useState<boolean | null>(null);
    const [unitTypes, setUnitTypes] = useState<QuantUnitDefinition[]>([]);

    const loadTableData = useCallback(async () => {
        const dynamicTableMetas = await repository.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);
        const tableNameMap = await repository.Configuration.Tables.Dynamic.getDynamicTableTypes();
        const showTotals = await repository.Area.getShowDynamicTotals(areaIdentifier);
        const quantTypes = await repository.Configuration.Units.GetSupportedUnitIds();

        // map that provides e.g. Select One Flat: SelectOneFlat. used to derive the pretty names
        const availableTablePrettyNames = Object.keys(tableNameMap);

        setShowTotals(showTotals);
        setTableMetas(cloneDeep(dynamicTableMetas));
        setAvailableTables(availableTablePrettyNames);
        setTableNameMap(tableNameMap);
        setUnitTypes(quantTypes);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const addDynamicTable = async () => {
        // We always add the default dynamic table - the Select One Flat table
        const newMeta = await repository.Configuration.Tables.Dynamic.createDynamicTable(areaIdentifier);
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

    const changeShowTotals = async (e: { target: { checked: any } }) => {
        const newShowTotals = e.target.checked;
        const shouldShow = await repository.Area.setShowDynamicTotals(areaIdentifier, newShowTotals);
        setShowTotals(shouldShow);
        setSuccessOpen(true);
    };

    const actions = (
        <>
            {showTotals !== null && <FormControlLabel label="Show Totals" control={<Checkbox disabled={showTotals === null} checked={showTotals} onChange={changeShowTotals} />} />}
            {planTypeMeta && tableMetas.length >= planTypeMeta.allowedDynamicTables ? (
                <>
                    <Typography display="inline">
                        <strong>Upgrade your subscription to add more dynamic tables</strong>
                    </Typography>
                    <Button disabled={true} startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                        <Typography>Add Pricing Strategy</Typography>
                    </Button>
                </>
            ) : (
                <Button startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                    <Typography>Add Pricing Strategy</Typography>
                </Button>
            )}
        </>
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
                        <Fade key={["Fade", index, tableMeta.tableId].join("-")}>
                            <SingleDynamicFeeTable
                                key={[index, tableMeta.tableId].join("-")}
                                unitTypes={unitTypes}
                                setLoaded={setLoaded}
                                tableMetas={tableMetas}
                                setTableMetas={setTableMetas}
                                tableMetaIndex={index}
                                tableMeta={tableMeta}
                                availablDynamicTableOptions={availableTables}
                                tableNameMap={tableNameMap}
                                parentState={parentState}
                                changeParentState={changeParentState}
                                areaIdentifier={areaIdentifier}
                                showDebug={showDebug}
                            />
                        </Fade>
                    );
                })}
            </Suspense>
        </PalavyrAccordian>
    );
};
