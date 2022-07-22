import React, { useEffect, useState } from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles, LinearProgress } from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import classNames from "classnames";
import { Align } from "../../../../common/positioning/Align";
import { useHistory, useLocation } from "react-router-dom";
import { SpaceEvenly } from "../../../../common/positioning/SpaceEvenly";
import { ErrorPanel } from "../Errors/ErrorPanel";
import { DASHBOARD_HEADER_TOPBAR_zINDEX, DRAWER_WIDTH } from "@constants";
import { yellow } from "@material-ui/core/colors";
import { UserDetailsMenu } from "./UserDetails";
import { NotificationBadges } from "./NotificationBadges";
import { InfoIconButton } from "./InfoIconButton";

interface DashboardHeaderProps {
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
    unseenNotifications: number;
    isLoading: boolean;
    dashboardIntentsLoading: boolean;
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
    "/dashboard/set-intents",
    "/dashboard/enquiries",
    "/dashboard/demo",
    "/dashboard/subscribe",
    "/dashboard/subscribe/purchase",
    "/dashboard/confirm",
    "/dashboard/getwidget",
    "/dashboard/activity",
    "/dashboard/file-assets",
];

export const routesToExclude = baseRoutesToExclude.concat(baseRoutesToExclude.map(x => x + "/"));

export const DashboardHeader = ({ isLoading, dashboardIntentsLoading, unseenNotifications, open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {
    const cls = useStyles();
    const [sized, setSized] = useState<boolean>(false);
    const handle = () => setSized(!sized);

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
                    <MenuIcon className={"menu-collapse-tour"} />
                </IconButton>
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
                        <NotificationBadges unseenNotifications={unseenNotifications} />
                    </Align>
                    <Align float="right" verticalCenter extraClassNames={cls.barItem}>
                        <UserDetailsMenu />
                    </Align>
                    <Align float="right" verticalCenter extraClassNames={cls.barItem}>
                        <InfoIconButton handleHelpDrawerOpen={handleHelpDrawerOpen} helpOpen={helpOpen} />
                    </Align>
                </div>
            </Toolbar>
            {(isLoading || dashboardIntentsLoading) && <LinearProgress classes={{ bar: cls.bar }} className={cls.loading} />}
            <ErrorPanel />
        </AppBar>
    );
};
