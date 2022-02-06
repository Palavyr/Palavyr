import React from "react";
import { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";
import classNames from "classnames";
import { TableData } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.common.white}`,
    },
    text: {
        fontSize: "16pt",
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        borderRight: `0px solid ${theme.palette.common.white}`,
    },
}));

export interface IBasicThresholdHeader {
    tableData: TableData;
    modifier: BasicThresholdModifier;
}

export const BasicThresholdHeader = ({ tableData, modifier }: IBasicThresholdHeader) => {
    const cls = useStyles();
    const variantHeader = "h5";
    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={variantHeader}>Threshold</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={variantHeader}>Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <PalavyrText variant={variantHeader}> Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">
                    <Button
                        variant="contained"
                        onClick={() => {
                            modifier.reorderThresholdData(tableData);
                            modifier.setTables(tableData);
                        }}
                    >
                        <PalavyrText variant={variantHeader}>Sort By Threshold</PalavyrText>
                    </Button>
                </TableCell>
            </TableRow>
        </TableHead>
    );
};
