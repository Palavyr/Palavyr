import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { OptionSelector } from "./options/Options";
import { SelectedOption, WidgetPreferences } from "./types";
import { useLocation } from "react-router-dom";
import CreateClient, { IClient } from "./client/Client";
import { CustomWidget } from "./widget/CustomWidget";
import { CollectDetailsForm } from "./common/UserDetailsDialog/CollectDetailsForm";
import { Provider } from "react-redux";
import store from "./widgetCore/store";
import { ContextProperties } from "./widgetCore/store/types";


export const App = () => {
    const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);
    const [widgetPrefs, setWidgetPrefs] = useState<WidgetPreferences>();

    // const [contextProperties, setContextProperties] = useState<ContextProperties>(defaultContextProperties);
    const [userDetailsDialogState, setUserDetailsDialogstate] = useState<boolean>(false);
    const [chatStarted, setChatStarted] = useState<boolean>(false);

    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo");

    let client: IClient;
    if (secretKey) client = CreateClient(secretKey);

    // const getContextProperties = useCallback(() => {
    //     return [contextProperties, setContextProperties]
    // }, [contextProperties, setContextProperties])

    const runAppPrecheck = useCallback(async () => {
        var { data: preCheckResult } = await client.Widget.Access.runPreCheck(isDemo === "true" ? true : false);

        setIsReady(preCheckResult.isReady);
        if (preCheckResult.isReady) {
            const { data: prefs } = await client.Widget.Access.fetchPreferences();
            setWidgetPrefs(prefs);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        runAppPrecheck();
    }, [runAppPrecheck]);

    return (
        <Provider store={store}>
            {isReady === true && selectedOption === null && !userDetailsDialogState && <OptionSelector setUserDetailsDialogState={setUserDetailsDialogstate} setSelectedOption={setSelectedOption} preferences={widgetPrefs} />}
            {isReady === true && selectedOption !== null && (
                <>
                    <div style={{ display: userDetailsDialogState ? null : "none", zIndex: 9999 }}>
                        <CollectDetailsForm chatStarted={chatStarted} setChatStarted={setChatStarted}  userDetailsDialogState={userDetailsDialogState} setUserDetailsDialogState={setUserDetailsDialogstate} />
                    </div>

                    <div style={{ display: userDetailsDialogState ? "none" : null, zIndex: 9999 }}>
                        <CustomWidget setUserDetailsDialogState={setUserDetailsDialogstate} option={selectedOption} preferences={widgetPrefs} />
                    </div>
                </>
            )}
            {isReady === false && (
                <div style={{ textAlign: "center", paddingTop: "3rem" }}>
                    <span>Not ready</span>
                </div>
            )}
        </Provider>
    );
};
