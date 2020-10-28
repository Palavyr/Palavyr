import * as React from 'react';
import { Fragment, Suspense } from "react";
import { MuiThemeProvider, CssBaseline } from "@material-ui/core";
import { BrowserRouter } from "react-router-dom";
import theme from "./theme";
import { Routes } from '@public-routes';


const App = () => {
    return (
        <BrowserRouter>
            <MuiThemeProvider theme={theme}>
                <CssBaseline />
                {/* <Suspense fallback={<Fragment />}> */}
                    <Routes />
                {/* </Suspense> */}
            </MuiThemeProvider>
        </BrowserRouter>
    )
}

export { App }
