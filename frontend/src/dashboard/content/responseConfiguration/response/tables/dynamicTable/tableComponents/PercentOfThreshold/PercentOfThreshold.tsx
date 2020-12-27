import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, Checkbox, FormControlLabel, makeStyles, Paper, Table, TableContainer } from "@material-ui/core";
import React from "react";
import { IDynamicTableBody, IDynamicTableProps } from "../../DynamicTableTypes";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";

const useStyles = makeStyles((theme) => ({
    root: {},
    tableStyles: {},
    trayWrapper: {},
    add: {},
    alignLeft: {},
    alignRight: {},
}));


export const PercentOfThresholdHeader = () => {
    return <div>Percent of Threshold TableHeader</div>;
};

export const PercentOfThresholdBody = ({ tableData, modifier }: IDynamicTableBody) => {
    return <div>TableBody</div>;
};

export const PercentOfThreshold = ({ tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: IDynamicTableProps) => {
    const client = new ApiClient();
    const classes = useStyles();

    const onclick = () => {alert("clicked")}

    const modifier = new PercentOfThresholdModifier(onclick);

    const addOptionOnClick = () => {
        return null;
    };

    const onSave = () => {
        return null;
    };

    const useOptionsAsPathsOnChange = () => {
        return null;
    };

    return (
        <>
            <TableContainer className={classes.tableStyles} component={Paper}>
                <Table>
                    <PercentOfThresholdHeader />
                    <PercentOfThresholdBody tableData={tableData} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addOptionOnClick} color="primary" variant="contained">
                            Add Option
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={tableMeta.valuesAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={() => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
        </>
    );
};
