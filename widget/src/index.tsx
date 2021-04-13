import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import theme from "./theme";
import { Provider } from "react-redux";
import { App } from "./App";
import store from "store/store";

ReactDOM.render(
    <React.StrictMode>
        <Provider store={store}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline />
                <Router>
                    <Route exact path="/widget" component={App} />
                </Router>
            </MuiThemeProvider>
        </Provider>
    </React.StrictMode>,
    document.getElementById("root")
);
