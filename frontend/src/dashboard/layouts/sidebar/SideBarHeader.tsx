import React from "react";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import IconButton from "@material-ui/core/IconButton";
import { makeStyles } from "@material-ui/core";

interface SideBarHeaderProps {
    handleDrawerClose: () => void;
    side?: "right" | "left";
    children?: React.ReactNode;
    roundTop?: boolean;
}

type StyleProps = {
    roundTop: boolean;
};

const useStyles = makeStyles((theme) => ({
    drawerHeader: (props: StyleProps) => ({
        borderTopLeftRadius: props.roundTop ? "7px" : "0px",
        borderTopRightRadius: props.roundTop ? "7px" : "0px",
        backgroundColor: theme.palette.primary.main,
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        justifyContent: "flex-end",
        ...theme.mixins.toolbar,
    }),
    icon: {
        color: theme.palette.getContrastText(theme.palette.primary.dark),
    },
    button: {
        borderRadius: "10px",
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
    },
}));

export const SideBarHeader = ({ handleDrawerClose, children, side = "left", roundTop = false }: SideBarHeaderProps) => {
    const cls = useStyles({ roundTop });

    return (
        <div className={cls.drawerHeader}>
            <IconButton className={cls.button} onClick={() => handleDrawerClose()}>
                {children}
                {side === "left" ? <ChevronLeftIcon className={cls.icon} /> : <ChevronRightIcon className={cls.icon} />}
            </IconButton>
        </div>
    );
};
