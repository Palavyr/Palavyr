import React, { useContext } from "react";
import { ListItem, ListItemIcon, ListItemText } from "@material-ui/core";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

export interface SidebarLinkItemProps {
    text: string;
    isActive: boolean;
    onClick(): void;
    IconComponent: JSX.Element;
    children?: React.ReactNode;
}

export const SidebarLinkItem = ({ text, isActive, onClick, IconComponent, children }: SidebarLinkItemProps) => {
    return (
        <ListItem disabled={!isActive} button key={text} onClick={onClick}>
            <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>
            <ListItemText primary={text} primaryTypographyProps={{ variant: "body2" }} />
            {children && children}
        </ListItem>
    );
};
