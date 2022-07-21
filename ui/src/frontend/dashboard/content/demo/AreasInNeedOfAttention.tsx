import React, { useState } from "react";
import { Fade, Grid, makeStyles, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@material-ui/core";
import { PreCheckErrorResource } from "@Palavyr-Types";
import { useEffect } from "react";

const useStyles = makeStyles(theme => ({
    uppercell: {
        paddingTop: "1rem",
        paddingBottom: "2rem",
        backgroundColor: theme.palette.warning.light,
        marginBottom: "3rem",
        boxShadow: theme.shadows[2],
    },

    table: {
        border: "0px solid grey",
    },
    tablegrid: {
        paddingRight: "20%",
        paddingLeft: "20%",
    },
    cell: {
        borderBottom: "0px solid gray",
    },
}));

interface IntentsInNeedOfAttentionProps {
    preCheckErrors: PreCheckErrorResource[];
}

export const IntentsInNeedOfAttention = ({ preCheckErrors }: IntentsInNeedOfAttentionProps) => {
    const cls = useStyles();
    const [isVisible, setIsVisible] = useState(true);
    const [hidden, setHidden] = useState(false);

    useEffect(() => {
        setTimeout(() => {
            setIsVisible(false);
            setTimeout(() => {
                setHidden(true);
            }, 800);
        }, 5000);
    }, []);
    return (
        <>
            {preCheckErrors.length > 0 && (
                <Fade in>
                    <Grid className={cls.uppercell}>
                        <Grid className={cls.tablegrid}>
                            <Typography gutterBottom align="center" variant="h4">
                                Errors
                            </Typography>
                            <Table className={cls.table}>
                                <TableHead>
                                    <TableRow>
                                        <TableCell className={cls.cell} width="50%" align="center">
                                            <Typography variant="h6">Error Location</Typography>
                                        </TableCell>
                                        <TableCell className={cls.cell} width="50%" align="center">
                                            <Typography variant="h6">Error</Typography>
                                        </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {preCheckErrors.map((error: PreCheckErrorResource, index: number) => {
                                        return (
                                            <TableRow>
                                                <TableCell key={error.intentName} className={cls.cell} width="50%" align="center">
                                                    <Typography>{error.intentName}</Typography>
                                                </TableCell>
                                                <TableCell key={index} className={cls.cell} width="50%" align="center">
                                                    {error.reasons.map((reason: string) => {
                                                        return <Typography>{reason}</Typography>;
                                                    })}
                                                </TableCell>
                                            </TableRow>
                                        );
                                    })}
                                </TableBody>
                            </Table>
                        </Grid>
                    </Grid>
                </Fade>
            )}
        </>
    );
};
