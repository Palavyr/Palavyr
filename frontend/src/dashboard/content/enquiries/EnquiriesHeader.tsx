import { makeStyles, TableRow, TableCell } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

const useStyles = makeStyles((theme) => ({
    headerCell: {
        fontWeight: "bold",
        fontSize: "16pt",
        textAlign: "center",
    },
    headerRow: {
        borderBottom: "3px solid black",
    },
}));

export const EnquiriesHeader = () => {
    const cls = useStyles();

    return (
        <TableRow className={cls.headerRow}>
            <TableCell className={classNames(cls.headerCell)}></TableCell>
            <TableCell className={classNames(cls.headerCell)}>Client</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Email</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Phone Number</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Conversation</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Estimate</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Area</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Time</TableCell>
            <TableCell className={classNames(cls.headerCell)}>Seen</TableCell>
        </TableRow>
    );
};
