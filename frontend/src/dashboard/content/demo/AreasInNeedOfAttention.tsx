import { Grid, makeStyles, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@material-ui/core";
import { IncompleteAreas, IncompleteArea } from "@Palavyr-Types";
import React from "react";

const useStyles = makeStyles((theme) => ({
    formroot: {
        display: "flex",
        flexWrap: "wrap",
        width: "100%",
        paddingLeft: "1.4rem",
        paddingRight: "2.3rem",
        justifyContent: "center",
    },
    paper: {
        alignItems: "center",
        backgroundColor: "#535c68",
        border: "0px solid black",
        boxShadow: "0 0 black",
    },
    grid: {
        border: "0px solid black",
        display: "flex",
        justifyContent: "center",
    },
    container: {
        height: "100%",
    },
    widgetcell: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        borderRight: "2px solid black",
        textAlign: "center",
        backgroundColor: "#535c68",
    },
    uppercell: {
        paddingTop: "1rem",
        paddingBottom: "2rem",
        borderTop: "4px solid black",
        borderBottom: "4px solid black",
    },
    lowercell: {
        borderBottom: "4px solid black",
    },
    table: {
        border: "0px solid black",
    },
    actions: {
        width: "100%",
        display: "flex",
        padding: "8px",
        alignItems: "center",
        justifyContent: "flex-start",
    },
    editorContainer: {
        width: "100%",
    },
    customizetext: {
        paddingTop: "1.8rem",
        paddingBottom: "1.8rem",
    },
    tablegrid: {
        paddingRight: "20%",
        paddingLeft: "20%",
    },
    cell: {
        borderBottom: "1px solid lightgray",
    },
    centerText: {
        textAlign: "center",
        justifyContent: "flex-end",
        alignSelf: "center",
        alignItems: "center",
    },
    div: {
        display: "flex",
        flexDirection: "row",
    },
    colorpicker: {
        paddingBottom: "1rem",
        marginTop: "-2rem",
        marginLeft: "1.2rem",
        borderLeft: "1px solid black",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
    },
}));

interface IAreasInNeedOfAttention {
    incompleteAreas: IncompleteAreas;
}

export const AreasInNeedOfAttention = ({ incompleteAreas }: IAreasInNeedOfAttention) => {
    const cls = useStyles();

    return (
        <>
            {incompleteAreas.length > 0 && (
                <Grid className={cls.uppercell}>
                    <Grid className={cls.tablegrid}>
                        <Typography style={{ paddingBottom: "1rem" }} align="center" variant="h6">
                            Areas in need of attention:
                        </Typography>
                        <Table className={cls.table}>
                            <TableHead>
                                <TableRow>
                                    <TableCell className={cls.cell} width="50%" align="center">
                                        <Typography variant="h6">Area Name</Typography>
                                    </TableCell>
                                    <TableCell className={cls.cell} width="50%" align="center">
                                        <Typography variant="h6">Area Title</Typography>
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {incompleteAreas.map((area: IncompleteArea, index: number) => {
                                    return (
                                        <TableRow>
                                            <TableCell key={area.areaName} className={cls.cell} width="50%" align="center">
                                                <Typography>{area.areaName}</Typography>
                                            </TableCell>
                                            <TableCell key={index} className={cls.cell} width="50%" align="center">
                                                <Typography>{area.areaDisplayTitle}</Typography>
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
