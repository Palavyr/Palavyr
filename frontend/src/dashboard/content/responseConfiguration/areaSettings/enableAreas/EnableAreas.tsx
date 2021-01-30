import { ApiClient } from "@api-client/Client";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { AreasEnabled, AreaTable } from "@Palavyr-Types";
import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { useCallback } from "react";
import { EnableAreaWrapper } from "./EnableAreaWrapper";

const useStyles = makeStyles(() => ({
    paper: {
        backgroundColor: "#C7ECEE",
        padding: "2rem",
        margin: "2rem",
        width: "40%",
    },
    titleText: {
        fontWeight: "bold",
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
    const client = new ApiClient();
    const cls = useStyles();
    const [areaIds, setAreaIds] = useState<AreasEnabled[]>([]);

    const loadAreas = useCallback(async () => {
        const { data: areaData } = await client.Area.GetAreas();
        const areaIdentifiers = areaData.map((x: AreaTable) => {
            return {
                areaId: x.areaIdentifier,
                isEnabled: x.isComplete,
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
                            <TableCell className={classNames(cls.center, cls.titleText)}>Area Name</TableCell>
                            <TableCell className={classNames(cls.center, cls.titleText)}>Status</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {areaIds.map((x: AreasEnabled, index: number) => {
                            return <EnableAreaWrapper key={index} areasEnabled={x} />;
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
};
