import React from "react";
import { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";
import classNames from "classnames";
import { TableData } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyles = makeStyles(theme => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
    },
    text: {
        // fontSize: theme.typography.body1.fontSize,
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        // borderRight: `0px solid ${theme.palette.common.white}`,
    },
}));

export interface IBasicThresholdHeader {
    tableData: TableData;
    modifier: BasicThresholdModifier;
}

export const BasicThresholdHeader = ({ tableData, modifier }: IBasicThresholdHeader) => {
    const cls = useStyles();
    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>Threshold</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <PalavyrText> Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center">
                    <PalavyrText align="center" className={classNames(cls.cell)}>
                        Range or Value
                    </PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
