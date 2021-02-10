import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { OptionSelector } from "./options/Options";
import { SelectedOption, UserDetails, WidgetPreferences } from "./types";
import { useLocation } from "react-router-dom";
import CreateClient, { IClient } from "./client/Client";
import { CollectDetailsForm } from "./common/UserDetailsDialog/CollectDetailsForm";
import { CustomWidget } from "./widget/CustomWidget";

export const App = () => {
    const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);
    const [widgetPrefs, setWidgetPrefs] = useState<WidgetPreferences>();

    const [userDetails, setUserDetails] = useState<UserDetails>({
        userEmail: "",
        userPhone: "",
        userName: "",
    });
    const [userDetailsDialogState, setUserDetailsDialogstate] = useState<boolean>(false);
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo");

    let client: IClient;
    if (secretKey) client = CreateClient(secretKey);

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
        <>
            {isReady === true && selectedOption === null && !userDetailsDialogState && <OptionSelector setUserDetailsDialogState={setUserDetailsDialogstate} setSelectedOption={setSelectedOption} preferences={widgetPrefs} />}
            {isReady === true && selectedOption !== null && userDetailsDialogState && (
                <CollectDetailsForm
                    detailsSet={detailsSet}
                    setDetailsSet={setDetailsSet}
                    userDetailsDialogState={userDetailsDialogState}
                    setUserDetailsDialogState={setUserDetailsDialogstate}
                    userDetails={userDetails}
                    setUserDetails={setUserDetails}
                />
            )}
            {isReady === true && selectedOption !== null && !userDetailsDialogState && <CustomWidget setUserDetailsDialogState={setUserDetailsDialogstate} userDetails={userDetails} option={selectedOption} preferences={widgetPrefs} />}

            {isReady === false && (
                <div style={{ textAlign: "center", paddingTop: "3rem" }}>
                    <span>Not ready</span>
                </div>
            )}
        </>
    );
};
