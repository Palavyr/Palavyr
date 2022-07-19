import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { CategoryNestedThresholdData, PricingStrategy, PricingStrategyProps } from "@Palavyr-Types";

import { DisplayTableData } from "../DisplayTableData";
import { CategoryNestedThresholdContainer } from "./CategoryNestedThresholdContainer";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PricingStrategyHeader } from "../../PricingStrategyHeader";
import { cloneDeep } from "lodash";
import { useIsMounted } from "@common/hooks/useIsMounted";

const useStyles = makeStyles(theme => ({
    root: {
        borderTop: "3px solid red",
    },
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

export const CategoryNestedThreshold = ({
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
    const isMounted = useIsMounted();

    const [localTable, setLocalTable] = useState<PricingStrategy>();
    useEffect(() => {
        if (isMounted) setLocalTable(table);
    }, [intentId, table, tables, table.tableRows, localTable?.tableMeta.unitId, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        (async () => {
            if (localTable && isMounted) {
                const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(localTable.tableMeta.intentId, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                localTable.tableRows = tableRows;
                setLocalTable(cloneDeep(localTable));
            }
        })();
    }, [intentId, localTable?.tableMeta.tableType]);

    const modifier = new CategoryNestedThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addCategory = async () => {
        if (localTable) await modifier.addCategory(localTable.tableRows, repository, intentId, tableId);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.SavePricingStrategy<CategoryNestedThresholdData[]>(
                    intentId,
                    PricingStrategyTypes.CategoryNestedThreshold,
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
            <CategoryNestedThresholdContainer
                tableData={localTable.tableRows}
                modifier={modifier}
                tableId={tableId}
                intentId={intentId}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addCategory} color="primary" variant="contained">
                            Add Category
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} properties={["category", "triggerFallback", "threshold", "valueMin", "valueMax", "rowOrder", "rowId", "itemOrder", "itemId"]} />}
        </>
    ) : (
        <></>
    );
};
