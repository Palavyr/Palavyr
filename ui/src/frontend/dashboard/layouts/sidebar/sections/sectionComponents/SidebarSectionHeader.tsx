import React from "react";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ExpandLessIcon from "@material-ui/icons/ExpandLess";
import { makeStyles, ListItem, ListItemText } from "@material-ui/core";

export interface SidebarSectionHeaderProps {
    onClick(): void;
    currentState: boolean;
    title: string;
    menuOpen: boolean;
    className?: string;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    listItemText: {
        textAlign: "center",
    },
}));
export const SidebarSectionHeader = ({ title, onClick, currentState, menuOpen, className = "" }: SidebarSectionHeaderProps) => {
    const cls = useStyles();
    return (
        <ListItem className={className} button onClick={onClick}>
            {menuOpen && <ListItemText style={{ textAlign: "center" }} primary={title} onClick={onClick} primaryTypographyProps={{ variant: "h4" }} />}
            {currentState ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </ListItem>
    );
};
