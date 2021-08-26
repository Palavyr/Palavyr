import React from "react";
import { ReactourStep } from "reactour";
import { TourText } from "./TourText";

export const editorTourSteps: ReactourStep[] = [
    {
        content: <TourText>This is the intent configuration editor. There are a few things to be aware of here.</TourText>,
    },
    {
        content: <TourText>These tabs give you the flow of the editor. They are numbered 1 to 6.</TourText>,
        selector: ".editor-tabs-tour",
    },
    {
        content: <TourText>The pricing editor lets you configure a pdf response that can include static fees, pricing strategies, and other information. This is optional.</TourText>,
        selector: ".response-editor-tab-tour",
    },
    {
        content: <TourText>Once you've designed your pricing strategy PDF, you can preview it here. Please note that some of the data used in the preview is fake data.</TourText>,
        selector: ".preview-tab-tour",
    },
    {
        content: <TourText>The conversation editor is where you will design what your chat bot will say and what questions it will ask.</TourText>,
        selector: ".conversation-editor-tab-tour",
    },
    {
        content: <TourText>This will take you to the email editor, where you can craft the confirmation email.</TourText>,
        selector: ".email-editor-tab-tour",
    },
    {
        content: <TourText>If you need to send attachments with this intent, you can upload them here. These will automatically be sent with the email. Again, this is optional.</TourText>,
        selector: ".attachments-editor-tab-tour",
    },
    {
        content: <TourText>The settings tab is where you can change the name of your intent, set the sender email, as well as delete the intent if you wish.</TourText>,
        selector: ".settings-tab-tour",
    },
];
