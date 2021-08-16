import { makeStyles, ListItem, ListItemIcon, ListItemText } from "@material-ui/core";
import React, { memo } from "react";
import { NavLink } from "react-router-dom";
import ChatIcon from "@material-ui/icons/Chat";

export const createNavLink = (areaIdentifier: string) => {
    return `/dashboard/editor/email/${areaIdentifier}?tab=${0}`;
};

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
    areaNameText: {
        color: theme.palette.common.white,
        textDecoration: "none",
    },
    sidebarText: {
        fontWeight: "normal",
        fontSize: "14px",
    },
}));

export interface AreaLinkItemProps {
    areaIdentifier: string;
    isActive: boolean;
    disabled: boolean;
    currentPage: string;
    areaName: string;
}
export const AreaLinkItem = memo(({ areaIdentifier, isActive, disabled, currentPage, areaName }: AreaLinkItemProps) => {
    const cls = useStyles();

    return (
        <NavLink key={areaIdentifier} to={!isActive || disabled ? currentPage : createNavLink(areaIdentifier)} className={cls.areaNameText}>
            <ListItem disabled={!isActive || disabled} button key={areaIdentifier}>
                <ListItemIcon className={cls.icon}>
                    <ChatIcon />
                </ListItemIcon>
                <ListItemText primary={areaName} primaryTypographyProps={{ className: cls.sidebarText, noWrap: true }} />
            </ListItem>
        </NavLink>
    );
});
