import React from "react";
import classNames from "classnames";
import { makeStyles } from "@material-ui/core";
import { DRAWER_WIDTH } from "@constants";
import LinearProgress from "@material-ui/core/LinearProgress";
import { DevStagingStrip } from "@common/components/devIndicators/DevStagingStrip";

interface IContentLoader {
    open: boolean;
    isLoading: boolean;
    dashboardAreasLoading: boolean;
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

export const ContentLoader = ({ open, isLoading, dashboardAreasLoading, children }: IContentLoader) => {
    const cls = useStyles();

    return (
        <main className={classNames(cls.content, { [cls.contentShift]: open })}>
            {(isLoading || dashboardAreasLoading) && <LinearProgress />}
            <DevStagingStrip />
            <div>{children}</div>
        </main>
    );
};
