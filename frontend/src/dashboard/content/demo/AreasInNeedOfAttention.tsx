import React, { useState } from "react";
import { Fade, Grid, makeStyles, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@material-ui/core";
import { PreCheckError } from "@Palavyr-Types";
import DoneOutlineIcon from "@material-ui/icons/DoneOutline";
import { useEffect } from "react";

const useStyles = makeStyles(theme => ({
    uppercell: {
        paddingTop: "1rem",
        paddingBottom: "2rem",
        // borderTop: "4px solid black",
        // borderBottom: "4px solid black",
        backgroundColor: theme.palette.warning.light,
        marginBottom: "3rem",
        boxShadow: theme.shadows[2],
        // borderRadius: "15px"
    },
    // sucessUpperCell: {
    //     // paddingTop: "1rem",
    //     // paddingBottom: "2rem",
    //     // borderTop: "4px solid black",
    //     // borderBottom: "4px solid black",
    //     backgroundColor: theme.palette.success.light,
    //     marginBottom: "3rem",
    //     boxShadow: theme.shadows[4],
    //     justifyItems: "center",
    //     // borderRadius: "15px"
    // },
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

interface AreasInNeedOfAttentionProps {
    preCheckErrors: PreCheckError[];
}

export const AreasInNeedOfAttention = ({ preCheckErrors }: AreasInNeedOfAttentionProps) => {
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
            {/* {preCheckErrors.length === 0 && !hidden && (
                <Fade in={isVisible} exit={!isVisible} style={{ textAlign: "center", display: "flex", justifyContent: "center" }}>
                    <Grid
                        className={cls.sucessUpperCell}
                        style={{ display: "flex", justifyContent: "center", justifyItems: "center", width: "30%", paddingLeft: "2rem", paddingRight: "2rem", paddingTop: "1rem", paddingBottom: "1rem" }}
                    >
                        <>
                            <div style={{ marginRight: "1.3rem" }}>
                                <Typography align="center" variant="h4">
                                    Ready
                                </Typography>
                            </div>
                            <div style={{ top: "30px" }}>
                                <DoneOutlineIcon style={{ margin: "0px", padding: "0px", top: "4px", position: "relative" }} />
                            </div>
                        </>
                    </Grid>
                </Fade>
            )} */}
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
                                    {preCheckErrors.map((error: PreCheckError, index: number) => {
                                        return (
                                            <TableRow>
                                                <TableCell key={error.areaName} className={cls.cell} width="50%" align="center">
                                                    <Typography>{error.areaName}</Typography>
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
