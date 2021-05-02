import React, { useState } from "react";
import classNames from "classnames";
import { makeStyles } from "@material-ui/core";
import { DRAWER_WIDTH } from "@constants";
import LinearProgress from "@material-ui/core/LinearProgress";
import { DevStagingStrip } from "@common/components/devIndicators/DevStagingStrip";
import { yellow } from "@material-ui/core/colors";
import { isDevelopmentStage } from "@api-client/clientUtils";

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
    loading: {
        backgroundColor: yellow[300],
    },
    strip: {
        backgroundColor: theme.palette.warning.main,
        height: "1rem",
    },
}));

export const ContentLoader = ({ open, isLoading, dashboardAreasLoading, children }: IContentLoader) => {
    const cls = useStyles();
    const isDev = isDevelopmentStage();
    const [show, setShow] = useState<boolean>(isDev);

    return (
        <main className={classNames(cls.content, { [cls.contentShift]: open })}>
            {(isLoading || dashboardAreasLoading) && <LinearProgress className={cls.loading} />}
            {isDev && <DevStagingStrip show={show} setShow={setShow} />}
            {!show && <div className={cls.strip} />}
            <div>{children}</div>
        </main>
    );
};
