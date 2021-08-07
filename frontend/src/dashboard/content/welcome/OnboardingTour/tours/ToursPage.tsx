import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { EDITOR_TOUR_COOKIE_NAME, WELCOME_TOUR_COOKIE_NAME } from "@constants";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { QuickStartCard } from "../../quickStartGuide/QuickStartCard";
import Cookies from "js-cookie";
import { useHistory, useLocation } from "react-router-dom";
import { createNavLink } from "dashboard/layouts/sidebar/sections/sectionComponents/AreaLinkItem";

export const ToursPage = () => {
    const { reRenderDashboard, areaNameDetails } = useContext(DashboardContext);
    const history = useHistory();
    const location = useLocation();

    const enableGettingStartedTour = () => {
        if (Cookies.get(WELCOME_TOUR_COOKIE_NAME) !== undefined) {
            Cookies.remove(WELCOME_TOUR_COOKIE_NAME);
        }

        if (location.pathname === "/dashboard/welcome") {
            reRenderDashboard();
        } else {
            history.push("/dashboard/welcome");
        }
    };

    const enableAreaEditorTour = () => {
        if (Cookies.get(EDITOR_TOUR_COOKIE_NAME) !== undefined) {
            Cookies.remove(EDITOR_TOUR_COOKIE_NAME);
        }

        const navUrl = createNavLink(areaNameDetails[0].areaIdentifier);
        history.push(navUrl);
    };

    return (
        <>
            <AreaConfigurationHeader divider title="Palavyr Product Tours" subtitle="Click any of the following tours to restart the product tours." />
            <QuickStartCard title="Welcome Tour" content="This tour will cover the basic navigation components of Palavyr, such as the sections in the side bar navigator." onClick={enableGettingStartedTour} />
            <QuickStartCard title="Area Editor Tour" content="This tour will cover basic navigation of the Area Editor." onClick={enableAreaEditorTour} />
        </>
    );
};
