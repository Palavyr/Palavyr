import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "./PalavyrWidgetTheme";
import { WidgetApp } from "./WidgetApp";
import { TestComponent } from "widget/test/testComponent";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/ErrorBoundaries/AppLevelErrorBoundary";

import ReactGA from "react-ga";
import RouteChangeTracker from "@common/Analytics/RouteChangeTracker";
import { googleAnalyticsTrackingId, isDevelopmentStage } from "@api-client/clientUtils";
import { BehaviorState, ContextProperties, ContextState, MessagesState } from "@Palavyr-Types";

if (!isDevelopmentStage()) {
    ReactGA.initialize(googleAnalyticsTrackingId);
}

export interface AppContext extends BehaviorState, ContextState, MessagesState {}

ReactDOM.render(
    <React.StrictMode>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <MuiThemeProvider theme={PalavyrWidgetTheme}>
                <CssBaseline />
                <Router>
                    <Route exact path="/widget" component={WidgetApp} />
                    <Route exact path="/test" component={TestComponent} />
                    <RouteChangeTracker />
                </Router>
            </MuiThemeProvider>
        </ErrorBoundary>
    </React.StrictMode>,
    document.getElementById("root")
);
