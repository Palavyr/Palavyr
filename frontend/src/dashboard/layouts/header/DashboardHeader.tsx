import React, { useEffect, useState } from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles, Badge, Tooltip, LinearProgress } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import classNames from "classnames";
import { Align } from "../positioning/Align";
import { useHistory, useLocation } from "react-router-dom";
import NotificationsIcon from "@material-ui/icons/Notifications";
import InfoIcon from "@material-ui/icons/Info";
import { SpaceEvenly } from "../positioning/SpaceEvenly";
import { ErrorPanel } from "../Errors/ErrorPanel";
import { DASHBOARD_HEADER_TOPBAR_zINDEX, DRAWER_WIDTH } from "@constants";
import { yellow } from "@material-ui/core/colors";
import { UserDetails } from "./UserDetails";

interface DashboardHeaderProps {
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
    unseenNotifications: number;
    isLoading: boolean;
    dashboardAreasLoading: boolean;
}

const useStyles = makeStyles(theme => ({
    icon: {
        borderRadius: "10px",
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
    },
    helpIconText: {
        // paddingRight: theme.spacing(3),
    },
    loading: {
        backgroundColor: theme.palette.primary.dark,
        height: "8px",
    },
    bar: {
        backgroundColor: yellow[300],
    },
    barItem: {
        marginRight: "0.8rem",
        marginLeft: "0.8rem",
    },

    hide: {
        display: "none",
    },
    appBar: {
        boxShadow: "none",
        zIndex: theme.zIndex.drawer + 1,
        transition: theme.transitions.create(["width", "margin"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    appBarShift: {
        marginLeft: DRAWER_WIDTH,
        width: `calc(100% - ${DRAWER_WIDTH}px)`,
        transition: theme.transitions.create(["width", "margin"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    menuButton: {
        marginRight: 36,
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
    "/dashboard/activity",
];

const routesToExclude = baseRoutesToExclude.concat(baseRoutesToExclude.map(x => x + "/"));

export const DashboardHeader = ({ isLoading, dashboardAreasLoading, unseenNotifications, open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {
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
        <AppBar
            position="fixed"
            className={classNames(cls.appBar, {
                [cls.appBarShift]: open,
            })}
        >
            <Toolbar>
                <IconButton
                    color="inherit"
                    aria-label="open drawer"
                    onClick={handleDrawerOpen}
                    edge="start"
                    className={classNames(cls.menuButton, {
                        [cls.hide]: open,
                    })}
                >
                    <MenuIcon />
                </IconButton>
                {/* <div style={{ flexGrow: 1 }} /> */}
                <Align direction="flex-start">
                    {title && (
                        <>
                            <Typography align="left" variant="h5">
                                {title}
                            </Typography>
                        </>
                    )}
                </Align>
                <div style={{ flexGrow: 1 }} />
                <div style={{ display: "flex", justifyContent: "space-between" }}>
                    <Align float="right" verticalCenter extraClassNames={cls.barItem}>
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
                    <Align float="right" verticalCenter extraClassNames={cls.barItem}>
                        <Tooltip title="Unseen enquiries">
                            <span className={classNames(cls.icon, "check-enquiries-badge-sidebar-tour")}>
                                <IconButton disabled={unseenNotifications === 0} onClick={() => history.push("/dashboard/enquiries")} className={cls.icon} edge="start" color="inherit">
                                    <Badge showZero={false} badgeContent={unseenNotifications} color="secondary">
                                        <NotificationsIcon />
                                    </Badge>
                                </IconButton>
                            </span>
                        </Tooltip>
                    </Align>
                    <Align float="right" verticalCenter extraClassNames={cls.barItem}>
                        <UserDetails />
                    </Align>
                </div>
            </Toolbar>
            {(isLoading || dashboardAreasLoading) && <LinearProgress classes={{ bar: cls.bar }} className={cls.loading} />}
            <ErrorPanel />
        </AppBar>
    );
};
