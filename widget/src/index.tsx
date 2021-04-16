import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "./PalavyrWidgetTheme";
import { Provider } from "react-redux";
import { App } from "./App";
import { PalavyrWidgetStore } from "store/store";
import { TestComponent } from "test/testComponent";

ReactDOM.render(
    <React.StrictMode>
        <Provider store={PalavyrWidgetStore}>
            <MuiThemeProvider theme={PalavyrWidgetTheme}>
                <CssBaseline />
                <Router>
                    <Route exact path="/widget" component={App} />
                    <Route exact path="/test" component={TestComponent} />
                </Router>
            </MuiThemeProvider>
        </Provider>
    </React.StrictMode>,
    document.getElementById("root")
);
