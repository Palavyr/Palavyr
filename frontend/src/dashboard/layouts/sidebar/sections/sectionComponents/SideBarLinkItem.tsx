import React from "react";
import { ListItem, ListItemIcon, ListItemText, Typography, TypographyClassKey, TypographyProps } from "@material-ui/core";

export interface SidebarLinkItemProps {
    text: string;
    isActive: boolean;
    onClick(): void;
    IconComponent?: JSX.Element;
    children?: React.ReactNode;
    disabled?: boolean;
    primaryTypographyProps?: TypographyProps;
}

export const SidebarLinkItem = ({ text, isActive, onClick, IconComponent, children, disabled, primaryTypographyProps = { variant: "body2" } }: SidebarLinkItemProps) => {
    return (
        <ListItem disabled={!isActive || disabled} button key={text} onClick={onClick}>
            {IconComponent && <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>}
            <ListItemText primary={text} primaryTypographyProps={primaryTypographyProps} />
            {children && children}
        </ListItem>
    );
};
