import * as React from "react";
import { MuiThemeProvider, CssBaseline } from "@material-ui/core";
import { BrowserRouter } from "react-router-dom";
import theme from "./theme";
import { Routes } from "@public-routes";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/ErrorBoundaries/ErrorFallback";

const App = () => {
    return (
        <div id="innerbody">
            <ErrorBoundary FallbackComponent={ErrorFallback}>
                <BrowserRouter>
                    <MuiThemeProvider theme={theme}>
                        <CssBaseline />
                        <Routes />
                    </MuiThemeProvider>
                </BrowserRouter>
            </ErrorBoundary>
        </div>
    );
};

export { App };
