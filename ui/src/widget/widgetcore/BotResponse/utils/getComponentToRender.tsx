import React from "react";
import { IMessage, CustomCompMessage } from "@Palavyr-Types";
import { MessageWrapper } from "./MessageWrapper";
import { MESSAGES_TYPES } from "@widgetcore/constants";

export const getComponentToRender = (message: IMessage | CustomCompMessage, showTimeStamp: boolean) => {
    if (message.type === "Bot") {
        // for the custom components
        const PalavyrComponent = message.component as CustomCompMessage["component"];
        return (
            <MessageWrapper>
                <PalavyrComponent {...message.props} />
            </MessageWrapper>
        );
    } else if (message.type === MESSAGES_TYPES.TEXT) {
        // for the user responses
        const IMessageComponent = message.component;
        return <IMessageComponent message={message} showTimeStamp={showTimeStamp} />;
    } else {
        throw new Error("Unknown message type");
    }
};
