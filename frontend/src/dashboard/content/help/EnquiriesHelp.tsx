import React from "react";
import { HelpDetails } from "./helpComponents/HelpDetails";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";
import { MoreInformation } from "./helpComponents/MoreInformation";

export const EnquiriesHelp = () => {
    const title = "Check your enquiries";
    const details = "This table lists all of the completed enquires you have received. Enquiries you have not checked will be in bold.";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
            <HelpDetails>
                <span></span>
            </HelpDetails>
            <MoreInformation/>
        </>
    );
};
