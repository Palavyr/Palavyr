import React from "react";
import { ReactourStep } from "reactour";
import { TourText } from "./TourText";

export const welcomeTourSteps: ReactourStep[] = [
    {
        content: <TourText>Welcome to Palavyr.com! I hope you are as excited as we are to build a brand-spanking-new chatbot!</TourText>,
    },
    {
        content: <TourText>This guided tour will show you around to get your oriented.</TourText>,
    },
    {
        content: <TourText>This is the navigation sidebar. You can get to anywhere in Palavyr from here.</TourText>,
        selector: ".sidebar-tour",
    },
    {
        content: <TourText>You can collapse this sidebar anytime to make space on the page.</TourText>,
        selector: ".menu-collapse-tour",
    },
    {
        selector: ".widget-state-switch",
        content: <TourText>This is the live widget status switch. Click to enable when you're ready to show your widget to the world!</TourText>,
    },
    {
        selector: ".widget-state-switch",
        content: (
            <TourText>
                While this is disabled, your widget will show "not ready".<br></br> <strong>Ask your web designer to make sure the widget doesn't load if this is disabled.</strong>
            </TourText>
        ),
    },
    {
        selector: ".configure-tour",
        content: (
            <TourText>
                This is the <strong>area</strong> configuration section. You will build a chatbot conversation for each <strong>area</strong>, and in this section you will...
            </TourText>
        ),
    },
    {
        selector: ".add-new-area-tour",
        content: (
            <TourText>
                ... add a new <strong>area</strong>...
            </TourText>
        ),
    },
    {
        selector: ".enable-disable-area-tour",
        content: (
            <TourText>
                ... enable and disable specific <strong>areas</strong> (so they show or don't show in your chat options)...
            </TourText>
        ),
    },
    {
        selector: ".configure-your-area-tour",
        content: (
            <TourText>
                ...and of course configure each <strong>area</strong> for your chatbot!
            </TourText>
        ),
    },
    {
        selector: ".review-sidebar-tour",
        content: <TourText>This section lets you review various aspects of your chatbot.</TourText>,
    },

    {
        selector: ".activity-sidebar-tour",
        content: <TourText>Check out stats about the usage of your bot, and get feedback about your designs.</TourText>,
    },

    {
        selector: ".check-enquiries-sidebar-tour",
        content: <TourText>Review your current enquiries and get proactive!</TourText>,
    },

    {
        selector: ".check-enquiries-badge-sidebar-tour",
        content: <TourText>You'll know if you have any unseen enquiries</TourText>,
        position: "top",
    },
    {
        selector: ".widget-designer-tour",
        content: <TourText>The widget designer lets you customize the widget to keep on brand</TourText>,
    },
    {
        selector: ".chat-demo-link-tour",
        content: <TourText>Review your chatbot configuration before you go live.</TourText>,
    },
    {
        selector: ".uploads-sidebar-tour",
        content: <TourText>If you have a Premium or Pro subscription, you can upload and review image uploads here.</TourText>,
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
        selector: ".get-widget-sidebar-tour",
        content: <TourText>When its time to place the widget into your website, your web developer will find what they need here.</TourText>,
    },
    {
        selector: ".quick-start-sidebar-tour",
        content: <TourText>Click here if you need to come to review the quick start guide.</TourText>,
    },
    {
        selector: ".take-tours-sidebard-tour",
        content: <TourText>Finally, if you want to see this or any other tour again, you can them here.</TourText>,
    },
    {
        selector: ".quick-start-guide-tour",
        content: <TourText>Now that you've got a basic orientation of the dashboard, follow the quick start guide to get going!</TourText>,
    },
];
