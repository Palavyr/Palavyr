import { makeStyles, ListItem, ListItemIcon, ListItemText, Tooltip } from "@material-ui/core";
import React, { memo } from "react";
import { NavLink } from "react-router-dom";
import ChatIcon from "@material-ui/icons/Chat";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export const createNavLink = (intentId: string) => {
    return `/dashboard/editor/pricing/${intentId}?tab=${0}`;
};

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
    areaListItem: {
        backgroundColor: theme.palette.secondary.dark,
    },
    areaNameText: {
        color: theme.palette.common.white,
        textDecoration: "none",
    },
    sidebarText: {

    },
}));

export interface AreaLinkItemProps {
    intentId: string;
    isActive: boolean;
    disabled: boolean;
    currentPage: string;
    areaName: string;
    menuOpen: boolean;
}
export const AreaLinkItem = memo(({ intentId, isActive, disabled, currentPage, areaName, menuOpen }: AreaLinkItemProps) => {
    const cls = useStyles();

    return (
        <NavLink key={intentId} to={!isActive || disabled ? currentPage : createNavLink(intentId)} className={cls.areaNameText}>
            <ListItem className={cls.areaListItem} disabled={!isActive || disabled} button key={intentId}>
                {menuOpen ? (
                    <ListItemIcon className={cls.icon}>
                        <ChatIcon />
                    </ListItemIcon>
                ) : (
                    <Tooltip title={areaName} placement="right">
                        <ListItemIcon className={cls.icon}>
                            <ChatIcon />
                        </ListItemIcon>
                    </Tooltip>
                )}
                <ListItemText primary={areaName} primaryTypographyProps={{ component: PalavyrText, className: cls.sidebarText, noWrap: true }} />
            </ListItem>
        </NavLink>
    );
});
