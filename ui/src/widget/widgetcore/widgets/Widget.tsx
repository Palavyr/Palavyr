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

import { WidgetNodeResource } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { designerData } from "@frontend/dashboard/content/designer/data/designerData";
import { addResponseMessage, addUserMessage, dropMessages } from "@store-dispatcher";

const useStyles = makeStyles(theme => ({
    root: {
        // display: "flex",
        // flexDirection: "column",
        // height: "100%",
        width: "100%",
        height: "100%",
        overflowY: "hidden",
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
        const conversation = designerData as WidgetNodeResource[];
        renderNextComponent(conversation[0], conversation, client, null);
    };
    const initialize = React.useCallback(async () => {
        dropMessages();
        if (designMode) {
            initializeDesignMode();
        } else {
            await initializeIntroduction();
        }
    }, []);

    useEffect(() => {
        initialize();
        return () => {};
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        messageRef.current = document.getElementById("messages") as HTMLDivElement;
        return () => {
            messageRef.current = null;
        };
    }, []);


    return (
        <div style={{ height: "100%", width: "100%", minHeight: '100%' }}>
            <div className={cls.root}>
                <ConvoHeader titleAvatar={""} />
                <Messages profileAvatar={""} showTimeStamp={true} />
                <BrandingStrip />
            </div>
        </div>
    );
};
