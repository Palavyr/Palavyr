import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { AreasEnabled } from "@Palavyr-Types";
import classNames from "classnames";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import React, { useEffect, useState } from "react";
import { OsTypeToggle } from "./OsTypeToggle";

type styleProps = {
    isEnabled: boolean;
};

const useStyles = makeStyles((theme) => ({
    center: {
        textAlign: "center",
    },
    cell: {
        textAlign: "left",
        // display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    row: (props: styleProps) => ({
        backgroundColor: props.isEnabled ? theme.palette.success.light : theme.palette.error.light,
    }),
}));

export interface EnableAreaRowProps {
    areasEnabled: AreasEnabled;
    rowNumber: number;
}

export const EnableAreaRow = ({ areasEnabled, rowNumber }: EnableAreaRowProps) => {
    const repository = new PalavyrRepository();
    const [isEnabled, setIsEnabled] = useState<boolean | null>(null);

    const cls = useStyles({ isEnabled });

    const onToggleChange = async () => {
        const updatedIsEnabled = await repository.Area.UpdateIsEnabled(!isEnabled, areasEnabled.areaId);
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
            <TableCell className={cls.cell}>
                <OsTypeToggle controlledState={isEnabled === true} onChange={onToggleChange} enabledLabel="Area Enabled" disabledLabel="Area Disabled" />
            </TableCell>
        </TableRow>
    );
};
