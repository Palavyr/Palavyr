import React from "react";
import { HelpSubtitle } from "./helpTitle/HelpSubtitle";
import { HelpTitle } from "./helpTitle/HelpTitle";

export const EnquiriesHelp = () => {
    const title = "Check your enquiries";
    const details = "This table lists all of the completed enquires you have received. Enquiries you have not checked will be in bold.";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
        </>
    );
};
