import React from "react";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import IconButton from "@material-ui/core/IconButton";
import { makeStyles, Typography } from "@material-ui/core";

interface SideBarHeaderProps {
    handleDrawerClose: () => void;
}

const useStyles = makeStyles((theme) => ({
    drawerHeader: {
        backgroundColor: theme.palette.primary.dark,
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        justifyContent: "flex-end",
        ...theme.mixins.toolbar,
    },
    icon: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
    },
}));

export const SideBarHeader = ({ handleDrawerClose }: SideBarHeaderProps) => {
    const cls = useStyles();

    return (
        <div className={cls.drawerHeader}>
            <IconButton onClick={() => handleDrawerClose()}>
                <ChevronLeftIcon className={cls.icon} />
            </IconButton>
        </div>
    );
};
