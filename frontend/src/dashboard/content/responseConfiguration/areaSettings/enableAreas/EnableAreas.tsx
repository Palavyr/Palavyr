import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@material-ui/core";
import { AreasEnabled, AreaTable } from "@Palavyr-Types";
import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { useCallback } from "react";
import theme from "theme";
import { EnableAreaRow } from "./EnableAreaRow";

const useStyles = makeStyles(() => ({
    paper: {
        backgroundColor: theme.palette.secondary.light,
        padding: "2rem",
        margin: "2rem",
        width: "40%",
    },
    container: {
        display: "flex",
        justifyContent: "center",
    },
    center: {
        textAlign: "center",
    },
}));

export const EnableAreas = () => {
    const repository = new PalavyrRepository();
    const cls = useStyles();
    const [areaIds, setAreaIds] = useState<AreasEnabled[]>([]);

    const loadAreas = useCallback(async () => {
        const areaData = await repository.Area.GetAreas();
        const areaIdentifiers = areaData.map((x: AreaTable) => {
            return {
                areaId: x.areaIdentifier,
                isEnabled: x.isEnabled,
                areaName: x.areaName,
            };
        });
        setAreaIds(areaIdentifiers);
    }, []);

    useEffect(() => {
        loadAreas();
    }, []);

    return (
        <>
            <AreaConfigurationHeader title="Enable or disable your Areas" subtitle="Use these toggles to enable or disable your configured areas. If an area is disabled, it will not appear in your chat widget." />
            <TableContainer className={cls.container}>
                <Table component={Paper} className={cls.paper}>
                    <TableHead>
                        <TableRow>
                            <TableCell></TableCell>
                            <TableCell className={classNames(cls.center)}>
                                <Typography variant="h4">Area Name</Typography>
                            </TableCell>
                            <TableCell className={classNames(cls.center)}>
                                <Typography variant="h4">Status</Typography>
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {sortByPropertyAlphabetical((x: AreasEnabled) => x.areaName, areaIds).map((x: AreasEnabled, index: number) => {
                            return <EnableAreaRow key={index} rowNumber={index + 1} areasEnabled={x} />;
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
};
