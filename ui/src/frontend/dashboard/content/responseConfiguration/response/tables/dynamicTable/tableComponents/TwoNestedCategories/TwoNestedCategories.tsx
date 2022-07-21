import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { PricingStrategy, PricingStrategyProps } from "@Palavyr-Types";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesContainer } from "./TwoNestedCategoriesContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { cloneDeep } from "lodash";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { PricingStrategyHeader } from "../../PricingStrategyTableHeader";
import { TwoNestedCategoryResource } from "@common/types/api/EntityResources";

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

export const TwoNestedCategories = ({
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

    useEffect(() => {
        setLocalTable(table);
    }, [intentId, table, tables, table.tableRows, localTable?.tableMeta.unitIdEnum, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        (async () => {
            if (localTable) {
                const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(localTable.tableMeta.intentId, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                localTable.tableRows = tableRows;
                setLocalTable(cloneDeep(localTable));
            }
        })();
        return () => {
            if (localTable) {
                localTable.tableRows = [];
                setLocalTable(cloneDeep(localTable));
            }
        };
    }, [intentId, localTable?.tableMeta.tableType]);

    const modifier = new TwoNestedCategoriesModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
            localTable.tableMeta = localTable.tableMeta;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addOuterCategory = async () => {
        if (localTable) await modifier.addOuterCategory(localTable.tableRows, repository, intentId, tableId);
    };

    const addInnerCategory = async () => {
        if (localTable) await modifier.addInnerCategory(localTable.tableRows, repository, intentId, tableId);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const newTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(localTable.tableMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.SavePricingStrategy<TwoNestedCategoryResource[]>(
                    intentId,
                    PricingStrategyTypes.TwoNestedCategory,
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
            <TwoNestedCategoriesContainer
                addInnerCategory={addInnerCategory}
                tableData={localTable.tableRows}
                modifier={modifier}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addOuterCategory} color="primary" variant="contained">
                            Add Outer Category
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} properties={["category", "subCategory"]} />}
        </>
    ) : (
        <></>
    );
};
