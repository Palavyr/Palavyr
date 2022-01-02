import { HeaderStrip } from "@common/components/HeaderStrip";
import { EDITOR_TOUR_COOKIE_NAME, WELCOME_TOUR_COOKIE_NAME } from "@constants";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { QuickStartCard } from "../../quickStartGuide/QuickStartCard";
import Cookies from "js-cookie";
import { useHistory, useLocation } from "react-router-dom";
import { createNavLink } from "frontend/dashboard/layouts/sidebar/sections/sectionComponents/AreaLinkItem";

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
            <HeaderStrip divider title="Palavyr Product Tours" subtitle="Click any of the following tours to restart the product tours." />
            <div style={{ marginTop: "3rem" }}>
                <QuickStartCard
                    title="Welcome Tour"
                    content="This tour will cover the basics of Palavyr."
                    onClick={enableGettingStartedTour}
                />
                <QuickStartCard title="Intent Editor Tour" content="This tour will cover basics of the Intent Editor." onClick={enableAreaEditorTour} />
            </div>
        </>
    );
};
