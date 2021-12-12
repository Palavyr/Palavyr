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
import classNames from "classnames";
import "./style.scss";

export interface WidgetProps {
    designMode?: boolean;
}

export const SmoothWidget = ({ designMode }: WidgetProps) => {
    const location = useLocation();

    let secretKey: string | undefined | null;
    if (designMode) {
        secretKey = "123";
    } else {
        secretKey = new URLSearchParams(location.search).get("key");
    }
    const client = new PalavyrWidgetRepository(secretKey);

    const initializeIntroduction = async () => {
        const intro = await client.Widget.Get.IntroSequence();
        const rootNode = getRootNode(intro);
        renderNextComponent(rootNode, intro, client, null);
    };

    const initializeDesignMode = async () => {


    }

    useEffect(() => {
        (async () => {
            if (designMode) {
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
        <div className={classNames("rcw-conversation-container", "active")} aria-live="polite">
            <ConvoHeader titleAvatar={""} />
            <Messages profileAvatar={""} showTimeStamp={true} />
            <BrandingStrip />
        </div>
    );
};
