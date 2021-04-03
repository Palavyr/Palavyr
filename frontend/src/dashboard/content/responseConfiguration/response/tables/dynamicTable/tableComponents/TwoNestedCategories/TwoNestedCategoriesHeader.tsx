import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles((theme) => ({
    cell: {
        borderRight: "1px solid gray",
    },
    text: {
        fontSize: "16pt",
    },
    noRight: {
        borderRight: "0px solid white",
    },
}));

export const TwoNestedCategoriesHeader = () => {
    const cls = useStyles();

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Outer Category
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Inner Category
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text, cls.noRight)}>
                    Max Amount (if range)
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">Delete</TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
