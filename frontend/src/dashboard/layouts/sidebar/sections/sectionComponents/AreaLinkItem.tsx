import { makeStyles, ListItem, ListItemIcon, ListItemText } from "@material-ui/core";
import React from "react";
import { NavLink } from "react-router-dom";
import ChatIcon from "@material-ui/icons/Chat";

const createNavLink = (areaIdentifier: string) => {
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
    index: number;
    numAreasAllowed: number;
    currentPage: string;
    areaName: string;
}
export const AreaLinkItem = ({ areaIdentifier, isActive, index, numAreasAllowed, currentPage, areaName }: AreaLinkItemProps) => {
    const cls = useStyles();

    return (
        <NavLink key={areaIdentifier} to={!isActive || index > numAreasAllowed ? currentPage : createNavLink(areaIdentifier)} className={cls.areaNameText}>
            <ListItem disabled={!isActive || index > numAreasAllowed} button key={areaIdentifier}>
                <ListItemIcon className={cls.icon}>
                    <ChatIcon />
                </ListItemIcon>
                <ListItemText primary={areaName} primaryTypographyProps={{ className: cls.sidebarText }} />
            </ListItem>
        </NavLink>
    );
};
