import { makeStyles, TableCell, TableHead, TableRow } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: "1px solid gray"
    },
    text: {
        fontSize: "12pt",
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        borderRight: "0px solid white"
    }
}))

export const PercentOfThresholdHeader = () => {
    const classes = useStyles();

    return (
        <TableHead>
            <TableRow className={classes.row}>
                <TableCell align="center" ></TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} >Threshold</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} >Amount</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} >Max Amount (if range)</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} > % of Value</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text, classes.noRight)} > Add or subtract</TableCell>
                <TableCell align="center" ></TableCell>
            </TableRow>
        </TableHead>
    )
}