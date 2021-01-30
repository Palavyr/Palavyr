import { ApiClient } from "@api-client/Client";
import { makeStyles, TableCell, TableRow } from "@material-ui/core";
import { AreasEnabled } from "@Palavyr-Types";
import React, { useEffect, useState } from "react";
import { AreaToggle } from "./AreaToggle";

const useStyles = makeStyles((theme) => ({
    center: {
        textAlign: "center",
    },
}));

interface IToggleWrapper {
    areasEnabled: AreasEnabled;
}

export const EnableAreaWrapper = ({ areasEnabled }: IToggleWrapper) => {
    const client = new ApiClient();
    const cls = useStyles();

    const [isCompleteState, setIsCompleteState] = useState<boolean | null>(null);

    const onToggleChange = async () => {
        const { data: updatedIsEnabled } = await client.Area.UpdateIsComplete(!isCompleteState, areasEnabled.areaId);
        setIsCompleteState(updatedIsEnabled);
    };

    useEffect(() => {
        setIsCompleteState(areasEnabled.isEnabled);
    }, []);

    return (
        <TableRow className={cls.center}>
            <TableCell className={cls.center}>{areasEnabled.areaName}</TableCell>
            <TableCell className={cls.center}>
                <AreaToggle isComplete={isCompleteState === true} onChange={onToggleChange} />
            </TableCell>
        </TableRow>
    );
};
