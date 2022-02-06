import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableProps, PercentOfThresholdData } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DynamicTableHeader } from "../../DynamicTableHeader";

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
    tables,
    setTables,
    tableMetaIndex,
    areaIdentifier,
    deleteAction,
    onSaveFactory,
    availableDynamicTableOptions,
    tableNameMap,
    unitTypes,
    tableTag,
    setTableTag,
    inUse,
    setLocalTable,
    localTable,
}: DynamicTableProps) => {
    const { repository } = useContext(DashboardContext);
    const classes = useStyles();

    const [tableRows, setTableRows] = useState<PercentOfThresholdData[]>([]);
    useEffect(() => {
        const tableRows = tables[tableMetaIndex].tableRows;
        setTableRows(tableRows);
    }, []);
    const modifier = new PercentOfThresholdModifier(updatedRows => {
        setTableRows(updatedRows);
    });

    const addItemOnClick = () => modifier.addItem(tableRows, repository, areaIdentifier, tableId);
    const addRowOnClickFactory = (itemId: string) => () => modifier.addRow(tableRows, repository, areaIdentifier, tableId, itemId);

    const onSave = onSaveFactory(modifier, DynamicTableTypes.PercentOfThreshold, () => {});

    return (
        <>
            <DynamicTableHeader
                localTable={localTable}
                setLocalTable={setLocalTable}
                setTables={setTables}
                availableDynamicTableOptions={availableDynamicTableOptions}
                tableNameMap={tableNameMap}
                unitTypes={unitTypes}
                inUse={inUse}
                tableTag={tableTag}
                setTableTag={setTableTag}
            />
            <PercentOfThresholdContainer tableData={tableRows} modifier={modifier} addRowOnClickFactory={addRowOnClickFactory} />
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Item
                        </Button>
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableRows} />}
        </>
    );
};
