import React, { useEffect, useState } from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import classNames from "classnames";
import HelpIcon from "@material-ui/icons/Help";
import { Align } from "../positioning/Align";
import { useLocation } from "react-router-dom";
import { log } from "console";

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
        zIndex: 999,
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
        borderRadius: "10px",
        marginRight: "2rem",
        paddingRIght: "5rem",
        "&:hover": {
            backgroundColor: theme.palette.primary.light
        }
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
        color: theme.palette.secondary.light,
    },
    helpIconText: {
        paddingRight: theme.spacing(3)
    }
}));

const routesToExclude = [
    "/dashboard",
    "/dashboard/welcome",
    "/dashboard/settings/password",
    "/dashboard/settings/email",
    "/dashboard/settings/companyName",
    "/dashboard/settings/phoneNumber",
    "/dashboard/settings/companyLogo",
    "/dashboard/settings/locale",
    "/dashboard/settings/default_email_template",
    "/dashboard/settings/deleteaccount",
    "/dashboard/set-areas",
    "/dashboard/enquiries/",
    "/dashboard/demo/",
    "/dashboard/subscribe/",
    "/dashboard/subscribe/purchase",
    "/dashboard/confirm",
];

export const DashboardHeader = ({ open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {
    const cls = useStyles();
    const [sized, setSized] = useState<boolean>(false);
    const handle = () => setSized(!sized);
    const location = useLocation();
    console.log(location.pathname);
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
                {!routesToExclude.includes(location.pathname) && (
                    <Align float="right">
                        <IconButton color="inherit" aria-label="open help drawer" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(cls.helpIcon, cls.helpMenuButton, helpOpen && cls.hide)}>
                            <Typography className={cls.helpIconText} variant="h5">Help</Typography>
                            <HelpIcon />
                        </IconButton>
                    </Align>
                )}
            </Toolbar>
        </AppBar>
    );
};
