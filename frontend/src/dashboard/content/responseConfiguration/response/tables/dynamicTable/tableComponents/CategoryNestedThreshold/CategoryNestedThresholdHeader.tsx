import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";
import { CategoryNestedThresholdData } from "@Palavyr-Types";
import classNames from "classnames";
import React from "react";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.common.white}`,
    },
    text: {
        fontSize: "16pt",
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
    const headerVariant = "h5";

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}> Category</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}> Threshold</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}> Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <PalavyrText variant={headerVariant}> Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">
                    <PalavyrText variant={headerVariant}>Delete</PalavyrText>
                </TableCell>
                <TableCell align="center">
                    <Button
                        onClick={() => {
                            modifier.reorderThresholdData(tableData);
                            modifier.setTables(tableData);
                        }}
                    >
                        <PalavyrText variant={headerVariant}> Reorder Thresholds</PalavyrText>
                    </Button>
                </TableCell>
            </TableRow>
        </TableHead>
    );
};
