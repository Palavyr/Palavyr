import React from "react";
import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableTypes, IDynamicTableProps } from "../../DynamicTableTypes";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";

const useStyles = makeStyles((theme) => ({
    root: {
        borderTop: "3px solid red",
    },
    tableStyles: {
        background: "transparent"
    },
    trayWrapper: {
        width: "100%"
    },
    add: {},
    alignLeft: {
        float: "left",
    },
    alignRight: {
        float: "right"
    },
}));

export const PercentOfThreshold = ({ tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: IDynamicTableProps) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new PercentOfThresholdModifier(setTableData);

    const addItemOnClick = () => {
        modifier.addItem(tableData, client, areaIdentifier, tableId);
    };

    const addRowOnClickFactory = (itemId: string) => () => {
        modifier.addRow(tableData, client, areaIdentifier, tableId, itemId);
    };

    const onSave = async () => {
        const { data } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.PercentOfThreshold, tableData, tableId, tableTag);
        setTableData(tableData);
        return true;
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
        </>
    );
};
