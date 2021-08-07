import React from "react";
import { HelpDetails } from "./helpComponents/HelpDetails";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";
import { MoreInformation } from "./helpComponents/MoreInformation";

export const ToursPageHelp = () => {
    const title = "Palavyr Tours";
    const details = "You can retake tours if you need a refresher.";

    return (
        <>
            <HelpTitle title={title} />
            <HelpSubtitle subtitle={details} />
            <HelpDetails>
                <span>Palavyr tours will highlight specific components of the dashboard and provide some helpful information about those items.</span>
                <span>Once a tour is completed, or you exit out of it early, a cookie will be used to remember that you've taken the tour. This prevents the tour from popping up each time you visit the page.</span>
                <span>If you would like to retake a tour for any reason, you can visit this page and click on the desired tour.</span>
            </HelpDetails>
            <MoreInformation />
        </>
    );
};
