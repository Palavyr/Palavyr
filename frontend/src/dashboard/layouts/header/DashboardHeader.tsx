import React, { useEffect, useState } from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import classNames from "classnames";
import HelpIcon from "@material-ui/icons/Help";
import { Align } from "../positioning/AlignCenter";

const drawerWidth: number = 240;

interface DashboardHeaderProps {
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
}

const useStyles = makeStyles((theme) => ({
    topbar: {
        background: theme.palette.primary.dark,
        position: "fixed",
    },
    appBar: {
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    toolbar: {
        width: "100%",
        height: "100%",
        display: "flex",
        justifyContent: "space-between",
    },

    helpIcon: {
        marginRight: "2rem",
        paddingRIght: "5rem",
    },
    appBarShift: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    hide: {
        display: "none",
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    helpMenuButton: {
        marginLeft: theme.spacing(5),
        alignSelf: "right",
        textAlign: "right",
    },
    name: {
        color: theme.palette.secondary.light
    }
}));

export const DashboardHeader = ({ open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {
    const cls = useStyles();
    const [sized, setSized] = useState<boolean>(false);
    const handle = () => setSized(!sized);

    useEffect(() => {
        window.addEventListener("resize", handle);
        return () => window.removeEventListener("resize", handle);
    }, [sized]);

    return (
        <AppBar position="absolute" className={classNames(cls.topbar, cls.appBar, { [cls.appBarShift]: open })}>
            <Toolbar className={cls.toolbar}>
                <Align float="left">
                    <IconButton color="inherit" aria-label="open drawer" onClick={handleDrawerOpen} edge="start" className={classNames(cls.menuButton, open && cls.hide)}>
                        <MenuIcon />
                    </IconButton>
                    <Typography className={cls.name} variant="h4">
                        Palavyr.com
                    </Typography>
                </Align>
                <Align>
                    {title && (
                        <Typography align="center" variant="h4">
                            Current Area: {title}
                        </Typography>
                    )}
                </Align>
                <Align float="right">
                    <IconButton color="inherit" aria-label="open help drawer" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(cls.helpIcon, cls.helpMenuButton, helpOpen && cls.hide)}>
                        <HelpIcon />
                    </IconButton>
                </Align>
            </Toolbar>
        </AppBar>
    );
};
