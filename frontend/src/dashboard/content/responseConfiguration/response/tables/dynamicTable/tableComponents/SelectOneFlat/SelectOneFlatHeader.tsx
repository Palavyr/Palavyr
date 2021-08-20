import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.grey[300]}`,
    },
    text: {
        fontSize: "16pt",
    },
    noRight: {
        borderRight: "0px solid white",
    },
    head: {
        border: "none",
        boxShadow: "none",
    },
    row: {
        border: "none",
        boxShadow: "none",
    },
}));

export const SelectOneFlatHeader = () => {
    const cls = useStyles();

    return (
        <TableHead className={cls.head}>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Option Name
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text, cls.noRight)}>
                    Max Amount (if range)
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
