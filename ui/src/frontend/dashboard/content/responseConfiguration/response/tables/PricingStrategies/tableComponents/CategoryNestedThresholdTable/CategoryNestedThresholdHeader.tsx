import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";


const useStyles = makeStyles<{}>((theme: any) => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
    },
    text: {},
    noRight: {},
}));

export interface ICategoryNestedThresholdHeader {}

export const CategoryNestedThresholdHeader = ({}: ICategoryNestedThresholdHeader) => {
    const cls = useStyles();

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Category
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Threshold
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    Max Amount
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
