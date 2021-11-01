import React, { useContext } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableProps, PercentOfThresholdData } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles((theme) => ({
    root: {
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

export const PercentOfThreshold = ({ showDebug, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: Omit<DynamicTableProps, "tableMeta" | "setTableMeta">) => {
    const { repository } = useContext(DashboardContext);
    const classes = useStyles();

    const modifier = new PercentOfThresholdModifier(setTableData);

    const addItemOnClick = () => modifier.addItem(tableData, repository, areaIdentifier, tableId);
    const addRowOnClickFactory = (itemId: string) => () => modifier.addRow(tableData, repository, areaIdentifier, tableId, itemId);

    const onSave = async () => {
        const reorderedData = modifier.reorderThresholdData(tableData);
        const result = modifier.validateTable(reorderedData);

        if (result) {
            const savedData = await repository.Configuration.Tables.Dynamic.saveDynamicTable<PercentOfThresholdData[]>(areaIdentifier, DynamicTableTypes.PercentOfThreshold, reorderedData, tableId, tableTag);
            setTableData(savedData);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            <PercentOfThresholdContainer tableData={tableData} modifier={modifier} addRowOnClickFactory={addRowOnClickFactory} />
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
            {showDebug && <DisplayTableData tableData={tableData} />}
        </>
    );
};
