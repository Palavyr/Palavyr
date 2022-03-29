import { TableHead, TableRow, TableCell as TC, makeStyles, TableCellProps } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        height: "10px",
        // borderRight: `1px solid ${theme.palette.grey[300]}`,
    },
    noRight: {
        borderRight: "0px solid white",
    },
    head: {
        border: "none",
        boxShadow: "none",
        height: "10px",
    },
    row: {
        border: "none",
        boxShadow: "none",
        height: "10px",
    },
    cellInner: {
        height: "10px",
        lineHeight: "0px",
        padding: "5px",
    },
}));

const TableCell = (props: TableCellProps) => {
    const cls = useStyles();
    return <TC classes={{ root: cls.cellInner }} className={cls.cellInner} {...props} />;
};

export const SelectOneFlatHeader = () => {
    const cls = useStyles();
    const variantHeader = "h4";

    return (
        <TableHead className={cls.head}>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <h4>Option Name</h4>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    <h4>Amount</h4>
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell, cls.noRight)}>
                    <h4>Max Amount</h4>
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
