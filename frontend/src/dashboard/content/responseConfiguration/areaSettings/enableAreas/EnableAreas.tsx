import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { AreasEnabled, AreaTable } from "@Palavyr-Types";
import classNames from "classnames";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { useCallback } from "react";
import { EnableAreaRow } from "./EnableAreaRow";

const useStyles = makeStyles(theme => ({
    paper: {
        backgroundColor: "rgb(0, 0, 0 ,0)",
        border: "0px",
        boxShadow: "none",
        color: "black",
        padding: "2rem",
        margin: "2rem",
        width: "40%",
    },
    container: {
        display: "flex",
        justifyContent: "center",
        borderRadius: "15px",
    },
    center: {
        textAlign: "center",
    },
    header: {
        backgroundColor: theme.palette.primary.light,
        color: theme.palette.common.white,
    },

}));

export const EnableAreas = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Enable / Disable Areas");

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
            <AreaConfigurationHeader
                title="Enable or disable your Areas"
                subtitle="Use these toggles to enable or disable your configured areas. If an area is disabled, it will not appear in your chat widget."
            />
            <TableContainer className={cls.container}>
                <Table component={Paper} className={cls.paper}>
                    <TableHead>
                        <TableRow className={classNames(cls.header)}>
                            <TableCell></TableCell>
                            <TableCell className={classNames(cls.center)}>
                                <PalavyrText className={cls.header} variant="h4">
                                    Area Name
                                </PalavyrText>
                            </TableCell>
                            <TableCell className={classNames(cls.center)}>
                                <PalavyrText className={cls.header} variant="h4">
                                    Status
                                </PalavyrText>
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
