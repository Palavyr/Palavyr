import { makeStyles, TableCell } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    tableCell: {
        textAlign: "center",
    },
}));
export interface EnquiryTableRowCellProps {
    children: React.ReactNode;
}
export const EnquiryTableRowCell = ({ children }: EnquiryTableRowCellProps) => {
    const cls = useStyles();
    return <TableCell className={cls.tableCell}>{children}</TableCell>;
};
