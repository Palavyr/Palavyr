import { ApiClient } from "@api-client/Client";
import { makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { AreasEnabled } from "@Palavyr-Types";
import React, { useEffect, useState } from "react";
import { OsTypeToggle } from "./OsTypeToggle";

const useStyles = makeStyles((theme) => ({
    center: {
        textAlign: "left",
    },
}));

export interface EnableAreaRowProps {
    areasEnabled: AreasEnabled;
    rowNumber: number;
}

export const EnableAreaRow = ({ areasEnabled, rowNumber }: EnableAreaRowProps) => {
    const client = new ApiClient();
    const cls = useStyles();

    const [isEnabled, setIsEnabled] = useState<boolean | null>(null);

    const onToggleChange = async () => {
        const { data: updatedIsEnabled } = await client.Area.UpdateIsEnabled(!isEnabled, areasEnabled.areaId);
        setIsEnabled(updatedIsEnabled);
    };

    useEffect(() => {
        setIsEnabled(areasEnabled.isEnabled);
    }, []);

    return (
        <TableRow className={cls.center}>
            <TableCell className={cls.center}>
                <Typography variant="body2">{rowNumber}</Typography>
            </TableCell>
            <TableCell className={cls.center}>
                <Typography variant="h6">{areasEnabled.areaName}</Typography>
            </TableCell>
            <TableCell className={cls.center}>
                <OsTypeToggle controlledState={isEnabled === true} onChange={onToggleChange} enabledLabel="Area Enabled" disabledLabel="Area Disabled" />
            </TableCell>
        </TableRow>
    );
};
