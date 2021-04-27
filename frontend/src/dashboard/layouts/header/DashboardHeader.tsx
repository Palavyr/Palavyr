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
        background: theme.palette.primary.main,
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
                <div>
                    <Align float="left">
                        <IconButton color="inherit" aria-label="open drawer" onClick={handleDrawerOpen} edge="start" className={classNames(cls.menuButton, open && cls.hide)}>
                            <MenuIcon />
                        </IconButton>
                    </Align>
                    <Align float="right">
                        <Typography variant="h4">Current Area: {title}</Typography>
                    </Align>
                </div>
                <div className={cls.helpIcon}>
                    <IconButton color="inherit" aria-label="open help drawer" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(cls.helpMenuButton, helpOpen && cls.hide)}>
                        <HelpIcon />
                    </IconButton>
                </div>
            </Toolbar>
        </AppBar>
    );
};
