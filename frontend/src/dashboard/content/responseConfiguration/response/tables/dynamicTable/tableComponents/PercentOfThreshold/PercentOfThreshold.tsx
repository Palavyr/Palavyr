import React from "react";
import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, Checkbox, FormControlLabel, makeStyles, Paper, TableContainer } from "@material-ui/core";
import { DynamicTableTypes, IDynamicTableProps } from "../../DynamicTableTypes";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";

const useStyles = makeStyles((theme) => ({
    root: {
        borderTop: "3px solid red",
    },
    tableStyles: {},
    trayWrapper: {},
    add: {},
    alignLeft: {},
    alignRight: {},
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
    };

    const useOptionsAsPathsOnChange = () => {
        return null;
    };

    return (
        <>
            <TableContainer className={classes.tableStyles} component={Paper}>
                <PercentOfThresholdContainer tableData={tableData} modifier={modifier} addRowOnClickFactory={addRowOnClickFactory} />
            </TableContainer>
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Item
                        </Button>
                        {/* <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={tableMeta.valuesAsPaths} onChange={useOptionsAsPathsOnChange} />} /> */}
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={() => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
        </>
    );
};
