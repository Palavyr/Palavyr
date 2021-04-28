import React from "react";
import { Grid, makeStyles, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@material-ui/core";
import { PreCheckError } from "@Palavyr-Types";

const useStyles = makeStyles((theme) => ({
    uppercell: {
        paddingTop: "1rem",
        paddingBottom: "2rem",
        borderTop: "4px solid black",
        borderBottom: "4px solid black",
    },

    table: {
        border: "0px solid black",
    },

    tablegrid: {
        paddingRight: "20%",
        paddingLeft: "20%",
    },
    cell: {
        borderBottom: "1px solid lightgray",
    },
}));

interface AreasInNeedOfAttentionProps {
    preCheckErrors: PreCheckError[];
}

export const AreasInNeedOfAttention = ({ preCheckErrors }: AreasInNeedOfAttentionProps) => {
    const cls = useStyles();

    return (
        <>
            {preCheckErrors.length > 0 && (
                <Grid className={cls.uppercell}>
                    <Grid className={cls.tablegrid}>
                        <Typography gutterBottom align="center" variant="h6">
                            Areas in need of attention:
                        </Typography>
                        <Table className={cls.table}>
                            <TableHead>
                                <TableRow>
                                    <TableCell className={cls.cell} width="50%" align="center">
                                        <Typography variant="h6">Area Name</Typography>
                                    </TableCell>
                                    <TableCell className={cls.cell} width="50%" align="center">
                                        <Typography variant="h6">Reason (TODO)</Typography>
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {preCheckErrors.map((error: PreCheckError, index: number) => {
                                    return (
                                        <TableRow>
                                            <TableCell key={error.areaName} className={cls.cell} width="50%" align="center">
                                                <Typography>{error.areaName}</Typography>
                                            </TableCell>
                                            <TableCell key={index} className={cls.cell} width="50%" align="center">
                                                <Typography variant="body1">{error.reasons.join(", ")}</Typography>
                                            </TableCell>
                                        </TableRow>
                                    );
                                })}
                            </TableBody>
                        </Table>
                    </Grid>
                </Grid>
            )}
        </>
    );
};
