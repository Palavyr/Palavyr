import React, { useEffect, useState } from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles, Badge, Tooltip } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import classNames from "classnames";
import { Align } from "../positioning/Align";
import { useHistory, useLocation } from "react-router-dom";
import NotificationsIcon from "@material-ui/icons/Notifications";
import InfoIcon from "@material-ui/icons/Info";
import { SpaceEvenly } from "../positioning/SpaceEvenly";
import { ErrorPanel } from "../Errors/ErrorPanel";
import { DASHBOARD_HEADER_TOPBAR_zINDEX } from "@constants";

const drawerWidth: number = 240;

interface DashboardHeaderProps {
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
    unseenNotifications: number;
}

const useStyles = makeStyles((theme) => ({
    topbar: {
        background: theme.palette.primary.main,
        position: "fixed",
        zIndex: DASHBOARD_HEADER_TOPBAR_zINDEX,
    },
    appBar: {
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    icon: {
        borderRadius: "10px",
        marginRight: "2rem",
        paddingRIght: "5rem",
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
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
    name: {
        color: theme.palette.success.main,
    },
    helpIconText: {
        // paddingRight: theme.spacing(3),
    },
}));

const baseRoutesToExclude = [
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
    "/dashboard/enquiries",
    "/dashboard/demo",
    "/dashboard/subscribe",
    "/dashboard/subscribe/purchase",
    "/dashboard/confirm",
    "/dashboard/getwidget",
    "/dashboard/images",
];

const routesToExclude = baseRoutesToExclude.concat(baseRoutesToExclude.map((x) => x + "/"));

export const DashboardHeader = ({ unseenNotifications, open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {
    const cls = useStyles();
    const [sized, setSized] = useState<boolean>(false);
    const handle = () => setSized(!sized);
    const location = useLocation();
    const history = useHistory();

    useEffect(() => {
        window.addEventListener("resize", handle);
        return () => window.removeEventListener("resize", handle);
    }, [sized]);

    return (
        <AppBar position="absolute" className={classNames(cls.topbar, cls.appBar, { [cls.appBarShift]: open })}>
            <>
                <Toolbar>
                    <Align float="left">
                        <IconButton color="inherit" aria-label="open drawer" onClick={handleDrawerOpen} edge="start" className={classNames(open && cls.hide)}>
                            <MenuIcon />
                        </IconButton>
                        <Typography className={cls.name} variant="h4">
                            Palavyr.com
                        </Typography>
                    </Align>
                    <div style={{ flexGrow: 1 }} />
                    <Align>
                        {title && (
                            <SpaceEvenly vertical>
                                <Typography display="inline" align="center" variant="h6">
                                    Current Area:
                                </Typography>
                                <Typography display="inline" align="center" variant="h5">
                                    {title}
                                </Typography>
                            </SpaceEvenly>
                        )}
                    </Align>
                    <div style={{ flexGrow: 1 }} />
                    <div style={{ display: "flex" }}>
                        <Align float="right">
                            {!routesToExclude.includes(location.pathname) ? (
                                <Tooltip title="Help about this page">
                                    <IconButton color="inherit" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(cls.icon, helpOpen && cls.hide)}>
                                        <InfoIcon />
                                    </IconButton>
                                </Tooltip>
                            ) : (
                                <div></div>
                            )}
                        </Align>
                        <Align float="right">
                            <Tooltip title="Unseen enquiries">
                                <span>
                                    <IconButton disabled={unseenNotifications === 0} onClick={() => history.push("/dashboard/enquiries")} className={cls.icon} edge="start" color="inherit">
                                        <Badge showZero={false} badgeContent={unseenNotifications} color="secondary">
                                            <NotificationsIcon />
                                        </Badge>
                                    </IconButton>
                                </span>
                            </Tooltip>
                        </Align>
                    </div>
                </Toolbar>
                <ErrorPanel />
            </>
        </AppBar>
    );
};
