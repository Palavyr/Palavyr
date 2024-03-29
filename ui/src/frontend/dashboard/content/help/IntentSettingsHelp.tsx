import React from "react";
import { HelpDetails } from "./helpComponents/HelpDetails";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";

export const IntentSettingsHelp = () => {
    return (
        <>
            <HelpTitle title="Conversation Design Help" />
            <HelpSubtitle subtitle="" />
            <HelpDetails>
                <span>This section is used to configure settings that are specific to this intent.</span>

                <span>
                    <h4>Enable / Disable</h4>
                    <p>You can enable and disable your intent using the toggle at the top center. If you disable the intent, it will not appear in the live Palavyr widget. It will, however, still appear in the Palavyr Demo widget on the demo page.</p>
                </span>
                <span>
                    <h4>Intent Name</h4>
                    <p>You can set the name used for this intent in the Palavyr widget. This name will appear in the intent selector in the widget.</p>
                </span>
                <span>
                    <h4>Intent Email</h4>
                    <p>
                        You can set the email which will be used to send the email to the customer. This allows you to customize your responses in a way that give the customer the impression that you have specific personelle handling specific types
                        of reqeusts and provides a more personalized experience.
                    </p>
                </span>
                <span>
                    <h4>Delete Intent</h4>
                    <p>If you wish, you can delete the intent. This is a permanent action and is not undoable.</p>
                </span>
            </HelpDetails>
        </>
    );
};
