import React from "react";
import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
import classNames from "classnames";


const useStyles = makeStyles<{}>((theme: any) => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
    },
    text: {},
    row: {},
    noRight: {},
}));

export interface IBasicThresholdHeader {}

export const BasicThresholdHeader = ({}: IBasicThresholdHeader) => {
    const cls = useStyles();
    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Threshold
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Amount
                </TableCell>
                <TableCell align="center" width="11ch" className={classNames(cls.cell)}>
                    Max Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Range or Value
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
