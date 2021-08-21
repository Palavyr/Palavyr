import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableHead, TableRow, TableCell, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.grey[300]}`,
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
    const variantHeader = "h5";

    return (
        <TableHead className={cls.head}>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={variantHeader}>Option Name</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={variantHeader}>Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <PalavyrText variant={variantHeader}>Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
