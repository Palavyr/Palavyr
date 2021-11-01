import React, { useEffect } from "react";
import { MemoryRouter } from "react-router";
import { Provider } from "react-redux";
import { PalavyrWidgetStore } from "store/store";
import { widgetUrl } from "test/routes";
import { CssBaseline, MuiThemeProvider } from "@material-ui/core";
import { PalavyrWidgetTheme } from "../src/PalavyrWidgetTheme";
import { dropMessages } from "../src/store/dispatcher";

const fakeKey = "secret-key";
const isDemo = false;
const homeUrl = widgetUrl(fakeKey, isDemo);

export const parameters = {
    actions: { argTypesRegex: "^on[A-Z].*" },
    controls: {
        matchers: {
            color: /(background|color)$/i,
            date: /Date$/,
        },
    },
};

export const decorators = [
    Story => {
        useEffect(() => {
            return () => {
                dropMessages();
            };
        }, [dropMessages]);
        return (
            <MemoryRouter initialEntries={[homeUrl]}>
                <Provider store={PalavyrWidgetStore}>
                    <MuiThemeProvider theme={PalavyrWidgetTheme}>
                        <CssBaseline />
                        <Story />
                    </MuiThemeProvider>
                </Provider>
            </MemoryRouter>
        );
    },
];
