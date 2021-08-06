import React from "react";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ExpandLessIcon from "@material-ui/icons/ExpandLess";
import { makeStyles, ListItem, ListItemText } from "@material-ui/core";

export interface SidebarSectionHeaderProps {
    onClick(): void;
    currentState: boolean;
    title: string;
    className?: string;
}

const useStyles = makeStyles((theme) => ({
    listItemText: {
        textAlign: "center",
    },
}));
export const SidebarSectionHeader = ({ title, onClick, currentState, className = "" }: SidebarSectionHeaderProps) => {
    const cls = useStyles();
    return (
        <ListItem className={className} button onClick={onClick}>
            <ListItemText style={{ textAlign: "center" }} primary={title} onClick={onClick} primaryTypographyProps={{ variant: "h4" }} />
            {currentState ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </ListItem>
    );
};
