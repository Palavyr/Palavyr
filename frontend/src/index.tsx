import React from "react";
import ReactDOM from "react-dom";
import AOS from "aos";
import "aos/dist/aos.css";
import { ReactQueryDevtools } from "react-query-devtools";
import ReactGA from "react-ga";
import { googleAnalyticsTrackingId, isDevelopmentStage } from "@api-client/clientUtils";

import { App } from "./App";

if (!isDevelopmentStage()) {
    ReactGA.initialize(googleAnalyticsTrackingId);
}

ReactDOM.render(
    <>
        <App />
        {/* <ReactQueryDevtools initialIsOpen /> */}
    </>,
    document.getElementById("root")
);
