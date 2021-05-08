import * as React from "react";
import { Fragment, Suspense } from "react";
import { MuiThemeProvider, CssBaseline } from "@material-ui/core";
import { BrowserRouter } from "react-router-dom";
import theme from "./theme";
import { Routes } from "@public-routes";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorFallback } from "@common/components/Errors/ErrorFallback";

const App = () => {
    return (
        <ErrorBoundary FallbackComponent={ErrorFallback}>
            <BrowserRouter>
                <MuiThemeProvider theme={theme}>
                    <CssBaseline />
                    {/* <Suspense fallback={<Fragment />}> */}
                    <Routes />
                    {/* </Suspense> */}
                </MuiThemeProvider>
            </BrowserRouter>
        </ErrorBoundary>
    );
};

export { App };
