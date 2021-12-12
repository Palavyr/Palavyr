import * as React from "react";

import { useEffect, useRef } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "@widgetcore/BotResponse/utils/renderNextComponent";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { _openFullscreenPreview } from "@store-actions";
import { getRootNode } from "@widgetcore/BotResponse/utils/utils";
import { ConvoHeader } from "@widgetcore/components/ConvoHeader/ConvoHeader";
import { Messages } from "@widgetcore/components/Messages/Messages";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";
import { shortStaticConvoSequence } from "@frontend/dashboard/content/designer/dummy_conversations";

import { WidgetNodeResource } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    root: {
        display: "flex",
        flexDirection: "column",
        height: "100%",
        width: "100%",
        backgroundColor: theme.palette.background.default,
        overflow: "hidden",
    },
}));

export interface WidgetProps {
    designMode?: boolean;
}

export const Widget = ({ designMode }: WidgetProps) => {
    const location = useLocation();
    const cls = useStyles();
    let secretKey = new URLSearchParams(location.search).get("key");
    if (!secretKey) {
        secretKey = "123";
    }
    const client = new PalavyrWidgetRepository(secretKey);

    const initializeIntroduction = async () => {
        const intro = await client.Widget.Get.IntroSequence();
        const rootNode = getRootNode(intro);
        renderNextComponent(rootNode, intro, client, null);
    };

    const initializeDesignMode = async () => {
        const conversation = shortStaticConvoSequence("area52") as WidgetNodeResource[];
        for (const message of conversation) {
            renderNextComponent(message, conversation, client, null);
        }
    };

    useEffect(() => {
        (async () => {
            if (designMode) {
                initializeDesignMode();
            } else {
                await initializeIntroduction();
            }
        })();

        return () => {};
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        messageRef.current = document.getElementById("messages") as HTMLDivElement;
        return () => {
            messageRef.current = null;
        };
    }, []);

    useEffect(() => {
        document.body.setAttribute("style", `overflow: "hidden"`);
    }, []);

    return (
        <div className={cls.root}>
            <div>
                <ConvoHeader titleAvatar={""} />
            </div>
            <div style={{ flexGrow: 1 }}>
                <Messages profileAvatar={""} showTimeStamp={true} messageRef={messageRef} />
            </div>
            <BrandingStrip />
        </div>
    );
};
