import React from "react";
import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableProps } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";
import { reOrderPercentOfThresholdTableData } from "./PercentOfThresholdUtils";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

const useStyles = makeStyles((theme) => ({
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

export const PercentOfThreshold = ({ showDebug, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: Omit<DynamicTableProps, "tableMeta" | "setTableMeta">) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new PercentOfThresholdModifier(setTableData);

    const addItemOnClick = () => modifier.addItem(tableData, client, areaIdentifier, tableId);
    const addRowOnClickFactory = (itemId: string) => () => modifier.addRow(tableData, client, areaIdentifier, tableId, itemId);

    const onSave = async () => {
        const reorderedData = reOrderPercentOfThresholdTableData(tableData);

        const result = modifier.validateTable(reorderedData);

        if (result){
            const { data: savedData } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.PercentOfThreshold, reorderedData, tableId, tableTag);
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
