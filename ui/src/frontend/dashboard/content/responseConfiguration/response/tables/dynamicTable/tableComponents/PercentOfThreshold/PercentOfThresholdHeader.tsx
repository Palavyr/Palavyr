import { PalavyrText } from "@common/components/typography/PalavyrTypography";
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
                <TableCell style={{width: "0px"}} classes={{ body: cls.button }}></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>If exceeds</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText> </PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>% of</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText>Max Amount</PalavyrText>
                </TableCell>
                <TableCell align="center">
                    <PalavyrText align="center" className={classNames(cls.cell)}>
                        Range or Value
                    </PalavyrText>
                </TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
    );
};
