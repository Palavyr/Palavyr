import { makeStyles, TableCell, TableHead, TableRow } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
    },
    text: {},
    row: {},
    noRight: {},
    button: { width: "0px" },
}));

export const PercentOfThresholdHeader = () => {
    const cls = useStyles();

    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell style={{ width: "0px" }} classes={{ body: cls.button }}></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    If exceeds
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    % of
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Max Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Range or Value
                </TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
    );
};