import React from "react";
import { ReactourStep } from "reactour";
import { TourText } from "./TourText";

export const welcomeTourSteps: ReactourStep[] = [
    {
        content: <TourText>Welcome to Palavyr.com! I hope you are as excited as we are to build a brand-spanking-new chatbot! Lets start with a quick tour.</TourText>,
    },
    {
        selector: ".widget-state-switch",
        content: <TourText>This is the live widget status switch. Click to enable when you're ready to show your widget to the world! If your widget is disabled, it will simply display "not ready"</TourText>,
    },
    {
        selector: ".configure-tour",
        content: (
            <TourText>
                This is the <strong>intent</strong> configuration section. You will build a chatbot conversation for each <strong>intent</strong>.
            </TourText>
        ),
    },
    {
        selector: ".review-sidebar-tour",
        content: <TourText>This section lets you review various aspects of your chatbot, such as usage, enquires, and design.</TourText>,
    },

    {
        selector: ".billing-sidebar-tour",
        content: <TourText>Palavyr has a lot of great features that require a subscription. Please consider subscribing! :D</TourText>,
    },
    {
        selector: ".settings-sidebar-tour",
        content: <TourText>These are the general application settings. Make sure you review these before you go live.</TourText>,
    },
    {
        selector: ".quick-start-guide-tour",
        content: <TourText>Now that you've got a basic orientation of the dashboard, follow the quick start guide to get going!</TourText>,
    },
];
