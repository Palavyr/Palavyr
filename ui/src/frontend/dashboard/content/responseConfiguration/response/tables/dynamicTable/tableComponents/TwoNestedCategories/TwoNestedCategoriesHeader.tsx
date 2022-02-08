import { TableHead, TableRow, TableCell, makeStyles, Tooltip } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.common.white}`,
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
                <TableCell align="center" className={classNames(cls.cell)}>
                    <h5> Outer Category</h5>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <h5>Inner Category</h5>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <h5>Amount</h5>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <Tooltip title="If Range">
                        <h5>Max Amount</h5>
                    </Tooltip>
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">
                    <h5>Delete</h5>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
