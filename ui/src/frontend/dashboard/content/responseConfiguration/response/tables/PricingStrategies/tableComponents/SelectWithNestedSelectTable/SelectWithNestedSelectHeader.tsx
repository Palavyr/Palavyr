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

export const TwoNestedCategoriesHeader = ({ show }: { show: boolean }) => {
    const cls = useStyles();

    return show ? (
        <TableHead>
            <TableRow>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && "Outer Category"}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && "Inner Category"}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && "Amount"}
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    {show && "Max Amount"}
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    ) : (
        <></>
    );
};
