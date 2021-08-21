import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableHead, TableRow, TableCell, makeStyles, Tooltip } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        borderRight: `1px solid ${theme.palette.common.white}`,
    },
    text: {
        fontSize: "16pt",
    },
    noRight: {
        borderRight: "0px solid white",
    },
}));

export const TwoNestedCategoriesHeader = () => {
    const cls = useStyles();
    const headerVariant = "h5";

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}> Outer Category</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>Inner Category</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <PalavyrText variant={headerVariant}>Amount</PalavyrText>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <Tooltip title="If Range">
                        <PalavyrText variant={headerVariant}>Max Amount</PalavyrText>
                    </Tooltip>
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">
                    <PalavyrText variant={headerVariant}>Delete</PalavyrText>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
