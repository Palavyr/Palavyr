import { TableCell } from "@material-ui/core";
import React from "react";

export const Cell = ({ children }: { children?: React.ReactNode }) => {
    return (
        <TableCell style={{ width: "30ch" }} align="center">
            {children}
        </TableCell>
    );
};
