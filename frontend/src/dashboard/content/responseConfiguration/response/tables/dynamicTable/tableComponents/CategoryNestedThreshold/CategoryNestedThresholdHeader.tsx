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

    return (
        <TableHead>
            <TableRow>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Category
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Threshold
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.text, cls.noRight)}>
                    Max Amount (if range)
                </TableCell>
                <TableCell align="center"></TableCell>
                <TableCell align="center">Delete</TableCell>
                <TableCell align="center">
                    <Button
                        onClick={() => {
                            modifier.reorderThresholdData(tableData);
                            modifier.setTables(tableData);
                        }}
                    >
                        Reorder Thresholds
                    </Button>
                </TableCell>
            </TableRow>
        </TableHead>
    );
};
