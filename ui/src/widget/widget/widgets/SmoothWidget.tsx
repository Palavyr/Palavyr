import * as React from "react";

import { useEffect, useRef } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "widget/BotResponse/utils/renderNextComponent";
import { GlobalState, WidgetNodes } from "@Palavyr-Types";
import { useDispatch } from "react-redux";

import { useSelector } from "react-redux";
import cn from "classnames";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { _openFullscreenPreview } from "@store-actions";
import { getRootNode } from "widget/BotResponse/utils/utils";
import { Conversation } from "widget/components/Conversation/Conversation";
import "./style.scss";

export interface WidgetProps {}

export const SmoothWidget = ({}: WidgetProps) => {
    const location = useLocation();
    const secretKey = new URLSearchParams(location.search).get("key");
    const client = new PalavyrWidgetRepository(secretKey);
    const dispatch = useDispatch();

    const initializeIntroduction = async () => {
        const intro = await client.Widget.Get.IntroSequence();

        const rootNode = getRootNode(intro);
        renderNextComponent(rootNode, intro, client, null);
    };

    useEffect(() => {
        (async () => {
            await initializeIntroduction();
        })();

        return () => {};
    }, []);

    const fullScreenMode = true;
    const { showChat, visible } = useSelector((state: GlobalState) => ({
        showChat: state.behaviorReducer.showChat,
        visible: true,
    }));

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        messageRef.current = document.getElementById("messages") as HTMLDivElement;
        return () => {
            messageRef.current = null;
        };
    }, [showChat]);

    const eventHandle = evt => {
        if (evt.target && evt.target.className === "rcw-message-img") {
            const { src, alt, naturalWidth, naturalHeight } = evt.target as HTMLImageElement;
            const obj = {
                src: src,
                alt: alt,
                width: naturalWidth,
                height: naturalHeight,
            };
            dispatch(_openFullscreenPreview(obj));
        }
    };

    /**
     * Previewer needs to prevent body scroll behavior when fullScreenMode is true
     */
    useEffect(() => {
        const target = messageRef?.current;
        target?.addEventListener("click", eventHandle, false);

        return () => {
            target?.removeEventListener("click", eventHandle);
        };

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        document.body.setAttribute("style", `overflow: "hidden"`);
    }, []);

    return (
        <div className={cn("rcw-widget-container", "rcw-full-screen")}>
            <Conversation className={"active"} showTimeStamp={true} />
        </div>
    );
};
