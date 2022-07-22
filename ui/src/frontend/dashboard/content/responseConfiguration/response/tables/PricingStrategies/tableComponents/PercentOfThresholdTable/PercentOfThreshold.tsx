import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { PricingStrategy, PricingStrategyProps, PercentOfThresholdResource } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";
import { DisplayTableData } from "../DisplayTableData";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PricingStrategyHeader } from "../../PricingStrategyTableHeader";

import { cloneDeep } from "lodash";

const useStyles = makeStyles(theme => ({
    root: {},
    tableStyles: {
        background: "transparent",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {},
    alignLeft: {
        float: "left",
    },
    alignRight: {
        float: "right",
    },
}));

export const PercentOfThreshold = ({
    showDebug,
    tableId,
    setTables,
    intentId,
    deleteAction,
    tables,
    tableIndex,
    availablePricingStrategyOptions,
    tableNameMap,
    unitTypes,
    inUse,
    table,
}: PricingStrategyProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [localTable, setLocalTable] = useState<PricingStrategy>();
    const [loaded, setLoaded] = useState(false);

    useEffect(() => {
        setLocalTable(table);
        setLoaded(true);
    }, [intentId, table, tables, table.tableRows, localTable?.tableMeta.unitIdEnum, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        (async () => {
            if (localTable && loaded) {
                const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(localTable.tableMeta.intentId, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                localTable.tableRows = tableRows;
                setLocalTable(cloneDeep(localTable));
            }
        })();
    }, [intentId, localTable?.tableMeta.tableType]);

    const modifier = new PercentOfThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addItemOnClick = async () => {
        if (localTable) await modifier.addItem(localTable.tableRows, repository, intentId, tableId);
    };

    const addRowOnClickFactory = (itemId: string) => async () => {
        if (localTable) await modifier.addRow(localTable.tableRows, repository, intentId, tableId, itemId);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.SavePricingStrategy<PercentOfThresholdResource[]>(
                    intentId,
                    PricingStrategyTypes.PercentOfThreshold,
                    tableRows,
                    localTable.tableMeta.tableId,
                    localTable.tableMeta.tableTag
                );

                const updatedTable = cloneDeep(localTable);
                updatedTable.tableRows = updatedRows;
                updatedTable.tableMeta = newTableMeta;
                setLocalTable(updatedTable);

                return true;
            } else {
                return false;
            }
        }
        return false;
    };

    return localTable ? (
        <>
            <PricingStrategyHeader
                localTable={localTable}
                setLocalTable={setLocalTable}
                setTables={setTables}
                availablePricingStrategyOptions={availablePricingStrategyOptions}
                unitTypes={unitTypes}
                inUse={inUse}
            />
            <PercentOfThresholdContainer
                tableData={localTable.tableRows}
                modifier={modifier}
                addRowOnClickFactory={addRowOnClickFactory}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Item
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} />}
        </>
    ) : (
        <></>
    );
};
