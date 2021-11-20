import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "./PalavyrWidgetTheme";
import { Provider } from "react-redux";
import { WidgetApp } from "./WidgetApp";
import { PalavyrWidgetStore } from "widget/store/store";
import { TestComponent } from "widget/test/testComponent";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/ErrorBoundaries/AppLevelErrorBoundary";

import ReactGA from "react-ga";
import RouteChangeTracker from "@common/Analytics/RouteChangeTracker";
import { googleAnalyticsTrackingId, isDevelopmentStage } from "@api-client/clientUtils";

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
                        <Route exact path="/widget" component={WidgetApp} />
                        <Route exact path="/test" component={TestComponent} />
                        <RouteChangeTracker />
                    </Router>
                </MuiThemeProvider>
            </Provider>
        </ErrorBoundary>
    </React.StrictMode>,
    document.getElementById("root")
);
