import { makeStyles, TableRow, TableCell, Typography } from "@material-ui/core";
import React from "react";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    headerRow: {
        borderBottom: `2px solid ${theme.palette.common.black}`,
    },
}));

export interface EnquiresHeaderProps {}

export const EnquiriesHeader = ({}: EnquiresHeaderProps) => {
    const cls = useStyles();

    return (
        <TableRow className={cls.headerRow}>
            <TableCell></TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Client
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Email
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Phone Number
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Conversation
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Response Pdf
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Intent
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Time
                </Typography>
            </TableCell>
            <TableCell></TableCell>
        </TableRow>
    );
};
