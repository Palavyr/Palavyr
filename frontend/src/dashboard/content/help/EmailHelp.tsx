import React from "react";
import { HelpDetails } from "./helpComponents/HelpDetails";
import { HelpSubtitle } from "./helpComponents/HelpSubtitle";
import { HelpTitle } from "./helpComponents/HelpTitle";

export const EmailHelp = () => {
    return (
        <>
            <HelpTitle title="Response Email Configuration" />
            <HelpSubtitle subtitle="" />
            <HelpDetails>
                <span>This section is used to configure the email used to send the PDF response. What you see below is what your users will see in their inbox. </span>
                <span>There are two ways to configure your response emails: </span>
                <ol>
                    <li>Upload an html template you prepared earlier</li>
                    <li>Use our inline editor</li>
                </ol>
                <span>
                    <h4>Upload</h4>
                    To upload a precreated email template, you simply need to use the upload dialog to select your email html file. We currently only support html formats for uploads.
                </span>
                <span>
                    <h4>Inline Editor</h4>
                    If you don't have a prepared html email template, you can use our inline editor. This provides functionality to fully customize your email response in rich text. Behind the scenes, this editor will convert your formatted text to
                    html.
                    <i>Note: The inline editor feature is only available with the pro subscription plan.</i>
                </span>
                <span>
                    <h4>Fallback Email</h4>
                    <p>You have the option to specify an area specific fallback email in cases where your Palavyr ends with a 'Too Complicated' conversation node. This email will be sent as is without any attachments.</p>
                    <p>
                        If you have opted to not send an area specific email, then the default fallback email will be used, so be sure to set this email in the general settings.
                    </p>
                </span>
            </HelpDetails>
        </>
    );
};
