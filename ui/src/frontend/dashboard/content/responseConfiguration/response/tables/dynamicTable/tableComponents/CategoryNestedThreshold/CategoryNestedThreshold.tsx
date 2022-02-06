import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { CategoryNestedThresholdData, DynamicTableProps } from "@Palavyr-Types";

import { DisplayTableData } from "../DisplayTableData";
import { CategoryNestedThresholdContainer } from "./CategoryNestedThresholdContainer";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DynamicTableHeader } from "../../DynamicTableHeader";

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

    const [tableRows, setTableRows] = useState<CategoryNestedThresholdData[]>([]);
    useEffect(() => {
        const tableRows = tables[tableMetaIndex].tableRows;
        setTableRows(tableRows);
    }, []);

    const modifier = new CategoryNestedThresholdModifier(updatedRows => {
        setTableRows(updatedRows);
    });

    const onSave = onSaveFactory(modifier, DynamicTableTypes.BasicThreshold, () => {});

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
            <CategoryNestedThresholdContainer tableData={tableRows} modifier={modifier} tableId={tableId} areaIdentifier={areaIdentifier} />
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={() => modifier.addCategory(tableRows, repository, areaIdentifier, tableId)} color="primary" variant="contained">
                            Add Category
                        </Button>
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableRows} properties={["category", "triggerFallback", "threshold", "valueMin", "valueMax", "rowOrder", "rowId", "itemOrder", "itemId"]} />}
        </>
    );
};
