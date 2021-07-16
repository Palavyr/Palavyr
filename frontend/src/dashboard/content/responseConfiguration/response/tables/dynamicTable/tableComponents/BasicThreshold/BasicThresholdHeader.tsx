import React from "react";
import { TableHead, TableRow, TableCell, makeStyles, Button } from "@material-ui/core";
import classNames from "classnames";
import { TableData } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";

const useStyles = makeStyles((theme) => ({
    cell: {
        borderRight: "1px solid gray",
    },
    text: {
        fontSize: "16pt",
        // fontWeight: "bold"
    },
    row: {
        // borderBottom: "3px solid black"
    },
    noRight: {
        borderRight: "0px solid white",
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
                <TableCell align="center">
                    <Button
                        onClick={() => {
                            modifier.reorderThresholdData(tableData);
                            modifier.setTables(tableData);
                        }}
                    >
                        Sort By Threshold
                    </Button>
                </TableCell>
            </TableRow>
        </TableHead>
    );
};
