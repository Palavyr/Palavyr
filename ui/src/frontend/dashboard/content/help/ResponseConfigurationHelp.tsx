import React from "react";
import { HelpDetails } from "./helpComponents/HelpDetails";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";

export const ResponseConfigurationHelp = () => {
    return (
        <>
            <HelpTitle title="PDF Response Configuration" />
            <HelpSubtitle subtitle="" />
            <HelpDetails>
                <span>This section is used to configure the body of your Response PDF (see the preview tab for a current example). </span>
                <p>The body of the Response PDF can broken into four section: </p>
                <ol>
                    <li>The Introductory state</li>
                    <li>Customized Fees</li>
                    <li>Static Fees</li>
                    <li>The Outro statement</li>
                </ol>
                <span>
                    <h4>Introductory and Outro Statements</h4>
                    You can use these sections however you'd like. Typically an intro statement might be used to explain the presented tables, wheras the outro statement might be used to provide terms and conditions, disclaimers, indemnity clauses, a
                    combination of those, or somethign else entirely.
                </span>
                <span>
                    <h4>Customized fees</h4>
                    The customized fees are linked to the conversation you design (should you choose to include any). When you configure a custom fee, that fee appears in the conversation node dropdown selection menu. Essentially, this table
                    configures how specific responses to that conversation node will affect the final calculation when providing fee estimates.
                </span>
                <span>
                    <h4>Static Fees</h4>
                    <p>
                        The static fees are items you wish to include regardless of how the conversation goes. These might be standard costs associated with your services that fall within a range or (optionally) are calculated per person. If the per
                    person option is selected for any items within the static fees, then the 'Ask how many individuals' becomes a required question for the conversation tree to be complete.
                    </p>
                    <p>
                        An additional option is available with each static fee table for selecting whether or not you would like to request a number of individuals from your client. This is used in cases where you have specified a 'per person' qualifier on the static table fee.
                        Keep in mind that if you collect a number of individuals elsewhere in the Palavyr that is not related to the static fees, it may be a good idea to keep this option un-selected.
                    </p>

                </span>
            </HelpDetails>
        </>
    );
};
