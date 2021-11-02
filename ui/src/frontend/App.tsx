import * as React from "react";
import { MuiThemeProvider, CssBaseline } from "@material-ui/core";
import { BrowserRouter } from "react-router-dom";
import theme from "./theme";
import { Routes } from "@public-routes";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/ErrorBoundaries/ErrorFallback";
// import RouteChangeTracker from "googleAnalytics/RouteChangeTracker";

const App = () => {
    return (
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <BrowserRouter>
                <MuiThemeProvider theme={theme}>
                    <CssBaseline />
                    <Routes />
                </MuiThemeProvider>
                {/* <RouteChangeTracker /> */}
            </BrowserRouter>
        </ErrorBoundary>
    );
};

export { App };
