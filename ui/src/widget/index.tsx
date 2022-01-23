import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route, useLocation, Switch } from "react-router-dom";
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
        <>
            <Route exact path="/widget" component={WidgetApp} />
            <Route exact path="/test" component={TestComponent} />
        </>
    );
};

const App = () => {
    return (
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <MuiThemeProvider theme={PalavyrWidgetTheme}>
                <CssBaseline />
                <Router>
                    <Switch>
                        <WidgetRoutes />
                    </Switch>
                </Router>
            </MuiThemeProvider>
        </ErrorBoundary>
    );
};

ReactDOM.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>,
    document.getElementById("root")
);
