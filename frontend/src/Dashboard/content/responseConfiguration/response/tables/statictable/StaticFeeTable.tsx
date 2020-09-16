import React from "react";
import { makeStyles, TextField, Divider, Button, TableRow, Table, TableHead, TableBody, TableCell } from "@material-ui/core";
import { StaticTableMetas, StaticTableMeta, StaticTableRow } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import { StaticRow } from "./StaticRow";
import DeleteIcon from '@material-ui/icons/Delete';
import AddBoxIcon from '@material-ui/icons/AddBox';
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward';
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward';
import DeleteOutlineIcon from '@material-ui/icons/DeleteOutline';



const useStyles = makeStyles((theme => ({
    staticFees: {
        background: "#E8E8E8",
        margin: "1rem",
        padding: "2.6rem",
        border: "3px dotted black",
        borderRadius: "7px",
        marginTop: "1.5rem",
        marginBottom: "1.5rem",
    },
    tableDescription: {
        marginBottom: "0.3rem",
        paddingBottom: "1.5rem",
        borderRadius: "3px",
        minWidth: "50%"
    },

    buttonWrapper: {
        marginTop: "1.5rem",
        marginBottom: "0.5rem",
    },

    feeTableButton: {
        marginLeft: "0.6rem",
        marginRight: "0.6rem",
        padding: "0.4rem",
        paddingLeft: "0.6rem",
        paddingRight: "0.6rem"
    },

})));

export interface IStaticFeeTable {
    staticTableMetas: StaticTableMetas;
    staticTableMeta: StaticTableMeta;
    tableModifier: StaticTablesModifier;
}

// we can produce a list of these components depending on the
export const StaticFeeTable = ({ staticTableMetas, staticTableMeta, tableModifier }: IStaticFeeTable) => {
    const classes = useStyles();
    const cellAlignment = "center";

    return (
        <div className={classes.staticFees}>
            <TextField
                className={classes.tableDescription}
                multiline
                rows={3}
                value={staticTableMetas[staticTableMeta.tableOrder].description}
                label="Table Description"
                onChange={(event) => {
                    tableModifier.setTableDescription(staticTableMetas, staticTableMeta.tableOrder, event.target.value);
                }}
            />
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell align={cellAlignment} ></TableCell>
                        <TableCell align={cellAlignment} >Description</TableCell>
                        <TableCell align={cellAlignment} >Amount</TableCell>
                        <TableCell align={cellAlignment} ></TableCell>
                        <TableCell align={cellAlignment} >Range</TableCell>
                        <TableCell align={cellAlignment} >Per Person</TableCell>
                        <TableCell align={cellAlignment} ></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {staticTableMeta.staticTableRows.map((row: StaticTableRow, index: number) => (
                        <StaticRow
                            key={row.id}
                            index={index}
                            staticTableMetas={staticTableMetas}
                            tableOrder={staticTableMeta.tableOrder}
                            rowOrder={row.rowOrder}
                            modifier={tableModifier}
                            minFee={row.fee.min}
                            maxFee={row.fee.max}
                            rangeState={row.range}
                            perState={row.perPerson}
                            description={row.description}
                        />
                    ))}
                </TableBody>
            </Table>
            {/* <Divider /> */}
            <div className={classes.buttonWrapper}>
                <Button startIcon={<AddBoxIcon />} variant="contained" size="small" color="primary" className={classes.feeTableButton} onClick={() => tableModifier.addRow(staticTableMetas, staticTableMeta.tableOrder)}>
                    Add Row
                </Button>

                {!tableModifier.isTableLastPosition(staticTableMetas, staticTableMeta.tableOrder) && (
                    <Button startIcon={<ArrowDownwardIcon />} variant="contained" size="small" color="primary" className={classes.feeTableButton} onClick={() => tableModifier.moveTableDown(staticTableMetas, staticTableMeta.tableOrder)}>
                        Shift Down
                    </Button>
                )}

                {!tableModifier.isTableFirstPosition(staticTableMeta.tableOrder) && (
                    <Button startIcon={<ArrowUpwardIcon />} variant="contained" size="small" color="primary" className={classes.feeTableButton} onClick={() => tableModifier.moveTableUp(staticTableMetas, staticTableMeta.tableOrder)}>
                        Shift Up
                    </Button>
                )}

                <Button startIcon={<DeleteOutlineIcon />} variant="contained" color="secondary" size="small" className={classes.feeTableButton} onClick={() => tableModifier.delTable(staticTableMetas, staticTableMeta.tableOrder)}>
                    Remove Table
                </Button>
            </div>
        </div>
    );
};

