import React, { useEffect } from "react";
import { makeStyles, createStyles, Theme, useTheme } from "@material-ui/core/styles";
import SpeedDial from "@material-ui/lab/SpeedDial";
import SpeedDialIcon from "@material-ui/lab/SpeedDialIcon";
import SpeedDialAction from "@material-ui/lab/SpeedDialAction";
import { Action } from "@Palavyr-Types";

const useStyles = makeStyles<{}>((theme: Theme) =>
    createStyles({
        root: {
            height: 380,
            transform: "translateZ(0px)",

            flexGrow: 1,
        },
        speedDial: {
            position: "absolute",
            bottom: theme.spacing(2),
            right: "0rem",
        },
        fabprops: {
            "&:hover": {},
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

    useEffect(() => {
        setOpen(startState);
    }, []);

    const onOpen = () => {
        setOpen(!open);
    };

    const onClose = e => {
        e.preventDefault();

        setOpen(false);
    };
    const [popperOpen, setPopperOpen] = React.useState(false);
    const theme = useTheme();
    return (
        <div className={cls.root}>
            <SpeedDial
                FabProps={{ style: { backgroundColor: theme.palette.secondary.main } }}
                direction="left"
                hidden={true}
                ariaLabel="palavyspeed-dial"
                className={cls.speedDial}
                icon={<SpeedDialIcon />}
                onClick={onClose}
                onOpen={onOpen}
                open={true}
            >
                {actions.map(action => (
                    <SpeedDialAction
                        FabProps={{ style: { marginLeft: "1.6rem" } }}
                        PopperProps={{
                            open: popperOpen,
                        }}
                        onMouseEnter={() => {
                            setPopperOpen(true);
                            setTimeout(() => {
                                setPopperOpen(false);
                            }, 2000);
                        }}
                        onMouseLeave={() => {
                            setPopperOpen(false);
                        }}
                        onClose={() => null}
                        key={action.name}
                        icon={action.icon}
                        tooltipTitle={action.name}
                        tooltipPlacement="top"
                        onClick={async () => await action.onClick()}
                    />
                ))}
            </SpeedDial>
        </div>
    );
};
