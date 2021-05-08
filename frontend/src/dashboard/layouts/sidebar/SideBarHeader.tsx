import React from "react";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import IconButton from "@material-ui/core/IconButton";
import { makeStyles, Typography } from "@material-ui/core";

interface SideBarHeaderProps {
    handleDrawerClose: () => void;
    side?: "right" | "left";
    children?: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    drawerHeader: {
        borderTopLeftRadius: "7px",
        borderTopRightRadius: "7px",
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
    button: {
        borderRadius: "10px",
        "&:hover": {
            backgroundColor: theme.palette.primary.light
        }
    }
}));

export const SideBarHeader = ({ handleDrawerClose, children, side = "left" }: SideBarHeaderProps) => {
    const cls = useStyles();

    return (
        <div className={cls.drawerHeader}>
            <IconButton className={cls.button} onClick={() => handleDrawerClose()}>
                {children}
                {side === "left" ? <ChevronLeftIcon className={cls.icon} /> : <ChevronRightIcon className={cls.icon} />}
            </IconButton>
        </div>
    );
};
