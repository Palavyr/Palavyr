import React, { useState, useCallback, useEffect, Suspense, useContext } from "react";
import { DynamicTable, PricingStrategyTableTypeResource, QuantUnitDefinition, TableData, TableNameMap } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, FormControlLabel, Checkbox } from "@material-ui/core";
import { PricingStrategyTable } from "./PricingStrategyTable";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { isDevelopmentStage } from "@common/client/clientUtils";
import { OsTypeToggle } from "frontend/dashboard/content/responseConfiguration/areaSettings/enableAreas/OsTypeToggle";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import Fade from "react-reveal/Fade";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface IDynamicTable {
    title: string;
    areaIdentifier: string;
    children: React.ReactNode;
    initialState?: boolean;
}

export const DynamicTableConfiguration = ({ title, areaIdentifier, children, initialState }: IDynamicTable) => {
    const { repository, planTypeMeta, setSuccessOpen } = useContext(DashboardContext);

    const [showDebug, setShowDebug] = useState<boolean>(false);
    const [availableTables, setAvailableTables] = useState<PricingStrategyTableTypeResource[]>([]);
    const [tableNameMap, setTableNameMap] = useState<TableNameMap>([]);
    const [showTotals, setShowTotals] = useState<boolean | null>(null);
    const [unitTypes, setUnitTypes] = useState<QuantUnitDefinition[]>([]);
    const [inUse, setInUse] = useState<boolean>(false);

    const [tables, setTables] = useState<DynamicTable[]>([]);

    const loadTableData = useCallback(async () => {
        const dynamicTableMetas = await repository.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);
        const showTotals = await repository.Area.getShowDynamicTotals(areaIdentifier);

        // show fee totals totals row
        setShowTotals(showTotals);

        // TODO - seend array of all table metas and retrive in a single request.
        let tables: DynamicTable[] = [];
        let counter: number = 0;
        dynamicTableMetas.forEach(async tableMeta => {
            const { tableRows, isInUse } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, tableMeta.tableType, tableMeta.tableId);
            tables.push({ tableMeta, tableRows: tableRows as TableData });
            counter++;
            if (counter === dynamicTableMetas.length) {
                setTables(tables);

                // enable / disable the selector depending on if the current pricing strategy has been included in the conversation nodes
                setInUse(isInUse);
            }
        });

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        (async () => {
            const tableNameMap = await repository.Configuration.Tables.Dynamic.getDynamicTableTypes();
            const quantTypes = await repository.Configuration.Units.GetSupportedUnitIds();

            // map that provides e.g. Select One Flat: SelectOneFlat. used to derive the pretty names
            setAvailableTables(tableNameMap);
            // map of pricing trategy pretty names
            setTableNameMap(tableNameMap);
            // Set array of quant unit types to select from
            setUnitTypes(quantTypes);
        })();
    }, []);

    const addDynamicTable = async () => {
        // We always add the default dynamic table - the Select One Flat table
        const newMeta = await repository.Configuration.Tables.Dynamic.createDynamicTable(areaIdentifier);
        const { tableRows } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, newMeta.tableType, newMeta.tableId);

        const tableNameMap = await repository.Configuration.Tables.Dynamic.getDynamicTableTypes();
        const availableTables = tableNameMap;
        setAvailableTables(availableTables);
        // map of pricing trategy pretty names
        setTableNameMap(tableNameMap);

        const newTable: DynamicTable = {
            tableMeta: newMeta,
            tableRows: tableRows as TableData,
        };

        setTables([...tables, newTable]);
    };

    useEffect(() => {
        loadTableData();
        return () => {
            setTables([]);
            setInUse(false);
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
            {showTotals !== null && tables.length > 0 && <FormControlLabel label="Show Totals" control={<Checkbox disabled={showTotals === null} checked={showTotals} onChange={changeShowTotals} />} />}
            {planTypeMeta && tables.length >= planTypeMeta.allowedDynamicTables ? (
                <>
                    <PalavyrText display="inline">
                        <strong>Upgrade your subscription to add more dynamic tables</strong>
                    </PalavyrText>
                    <Button disabled={true} startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                        <PalavyrText>Add Pricing Strategy</PalavyrText>
                    </Button>
                </>
            ) : (
                <Button startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addDynamicTable}>
                    <PalavyrText>Add Pricing Strategy</PalavyrText>
                </Button>
            )}
        </>
    );

    return (
        <PalavyrAccordian title={title} initialState={initialState ?? false} actions={actions}>
            {children}
            {isDevelopmentStage() && (
                <OsTypeToggle
                    controlledState={showDebug}
                    onChange={() => {
                        setShowDebug(!showDebug);
                        setTables(cloneDeep(tables));
                    }}
                    enabledLabel="Show Debug"
                    disabledLabel="Show Debug"
                />
            )}
            <Suspense fallback={<h1>Loading Dynamic Tables...</h1>}>
                {tables.length === 0 && (
                    <PalavyrText align="center" color="secondary" style={{ padding: "0.8rem" }} variant="h5">
                        No dynamic tables configured for this area.
                    </PalavyrText>
                )}

                {unitTypes &&
                    tables.map((table: DynamicTable, tableIndex: number) => {
                        const onDelete = async () => {
                            // delete table from DB
                            await repository.Configuration.Tables.Dynamic.deleteDynamicTable(areaIdentifier, table.tableMeta.tableType, table.tableMeta.tableId);

                            // delete table from UI
                            const newTables = cloneDeep(tables);
                            newTables.splice(tableIndex, 1);
                            setTables(newTables);
                        };

                        return (
                            <Fade key={["Fade", tableIndex, table.tableMeta.tableId].join("-")}>
                                <PricingStrategyTable
                                    key={[tableIndex, table.tableMeta.tableId].join("-")}
                                    table={table}
                                    tables={tables}
                                    setTables={setTables}
                                    unitTypes={unitTypes}
                                    tableIndex={tableIndex}
                                    availableDynamicTableOptions={availableTables}
                                    tableNameMap={tableNameMap}
                                    areaIdentifier={areaIdentifier}
                                    showDebug={showDebug}
                                    inUse={inUse}
                                    deleteAction={onDelete}
                                />
                            </Fade>
                        );
                    })}
            </Suspense>
        </PalavyrAccordian>
    );
};
