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
        margin: "1.2rem",
        // background: "#E8E8E8",
        background: "#C7ECEE",
        padding: "1.3rem",
        border: `3px dashed ${theme.palette.common.black}`,
        borderRadius: "7px",
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
    headerText: {
        fontSize: "16pt",
        fontWight: "bold"
    }

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
                    <TableRow style={{borderBottom: "1px solid black"}}>
                        <TableCell align={cellAlignment} className={classes.headerText}></TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} >Description</TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} >Amount</TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} >Max Amount (if range)</TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} >Range</TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} >Per Person</TableCell>
                        <TableCell align={cellAlignment} className={classes.headerText} ></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody style={{borderTop: "2px solid black"}}>
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

