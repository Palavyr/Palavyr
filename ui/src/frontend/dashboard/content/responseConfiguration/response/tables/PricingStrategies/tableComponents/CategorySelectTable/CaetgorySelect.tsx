import React, { useContext, useEffect, useState } from "react";
import { CategorySelectModifier } from "./CategorySelectModifier";
import { TableContainer, Paper, Table, Button, FormControlLabel, Checkbox, AccordionActions, makeStyles } from "@material-ui/core";
import { CategorySelectHeader } from "./CategorySelectHeader";
import { CategorySelectBody } from "./CategorySelectBody";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { PricingStrategy, PricingStrategyProps } from "@Palavyr-Types";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { DisplayTableData } from "../DisplayTableData";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { cloneDeep } from "lodash";
import { CategorySelectTableRowResource } from "@common/types/api/EntityResources";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { PricingStrategyHeader } from "../../PricingStrategyTableHeader";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    tableStyles: {
        width: "100%",
        backgroundColor: "transparent",
        border: "none",
        borderBottom: `4px solid ${theme.palette.primary.main}`,
        boxShadow: "none",
    },
    table: {
        border: "none",
        boxShadow: "none",
    },
    alignLeft: {
        position: "relative",
        top: "50%",
        float: "left",
        paddingLeft: "0.3rem",
    },
    alignRight: {
        position: "relative",
        top: "50%",
        transform: "translateY(25%)",
        float: "right",
        paddingRight: "0.3rem",
        height: "100%",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {
        marginRight: "0.4rem",
    },
}));

export const CategorySelect = ({ showDebug, tableId, setTables, intentId, deleteAction, tables, tableIndex, availablePricingStrategyOptions, tableNameMap, unitTypes, inUse, table }: PricingStrategyProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [localTable, setLocalTable] = useState<PricingStrategy>();
    const [useOptionsAsPaths, setUseOptionsAsPaths] = useState<boolean>(false);

    useEffect(() => {
        setLocalTable(table);
        const useOptionsAsPaths = table.tableMeta.valuesAsPaths;
        setUseOptionsAsPaths(useOptionsAsPaths);
    }, [intentId, table, tables, table.tableRows, localTable?.tableMeta.unitIdEnum, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        (async () => {
            if (localTable) {
                const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(localTable.tableMeta.intentId, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                localTable.tableRows = tableRows;
                setLocalTable(cloneDeep(localTable));
            }
        })();
    }, [intentId, localTable?.tableMeta.tableType]);

    const modifier = new CategorySelectModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const useOptionsAsPathsOnChange = async (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        setUseOptionsAsPaths(checked);
    };

    const addOptionOnClick = async () => {
        if (localTable) await modifier.addOption(localTable.tableRows, repository, intentId, tableId);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.SavePricingStrategy<CategorySelectTableRowResource[]>(
                    intentId,
                    PricingStrategyTypes.CategorySelect,
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
            <TableContainer className={cls.tableStyles} component={Paper}>
                <Table className={cls.table}>
                    <CategorySelectHeader />
                    <CategorySelectBody tableData={localTable.tableRows} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addOptionOnClick} color="primary" variant="contained">
                            Add Option
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={useOptionsAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel position="center" onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} properties={["option", "valueMin", "valueMax", "range", "rowOrder"]} />}
        </>
    ) : (
        <></>
    );
};
