import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
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

export const TwoNestedCategoriesHeader = ({ show }: { show: boolean }) => {
    const cls = useStyles();

    return show ? (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && <h5> Outer Category</h5>}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && <h5>Inner Category</h5>}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && <h5>Amount</h5>}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    {show && <h5>Max Amount</h5>}
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">{show && <h5>Delete</h5>}</TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    ) : (
        <></>
    );
};
