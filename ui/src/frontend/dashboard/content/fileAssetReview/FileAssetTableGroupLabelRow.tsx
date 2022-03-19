import { TableRow, TableCell, Typography } from "@material-ui/core";
import React from "react";

export const FileAssetTableGroupLabelRow = ({ extension }: { extension: string }) => {
    return (
        <TableRow>
            <TableCell>
                <Typography align="center" variant="h4">
                    {extension.toLocaleUpperCase()}
                </Typography>
            </TableCell>
            <TableCell></TableCell>
            <TableCell></TableCell>
        </TableRow>
    );
};
