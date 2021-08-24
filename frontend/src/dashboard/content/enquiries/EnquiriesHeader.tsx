import { PalavyrCheckbox } from "@common/components/PalavyrCheckBox";
import { makeStyles, TableRow, TableCell, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles(theme => ({
    headerRow: {
        borderBottom: "3px solid black",
    },
}));

export interface EnquiresHeaderProps {
    checked: boolean;
    onChange(): void;
    disabled: boolean;
}

export const EnquiriesHeader = ({ checked, onChange, disabled }: EnquiresHeaderProps) => {
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
                    Area
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Time
                </Typography>
            </TableCell>
            <TableCell>
                <Typography align="center" variant="h5">
                    Seen
                </Typography>
            </TableCell>
            <TableCell>
                <PalavyrCheckbox label="Select All" checked={checked} onChange={onChange} disabled={disabled} />
            </TableCell>
        </TableRow>
    );
};
