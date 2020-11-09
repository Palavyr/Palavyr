import React from "react";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import IconButton from "@material-ui/core/IconButton";
import { makeStyles, useTheme } from "@material-ui/core";
import { UserDetails } from "../header/UserDetails";

interface SideBarHeaderProps {
    handleDrawerClose: () => void;
}

const useStyles = makeStyles(theme => ({
    drawerHeader: {
        border: "0px solid white",
        // backgroundColor: "#686de0",
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        justifyContent: "flex-end",
        ...theme.mixins.toolbar,
    }
}))

export const SideBarHeader = ({ handleDrawerClose }: SideBarHeaderProps) => {

    const classes = useStyles();
    const theme = useTheme();

    return (
        <>
            <div className={classes.drawerHeader}>
                <IconButton onClick={() => handleDrawerClose()}>
                    <div>
                        {theme.direction === "ltr" ? <ChevronLeftIcon style={{color: "black"}} /> : <ChevronRightIcon style={{color: "black"}} />}
                        </div>
                </IconButton>
            </div>
            <UserDetails />
        </>
    );
};
