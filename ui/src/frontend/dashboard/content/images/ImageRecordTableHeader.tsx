import { makeStyles, TableCell, TableHead, TableRow, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    cell: {
        textAlign: "center",
    },
}));
export const ImageRecordTableHeader = () => {
    return (
        <TableHead>
            <TableRow>
                <TableCell></TableCell>
                <TableCell>
                    <Typography variant="body1" align="center">
                        File Name
                    </Typography>
                </TableCell>
                <TableCell>
                    <Typography variant="body1" align="center">
                        Preview
                    </Typography>
                </TableCell>
                <TableCell></TableCell>
            </TableRow>
        </TableHead>
    );
};
