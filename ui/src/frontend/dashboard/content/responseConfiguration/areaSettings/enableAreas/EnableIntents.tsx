import { HeaderStrip } from "@common/components/HeaderStrip";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { IntentsEnabled, IntentResource } from "@Palavyr-Types";
import classNames from "classnames";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { useCallback } from "react";
import { EnableIntentRow } from "./EnableIntentRow";

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

export const EnableIntents = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Enable / Disable Intents");

    const cls = useStyles();
    const [intentIds, setIntentIds] = useState<IntentsEnabled[]>([]);

    const loadIntents = useCallback(async () => {
        const intentData = await repository.Intent.GetAllIntents();
        const intentIds = intentData.map((x: IntentResource) => {
            return {
                areaId: x.intentId,
                isEnabled: x.isEnabled,
                areaName: x.areaName,
            };
        });
        setIntentIds(intentIds);
    }, []);

    useEffect(() => {
        loadIntents();
    }, []);

    return (
        <>
            <HeaderStrip
                title="Enable or disable your Intents"
                subtitle="Use these toggles to enable or disable your configured intents. If an intent is disabled, it will not appear in your chat widget."
            />
            <TableContainer className={cls.container}>
                <Table component={Paper} className={cls.paper}>
                    <TableHead>
                        <TableRow className={classNames(cls.header)}>
                            <TableCell></TableCell>
                            <TableCell className={classNames(cls.center)}>
                                <PalavyrText className={cls.header} variant="h4">
                                    Intent Name
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
                        {sortByPropertyAlphabetical((x: IntentsEnabled) => x.areaName, intentIds).map((x: IntentsEnabled, index: number) => {
                            return <EnableIntentRow key={index} rowNumber={index + 1} areasEnabled={x} />;
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
};
