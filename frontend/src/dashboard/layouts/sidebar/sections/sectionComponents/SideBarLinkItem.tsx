import React from "react";
import { ListItem, ListItemIcon, ListItemText } from "@material-ui/core";

export interface SidebarLinkItemProps {
    text: string;
    isActive: boolean;
    onClick(): void;
    IconComponent: JSX.Element;
    children?: React.ReactNode;
    disabled?: boolean;
}

export const SidebarLinkItem = ({ text, isActive, onClick, IconComponent, children, disabled }: SidebarLinkItemProps) => {
    return (
        <ListItem disabled={!isActive || disabled} button key={text} onClick={onClick}>
            <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>
            <ListItemText primary={text} primaryTypographyProps={{ variant: "body2" }} />
            {children && children}
        </ListItem>
    );
};
