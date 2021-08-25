import React, { useContext, useEffect, useRef } from "react";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { makeStyles } from "@material-ui/core";

import "./styles.scss";
import classNames from "classnames";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "../../context/WidgetContext";
import { scrollToBottom } from "../../utils/messages";
import { fakeMessages } from "../../../fakeMessages";
import { getComponentToRender } from "../../BotResponse/utils/getComponentToRender";

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
};

const useStyles = makeStyles(theme => ({
    face: {
        height: "4rem",
    },
    messageTube: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        height: "100%",
    }),
    messages: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: Props) => {
    const { preferences } = useContext(WidgetContext);

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    const cls = useStyles({ ...preferences });

    return (
        <>
            <div id="messages" className={classNames("rcw-messages-container", cls.messageTube)} ref={messageRef} style={{ paddingBottom: "2rem" }}>
                {fakeMessages.map((message, index) => (
                    <div className={classNames("rcw-message", cls.messages)} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                        {getComponentToRender(message, showTimeStamp)}
                    </div>
                ))}
                <Loader typing={true} />
            </div>
        </>
    );
};
