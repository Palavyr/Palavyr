import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "./PalavyrWidgetTheme";
import { Provider } from "react-redux";
import { App } from "./App";
import { PalavyrWidgetStore } from "store/store";
import { TestComponent } from "test/testComponent";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "common/ErrorBoundaries/AppLevelErrorBoundary";

import ReactGA from "react-ga";
import RouteChangeTracker from "Analytics/RouteChangeTracker";
import { googleAnalyticsTrackingId, isDevelopmentStage } from "client/clientUtils";

if (!isDevelopmentStage()) {
    ReactGA.initialize(googleAnalyticsTrackingId);
}

ReactDOM.render(
    <React.StrictMode>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <Provider store={PalavyrWidgetStore}>
                <MuiThemeProvider theme={PalavyrWidgetTheme}>
                    <CssBaseline />
                    <Router>
                        <Route exact path="/widget" component={App} />
                        <Route exact path="/test" component={TestComponent} />
                        <RouteChangeTracker />
                    </Router>
                </MuiThemeProvider>
            </Provider>
        </ErrorBoundary>
    </React.StrictMode>,
    document.getElementById("root")
);
