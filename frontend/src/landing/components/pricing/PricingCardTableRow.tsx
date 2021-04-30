import React from "react";
import { TableRow, TableCell, makeStyles, Typography } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    tableRow: {
        borderBottom: "2px solid lightgray",
        margin: "0px",
    },
    tableRoot: {
        paddingTop: "0.8rem",
        paddingBottom: "0.8rem",
    },
    tablecellLeft: {
        padding: "0px",
        fontSize: "16px",
        color: "white",
    },
    tablecellRight: {
        textAlign: "center",
        fontSize: "16px",
        color: "white",
    },
}));

export interface PricingCarTableRowProps {
    left: string;
    right: string | React.ReactNode;
}

export const PricingCardTableRow = ({ left, right }: PricingCarTableRowProps) => {
    const cls = useStyles();
    return (
        <TableRow className={cls.tableRow}>
            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                <Typography align="left">{left}</Typography>
            </TableCell>
            <TableCell classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                <Typography>{right}</Typography>
            </TableCell>
        </TableRow>
    );
};
