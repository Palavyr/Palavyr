import React from "react";
import { HelpSubtitle } from "./helpTitle/HelpSubtitle";
import { HelpTitle } from "./helpTitle/HelpTitle";

export const ConversationReviewHelp = () => {
    const title = "Check your Conversation";
    const details = "You can review the precise conversation with your widget here along with all of the answers provided";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
        </>
    );
};
