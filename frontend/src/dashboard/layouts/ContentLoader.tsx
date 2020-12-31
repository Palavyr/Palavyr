import React from "react";
import classNames from "classnames";
import { makeStyles, useTheme } from "@material-ui/core";
import { DRAWER_WIDTH } from "@common/constants";

interface IContentLoader {
    open: boolean;
    children: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    content: {
        position: "relative",
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
}));

export const ContentLoader = ({ open, children }: IContentLoader) => {
    const classes = useStyles();
    return (
        <main className={classNames(classes.content, { [classes.contentShift]: open })}>

            <div>{children}</div>
        </main>
    );
};
