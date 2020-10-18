import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core"
import classNames from "classnames";
import React from "react"


const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: "1px solid gray"
    },
    text: {
        fontSize: "16pt",
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        borderRight: "0px solid white"
    }
}))

export const SelectOneFlatHeader = () => {
    const classes = useStyles();

    return (
        <TableHead>
            <TableRow className={classes.row}>
                <TableCell align="center" ></TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} >Option Name</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text)} >Amount</TableCell>
                <TableCell align="center" className={classNames(classes.cell, classes.text, classes.noRight)}>Max Amount (if range)</TableCell>
                <TableCell align="center" ></TableCell>
            </TableRow>
        </TableHead>
    )
}
