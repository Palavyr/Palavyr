import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";
import { CategoryNestedThresholdData } from "@Palavyr-Types";
import classNames from "classnames";
import React from "react";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";

const useStyles = makeStyles(theme => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
    },
    text: {
        // fontSize: "16pt",
    },
    noRight: {
        borderRight: `0px solid ${theme.palette.common.white}`,
    },
}));

export interface ICategoryNestedThresholdHeader {
    tableData: CategoryNestedThresholdData[];
    modifier: CategoryNestedThresholdModifier;
}

export const CategoryNestedThresholdHeader = ({ tableData, modifier }: ICategoryNestedThresholdHeader) => {
    const cls = useStyles();

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText> Category</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText> Threshold</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText> Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <PalavyrText> Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">
                    <PalavyrText>Delete</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
