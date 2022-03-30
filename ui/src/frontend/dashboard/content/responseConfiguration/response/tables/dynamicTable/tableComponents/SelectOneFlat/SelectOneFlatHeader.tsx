import { TableHead, TableRow, TableCell as TC, makeStyles, TableCellProps } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles(theme => ({
    cell: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.fontWeightBold,
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
        padding: "15px",
    },
}));

const TableCell = (props: TableCellProps) => {
    const cls = useStyles();
    return <TC classes={{ root: cls.cellInner }} className={cls.cellInner} {...props} />;
};

export const SelectOneFlatHeader = () => {
    const cls = useStyles();

    return (
        <TableHead className={cls.head}>
            <TableRow className={cls.row}>
                <TableCell align="center"></TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Option Name
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Amount
                </TableCell>
                <TableCell align="center" className={classNames(cls.cell)}>
                    Max Amount
                </TableCell>
                <TableCell align="center"></TableCell>
            </TableRow>
        </TableHead>
    );
};
