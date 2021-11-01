import React from "react";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";
import { MoreInformation } from "./helpComponents/MoreInformation";

export const ConversationReviewHelp = () => {
    const title = "Check your Conversation";
    const details = "You can review the precise conversation with your widget here along with all of the answers provided";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
            <MoreInformation />
        </>
    );
};
