import React, { useState } from "react";
import classNames from "classnames";
import { makeStyles } from "@material-ui/core";
import { DRAWER_WIDTH, MAIN_CONTENT_DIV_ID } from "@constants";
import { DevStagingStrip } from "@common/components/devIndicators/DevStagingStrip";
import { yellow } from "@material-ui/core/colors";
import { isDevelopmentStage } from "@api-client/clientUtils";
import { YellowStrip } from "@common/components/YellowStrip";

interface IContentLoader {
    open: boolean;
    children: React.ReactNode;
}

const useStyles = makeStyles(theme => ({
    toolbar: {
        display: "flex",
        alignItems: "center",
        justifyContent: "flex-end",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        ...theme.mixins.toolbar,
    },
    content: {
        width: "100vw",
        flexGrow: 1,
        paddingBottom: theme.spacing(5),
    },
}));

export const ContentLoader = ({ open, children }: IContentLoader) => {
    const cls = useStyles();
    const isDev = isDevelopmentStage();
    const [show, setShow] = useState<boolean>(isDev);

    return (
        <main className={cls.content}>
            <div className={cls.toolbar} />
            {/* {isDev && <DevStagingStrip show={show} setShow={setShow} />} */}
            {children}
        </main>
    );
};
