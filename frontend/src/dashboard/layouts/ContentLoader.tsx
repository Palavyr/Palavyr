import React from "react";
import classNames from "classnames";
import { Hidden, makeStyles, useTheme } from "@material-ui/core";
import { DRAWER_WIDTH, HELP_DRAWER_WIDTH } from "@common/constants";

interface IContentLoader {
    open: boolean;
    children: React.ReactNode;
}

const useStyles = makeStyles(theme => ({
    content: {
        position: "relative", // TESTING
        flexGrow: 1,
        top: theme.mixins.toolbar.minHeight,
        transition: theme.transitions.create("margin", {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        marginLeft: -DRAWER_WIDTH,
    },
    contentShift: {
        transition: theme.transitions.create("margin", {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
        marginLeft: 0,
    },
    // appBarSpacer: theme.mixins.toolbar,

}))


export const ContentLoader = ({ open, children }: IContentLoader) => {
    const classes = useStyles();
    const them = useTheme();
    console.log("WOw: " + them.mixins.toolbar.minHeight);
    return (
        <main
            className={
                classNames(
                    classes.content,
                    { [classes.contentShift]: open }
                )
            }
        >
            {/* appbar spacer needed */}
            {/* <div className={classes.appBarSpacer} /> */}
            {/* */}

            <div>
                {children}
            </div>
        </main>
    );
};
