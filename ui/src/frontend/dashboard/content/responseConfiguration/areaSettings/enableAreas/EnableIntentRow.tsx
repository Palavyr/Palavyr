import { makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { IntentsEnabled } from "@Palavyr-Types";
import classNames from "classnames";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { OsTypeToggle } from "./OsTypeToggle";

type styleProps = {
    isEnabled?: boolean | null;
};

const useStyles = makeStyles(theme => ({
    center: {
        textAlign: "center",
    },
    cell: {
        textAlign: "left",
        flexDirection: "column",
        justifyContent: "center",
    },
    row: (props: styleProps) => ({
        backgroundColor: props.isEnabled ? theme.palette.success.light : theme.palette.grey[300],
        borderTop: `10px solid ${theme.palette.background.default}`,
    }),
    wide: {
        minWidth: "150px",
    },
}));

export interface EnableAreaRowProps {
    areasEnabled: IntentsEnabled;
    rowNumber: number;
}

export const EnableIntentRow = ({ areasEnabled, rowNumber }: EnableAreaRowProps) => {
    const { repository } = useContext(DashboardContext);
    const [isEnabled, setIsEnabled] = useState<boolean | null>(null);

    const cls = useStyles({ isEnabled });

    const onToggleChange = async () => {
        const updatedIsEnabled = await repository.Intent.ToggleIsEnabled(!isEnabled, areasEnabled.areaId);
        setIsEnabled(updatedIsEnabled);
    };

    useEffect(() => {
        setIsEnabled(areasEnabled.isEnabled);
    }, []);

    return (
        <TableRow className={classNames(cls.row)}>
            <TableCell className={cls.cell}>
                <Typography variant="body2">{rowNumber}</Typography>
            </TableCell>
            <TableCell className={cls.cell}>
                <Typography variant="h6">{areasEnabled.areaName}</Typography>
            </TableCell>
            <TableCell className={classNames(cls.cell, cls.wide)}>
                <OsTypeToggle controlledState={isEnabled === true} onChange={onToggleChange} enabledLabel="Enabled" disabledLabel="Disabled" />
            </TableCell>
        </TableRow>
    );
};
