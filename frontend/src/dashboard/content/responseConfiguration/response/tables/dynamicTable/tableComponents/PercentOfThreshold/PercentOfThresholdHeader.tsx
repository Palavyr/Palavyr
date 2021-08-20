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
    button: {

    }
}));

export interface IPercentOfThresholdHeader {
    tableData: PercentOfThresholdData[];
    modifier: PercentOfThresholdModifier;
}

export const PercentOfThresholdHeader = ({ tableData, modifier }: IPercentOfThresholdHeader) => {
    const cls = useStyles();

    return (
        <TableHead>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    If exceeds Threshold
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Add or subtract
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    % of Value
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    From base Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Max Amount (if range)
                </TableCell>
                <TableCell align="center">
                    <Button
                        className={cls.button}
                        onClick={() => {
                            const reordered = modifier.reorderThresholdData(tableData);
                            modifier.setTables(reordered);
                        }}
                    >
                        Reorder thresholds
                    </Button>
                </TableCell>
            </TableRow>
        </TableHead>
    );
};
