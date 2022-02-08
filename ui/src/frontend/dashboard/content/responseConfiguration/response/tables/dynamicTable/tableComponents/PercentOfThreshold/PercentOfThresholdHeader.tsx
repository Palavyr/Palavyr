import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { Button, makeStyles, TableCell, TableHead, TableRow } from "@material-ui/core";
import { PercentOfThresholdData } from "@Palavyr-Types";
import classNames from "classnames";
import React from "react";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.common.white}`,
    },
    text: {
        fontSize: "12pt",
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        borderRight: "0px solid white",
    },
    button: {},
}));

export interface IPercentOfThresholdHeader {
    tableData: PercentOfThresholdData[];
    modifier: PercentOfThresholdModifier;
}

export const PercentOfThresholdHeader = ({ tableData, modifier }: IPercentOfThresholdHeader) => {
    const cls = useStyles();
    const headerVariant = "h5";

    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>If exceeds</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>Add or subtract</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>% of</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>From Base Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center">
                    <Button
                        className={cls.button}
                        onClick={() => {
                            const reordered = modifier.reorderThresholdData(tableData);
                            modifier.setTables(reordered);
                        }}
                    >
                        <PalavyrText variant="body1">Reorder thresholds</PalavyrText>
                    </Button>
                </TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
    );
};
