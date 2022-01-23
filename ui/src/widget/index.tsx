import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, useLocation } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "./PalavyrWidgetTheme";
import { WidgetApp } from "./WidgetApp";
import { TestComponent } from "widget/test/testComponent";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/ErrorBoundaries/AppLevelErrorBoundary";
import { WidgetPageView } from "@common/Analytics/gtag";

export const WidgetRoutes = () => {
    const location = useLocation();
    WidgetPageView(location.pathname);
    return (
        <Router>
            <Route exact path="/widget" component={WidgetApp} />
            <Route exact path="/test" component={TestComponent} />
        </Router>
    );
};

ReactDOM.render(
    <React.StrictMode>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <MuiThemeProvider theme={PalavyrWidgetTheme}>
                <CssBaseline />
                <WidgetRoutes />
            </MuiThemeProvider>
        </ErrorBoundary>
    </React.StrictMode>,
    document.getElementById("root")
);
