import React from "react";
import { ReactourStep } from "reactour";
import { TourText } from "./TourText";

export const editorTourSteps: ReactourStep[] = [
    {
        content: <TourText>This is the area configuration editor. There are a few things to be aware of here.</TourText>,
    },

    {
        content: <TourText>These tabs give you the flow of the editor. They are numbered 1 to 6.</TourText>,
        selector: ".editor-tabs-tour",
    },
    {
        content: <TourText>The email editor lets you create the email that will be sent upon a completed conversation. Sending emails at the end of chats is optional.</TourText>,
        selector: ".email-editor-tab-tour",
    },
    {
        content: <TourText>The response editor lets you configure a pdf response that can include static fees, pricing strategies, and other information. This is also optional.</TourText>,
        selector: ".response-editor-tab-tour",
    },
    {
        content: <TourText>If you need to send attachments with this area, you can upload them here. These will automatically be sent with the email. Again, this is optional.</TourText>,
        selector: ".attachments-editor-tab-tour",
    },
    {
        content: <TourText>The conversation editor is where you will design what your chat bot will say and what questions it will ask.</TourText>,
        selector: ".conversation-editor-tab-tour",
    },
    {
        content: <TourText>The settings tab is where you can change the name of your area, set the sender email, as well as delete the area if you wish.</TourText>,
        selector: ".settings-tab-tour",
    },
    {
        content: <TourText>When you've finished designing your area, you can preview the response PDF. Please note that some of the data used in the preview is fake data.</TourText>,
        selector: ".preview-tab-tour",
    },
];
