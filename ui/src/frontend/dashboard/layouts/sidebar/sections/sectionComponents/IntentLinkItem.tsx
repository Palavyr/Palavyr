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
    intentListItem: {
        backgroundColor: theme.palette.secondary.dark,
    },
    intentNameText: {
        color: theme.palette.common.white,
        textDecoration: "none",
    },
    sidebarText: {

    },
}));

export interface IntentLinkItemProps {
    intentId: string;
    isActive: boolean;
    disabled: boolean;
    currentPage: string;
    intentName: string;
    menuOpen: boolean;
}
export const IntentLinkItem = memo(({ intentId, isActive, disabled, currentPage, intentName, menuOpen }: IntentLinkItemProps) => {
    const cls = useStyles();

    return (
        <NavLink key={intentId} to={!isActive || disabled ? currentPage : createNavLink(intentId)} className={cls.intentNameText}>
            <ListItem className={cls.intentListItem} disabled={!isActive || disabled} button key={intentId}>
                {menuOpen ? (
                    <ListItemIcon className={cls.icon}>
                        <ChatIcon />
                    </ListItemIcon>
                ) : (
                    <Tooltip title={intentName} placement="right">
                        <ListItemIcon className={cls.icon}>
                            <ChatIcon />
                        </ListItemIcon>
                    </Tooltip>
                )}
                <ListItemText primary={intentName} primaryTypographyProps={{ component: PalavyrText, className: cls.sidebarText, noWrap: true }} />
            </ListItem>
        </NavLink>
    );
});
