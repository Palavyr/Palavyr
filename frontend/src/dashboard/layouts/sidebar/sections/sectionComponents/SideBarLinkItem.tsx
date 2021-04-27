import React from "react";
import { makeStyles, ListItem, ListItemIcon, ListItemText } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
    sidebarText: {
        fontWeight: "normal",
        // fontSize: "14px",
    },
}));
export interface SidebarLinkItemProps {
    text: string;
    isActive: boolean;
    onClick(): void;
    IconComponent: JSX.Element;
}

export const SidebarLinkItem = ({ text, isActive, onClick, IconComponent }: SidebarLinkItemProps) => {
    return (
        <ListItem disabled={!isActive} button key={text} onClick={onClick}>
            <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>
            <ListItemText primary={text} primaryTypographyProps={{ variant: "body2" }} />
        </ListItem>
    );
};
