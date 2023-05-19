import React from "react";
import { TableRow, TableCell, makeStyles, Typography } from "@material-ui/core";

const useStyles = makeStyles<{}>((theme: any) => ({
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
    itemName: string;
    value: string | React.ReactNode;
    rowStyle?: {};
    textStyle?: {};
}

export const PricingCardTableRow = ({ itemName, value, rowStyle, textStyle }: PricingCarTableRowProps) => {
    const cls = useStyles();
    return (
        <TableRow className={cls.tableRow}>
            <TableCell style={rowStyle} classes={{ root: cls.tableRoot }} className={cls.tablecellLeft} align="left">
                <Typography style={textStyle} align="left">
                    {itemName}
                </Typography>
            </TableCell>
            <TableCell style={rowStyle} classes={{ root: cls.tableRoot }} className={cls.tablecellRight} align="right">
                <Typography style={textStyle}>{value}</Typography>
            </TableCell>
        </TableRow>
    );
};
