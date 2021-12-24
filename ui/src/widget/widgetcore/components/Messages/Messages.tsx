import React, { useContext, useEffect, useRef } from "react";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { WidgetPreferences } from "@Palavyr-Types";
import { scrollToBottom } from "@widgetcore/utils/messages";
import { getComponentToRender } from "@widgetcore/BotResponse/utils/getComponentToRender";
import { makeStyles } from "@material-ui/core";

import { WidgetContext } from "@widgetcore/context/WidgetContext";

import "@widgetcore/widget/widget.module.scss";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import classNames from "classnames";
import { useAppContext } from "widget/hook";

export interface MessageProps {
    showTimeStamp: boolean;
    profileAvatar?: string;
}

const useStyles = makeStyles(theme => ({
    message: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        overflowY: "hidden",
    }),
    messageTubeContainer: (prefs: WidgetPreferences) => ({
        paddingTop: "2rem",
        backgroundColor: prefs.chatBubbleColor,
        paddingLeft: "0.8rem",
        paddingRight: "0.8rem",
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: MessageProps) => {
    const { messages, loading } = useAppContext();
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });
    const wcls = useWidgetStyles();


    useEffect(() => {
        document.body.setAttribute("style", `overflow: "hidden"`);
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, [messages, loading]);

    return (
        <div id="messages" className={classNames(wcls.pwrow, wcls.pcontent, cls.messageTubeContainer)} ref={messageRef}>
            {messages?.map((message, index) => (
                <div className={cls.message} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {getComponentToRender(message, showTimeStamp)}
                </div>
            ))}
            {loading && <Loader typing={loading} />}
            <div style={{ height: "3rem" }} />
        </div>
    );
};
