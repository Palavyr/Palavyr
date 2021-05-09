import React from "react";
import ReactDOM from "react-dom";
import AOS from "aos";
import "aos/dist/aos.css";
import { ReactQueryDevtools } from "react-query-devtools";

import { App } from "./App";

import ReactGA from "react-ga";
import { googleAnalyticsTrackingId } from "@api-client/clientUtils";
ReactGA.initialize(googleAnalyticsTrackingId);

ReactDOM.render(
    <>
        <App />
        {/* <ReactQueryDevtools initialIsOpen /> */}
    </>,
    document.getElementById("root")
);
