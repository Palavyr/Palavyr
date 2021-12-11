import React, { useEffect } from "react";
import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import Backdrop from "@material-ui/core/Backdrop";
import SpeedDial from "@material-ui/lab/SpeedDial";
import SpeedDialIcon from "@material-ui/lab/SpeedDialIcon";
import SpeedDialAction from "@material-ui/lab/SpeedDialAction";
import { Action } from "@Palavyr-Types";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            height: 380,
            transform: "translateZ(0px)",
            flexGrow: 1,
        },
        speedDial: {
            position: "absolute",
            bottom: theme.spacing(2),
            right: theme.spacing(2),
        },
    })
);

export interface PalavyrSpeedDialProps {
    actions: Action[];
    startState?: boolean;
}

export const PalavyrSpeedDial = ({ actions, startState = false }: PalavyrSpeedDialProps) => {
    const cls = useStyles();
    const [open, setOpen] = React.useState(false);
    const [hidden, setHidden] = React.useState(false);

    useEffect(() => {
        setOpen(startState);
    }, []);

    const handleVisibility = () => {
        setHidden(prevHidden => !prevHidden);
    };

    const onOpen = () => {
        setOpen(!open);
    };

    const onClose = () => {
        setOpen(false);
    };

    return (
        <div className={cls.root}>
            <SpeedDial direction="left" ariaLabel="palavyr-speed-dial" className={cls.speedDial} hidden={hidden} icon={<SpeedDialIcon />} onClick={onClose} onOpen={onOpen} open={open}>
                {actions.map(action => (
                    <SpeedDialAction key={action.name} icon={action.icon} tooltipTitle={action.name} tooltipPlacement="top" onClick={async () => await action.onClick()} />
                ))}
            </SpeedDial>
        </div>
    );
};
