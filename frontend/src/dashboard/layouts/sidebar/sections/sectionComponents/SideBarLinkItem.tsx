import React from "react";
import { makeStyles, ListItem, ListItemIcon, ListItemText } from "@material-ui/core";

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
