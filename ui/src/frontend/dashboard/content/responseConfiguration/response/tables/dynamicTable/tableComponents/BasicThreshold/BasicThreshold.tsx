import React, { useContext, useEffect } from "react";
import { PricingStrategy, PricingStrategyProps } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { BasicThresholdHeader } from "./BasicThresholdHeader";
import { BasicThresholdBody } from "./BasicThresholdBody";
import { useState } from "react";
import { Button, makeStyles, Table, AccordionActions } from "@material-ui/core";
import { DisplayTableData } from "../DisplayTableData";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { cloneDeep } from "lodash";
import { useIsMounted } from "@common/hooks/useIsMounted";
import { PricingStrategyHeader } from "../../PricingStrategyTableHeader";
import { BasicThresholdResource } from "@common/types/api/EntityResources";

const useStyles = makeStyles(theme => ({
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
    input: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
    container: {
        borderBottom: `4px solid ${theme.palette.primary.main}`,
    },
}));

export const BasicThreshold = ({ showDebug, tableId, setTables, intentId, deleteAction, tables, tableIndex, availablePricingStrategyOptions, tableNameMap, unitTypes, inUse, table }: PricingStrategyProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [localTable, setLocalTable] = useState<PricingStrategy>();
    const isMounted = useIsMounted();

    useEffect(() => {
        if (isMounted) {
            setLocalTable(table);
        }
    }, [intentId, table, tables, table.tableRows, localTable?.tableMeta.unitIdEnum, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        if (isMounted) {
            (async () => {
                if (localTable) {
                    const { tableRows } = await repository.Configuration.Tables.Dynamic.GetPricingStrategyRows(localTable.tableMeta.intentId, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                    localTable.tableRows = tableRows;
                    setLocalTable(cloneDeep(localTable));
                }
            })();
        }
    }, [intentId, localTable?.tableMeta.tableType]);

    const modifier = new BasicThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addThresholdOnClick = async () => {
        if (localTable) await modifier.addThreshold(localTable.tableRows, intentId, tableId, repository);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.SavePricingStrategy<BasicThresholdResource[]>(
                    intentId,
                    PricingStrategyTypes.BasicThreshold,
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

    const onItemNameChange = (event: { preventDefault: () => void; target: { value: string } }) => {
        event.preventDefault();
        if (localTable) {
            modifier.setItemName(localTable.tableRows, event.target.value);
        }
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
            <div className={cls.container}>
                <Table>
                    <BasicThresholdHeader />
                    <BasicThresholdBody tableData={localTable.tableRows} modifier={modifier} unitPrettyName={localTable.tableMeta.unitPrettyName} unitGroup={localTable.tableMeta.unitGroup} />
                </Table>
            </div>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addThresholdOnClick} color="primary" variant="contained">
                            Add Threshold
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
