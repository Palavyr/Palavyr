import React, { useState, useCallback, useEffect, Suspense, useContext } from "react";
import { PricingStrategy, PricingStrategyTableTypeResource, QuantUnitDefinition, TableData, TableNameMap } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, FormControlLabel, Checkbox } from "@material-ui/core";
import { PricingStrategyTable } from "./PricingStrategyTable";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { isDevelopmentStage } from "@common/client/clientUtils";
import { OsTypeToggle } from "frontend/dashboard/content/responseConfiguration/intentSettings/enableIntents/OsTypeToggle";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface IPricingStrategy {
    title: string;
    intentId: string;
    children: React.ReactNode;
    initialState?: boolean;
}

export const PricingStrategyConfiguration = ({ title, intentId, children, initialState }: IPricingStrategy) => {
    const { repository, planTypeMeta, setSuccessOpen } = useContext(DashboardContext);

    const [showDebug, setShowDebug] = useState<boolean>(false);
    const [availableTables, setAvailableTables] = useState<PricingStrategyTableTypeResource[]>([]);
    const [tableNameMap, setTableNameMap] = useState<TableNameMap>([]);
    const [showTotals, setShowTotals] = useState<boolean | null>(null);
    const [unitTypes, setUnitTypes] = useState<QuantUnitDefinition[]>([]);
    const [inUse, setInUse] = useState<boolean>(false);

    const [tables, setTables] = useState<PricingStrategy[]>([]);

    const loadTableData = useCallback(async () => {
        const PricingStrategyMetas = await repository.Configuration.Tables.Dynamic.GetPricingStrategyMetas(intentId);
        const showTotals = await repository.Intent.GetShowDynamicTotals(intentId);

        // show fee totals totals row
        setShowTotals(showTotals);

        // TODO - seend array of all table metas and retrive in a single request.
        let tables: PricingStrategy[] = [];
        let counter: number = 0;
        PricingStrategyMetas.forEach(async tableMeta => {
            const { tableRows, isInUse } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(intentId, tableMeta.tableType, tableMeta.tableId);
            tables.push({ tableMeta, tableRows: tableRows as TableData });
            counter++;
            if (counter === PricingStrategyMetas.length) {
                setTables(tables);

                // enable / disable the selector depending on if the current pricing strategy has been included in the conversation nodes
                setInUse(isInUse);
            }
        });

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [intentId]);

    useEffect(() => {
        (async () => {
            const tableNameMap = await repository.Configuration.Tables.Dynamic.GetPricingStrategyTypes();
            const quantTypes = await repository.Configuration.Units.GetSupportedUnitIds();

            // map that provides e.g. Select One Flat: CategorySelect. used to derive the pretty names
            setAvailableTables(tableNameMap);
            // map of pricing trategy pretty names
            setTableNameMap(tableNameMap);
            // Set array of quant unit types to select from
            setUnitTypes(quantTypes);
        })();
    }, []);

    const addPricingStrategy = async () => {
        // We always add the default dynamic table - the Select One Flat table
        const newMeta = await repository.Configuration.Tables.Dynamic.CreatePricingStrategy(intentId);
        const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(intentId, newMeta.tableType, newMeta.tableId);

        const tableNameMap = await repository.Configuration.Tables.Dynamic.GetPricingStrategyTypes();
        const availableTables = tableNameMap;
        setAvailableTables(availableTables);
        // map of pricing trategy pretty names
        setTableNameMap(tableNameMap);

        const newTable: PricingStrategy = {
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
    }, [intentId, loadTableData]);

    const changeShowTotals = async (e: { target: { checked: any } }) => {
        const newShowTotals = e.target.checked;
        const shouldShow = await repository.Intent.SetShowDynamicTotals(intentId, newShowTotals);
        setShowTotals(shouldShow);
        setSuccessOpen(true);
    };

    const actions = (
        <>
            {showTotals !== null && tables.length > 0 && <FormControlLabel label="Show Totals" control={<Checkbox disabled={showTotals === null} checked={showTotals} onChange={changeShowTotals} />} />}
            {planTypeMeta && tables.length >= planTypeMeta.allowedPricingStrategys ? (
                <>
                    <PalavyrText display="inline">
                        <strong>Upgrade your subscription to add more dynamic tables</strong>
                    </PalavyrText>
                    <Button disabled={true} startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addPricingStrategy}>
                        <PalavyrText>Add Pricing Strategy</PalavyrText>
                    </Button>
                </>
            ) : (
                <Button startIcon={<AddBoxIcon />} variant="contained" color="primary" onClick={addPricingStrategy}>
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
                        No dynamic tables configured for this intent.
                    </PalavyrText>
                )}

                {unitTypes &&
                    tables.map((table: PricingStrategy, tableIndex: number) => {
                        const onDelete = async () => {
                            // delete table from DB
                            await repository.Configuration.Tables.Dynamic.DeletePricingStrategy(intentId, table.tableMeta.tableType, table.tableMeta.tableId);

                            // delete table from UI
                            const newTables = cloneDeep(tables);
                            newTables.splice(tableIndex, 1);
                            setTables(newTables);
                        };

                        return (
                            <PricingStrategyTable
                                key={[tableIndex, table.tableMeta.tableId].join("-")}
                                table={table}
                                tables={tables}
                                setTables={setTables}
                                unitTypes={unitTypes}
                                tableIndex={tableIndex}
                                availablePricingStrategyOptions={availableTables}
                                tableNameMap={tableNameMap}
                                intentId={intentId}
                                showDebug={showDebug}
                                inUse={inUse}
                                deleteAction={onDelete}
                            />
                        );
                    })}
            </Suspense>
        </PalavyrAccordian>
    );
};
