import React from "react";
import ReactDOM from "react-dom";
import "aos/dist/aos.css";
import ReactGA from "react-ga";
import { googleAnalyticsTrackingId, isDevelopmentStage } from "@api-client/clientUtils";

import { App } from "./App";

if (!isDevelopmentStage()) {
    ReactGA.initialize(googleAnalyticsTrackingId);
}

ReactDOM.render(<App />, document.getElementById("root"));
