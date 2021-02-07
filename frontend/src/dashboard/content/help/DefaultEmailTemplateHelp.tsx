import React from "react";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";
import { MoreInformation } from "./helpComponents/MoreInformation";

export const DefaultEmailTemplateHelp = () => {
    const title = "Set your Default Fallback Email Template";
    const details = "Use this option set a default fallback email that will be sent in the event of a 'too complicated' palaver result in areas that don't provide their own.";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
            <MoreInformation />
        </>
    );
};
