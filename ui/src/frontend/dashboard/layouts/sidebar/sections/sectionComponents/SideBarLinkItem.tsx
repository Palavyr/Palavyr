import React from "react";
import { ListItem, ListItemIcon, ListItemText, Tooltip, TypographyProps } from "@material-ui/core";
export interface SidebarLinkItemProps {
    text: string;
    isActive: boolean;
    onClick(): void;
    IconComponent?: JSX.Element;
    children?: React.ReactNode;
    disabled?: boolean;
    primaryTypographyProps?: TypographyProps;
    className?: string;
    toolTipText?: string;
    menuOpen: boolean;
}

export const SidebarLinkItem = ({ text, isActive, className = "", onClick, IconComponent, children, disabled, primaryTypographyProps = { variant: "body2" }, toolTipText, menuOpen }: SidebarLinkItemProps) => {
    return (
        <ListItem className={className} disabled={!isActive || disabled} button key={text} onClick={onClick}>
            {IconComponent && menuOpen && toolTipText ? (
                <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>
            ) : (
                <Tooltip placement="right" title={toolTipText ? toolTipText : ""}>
                    <ListItemIcon onClick={onClick}>{IconComponent}</ListItemIcon>
                </Tooltip>
            )}
            <ListItemText primary={text} primaryTypographyProps={primaryTypographyProps} />
            {children && children}
        </ListItem>
    );
};
