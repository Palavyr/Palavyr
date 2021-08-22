import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { OptionSelector } from "./options/Options";
import { useLocation } from "react-router-dom";
import { Widget } from "./widget/Widget";
import { CollectDetailsForm } from "./common/UserDetailsDialog/CollectDetailsForm";
import { useSelector } from "react-redux";
import { GlobalState, SelectedOption, WidgetPreferences } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { setWidgetPreferences } from "@store-dispatcher";
import { WidgetContext } from "widget/context/WidgetContext";

export const App = () => {
    const userDetailsVisible = useSelector((state: GlobalState) => state.behaviorReducer.userDetailsVisible);

    const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);
    const [preferences, setWidgetPrefs] = useState<WidgetPreferences>();
    const [kickoff, setKickoff] = useState<boolean>(false);

    const [chatStarted, setChatStarted] = useState<boolean>(false);

    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo");

    const client = new PalavyrWidgetRepository(secretKey);

    const runAppPrecheck = useCallback(async () => {
        const preCheckResult = await client.Widget.Get.PreCheck(isDemo === "true" ? true : false);
        setIsReady(preCheckResult.isReady);
        if (preCheckResult.isReady) {
            const prefs = await client.Widget.Get.WidgetPreferences();
            setWidgetPrefs(prefs);
            setWidgetPreferences(prefs);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        runAppPrecheck();
    }, [runAppPrecheck]);

    return (
        <>
            {preferences && (
                <WidgetContext.Provider value={{ preferences }}>
                    {isReady === true && selectedOption === null && preferences && !userDetailsVisible && (
                        <>
                            <OptionSelector setSelectedOption={setSelectedOption} />
                        </>
                    )}
                    {isReady === true && selectedOption !== null && (
                        <>
                            <CollectDetailsForm chatStarted={chatStarted} setChatStarted={setChatStarted} setKickoff={setKickoff} />
                            {preferences && kickoff && <Widget option={selectedOption} />}
                        </>
                    )}
                    {isReady === false && (
                        <div style={{ textAlign: "center", paddingTop: "3rem" }}>
                            <span>Not ready</span>
                        </div>
                    )}
                </WidgetContext.Provider>
            )}
        </>
    );
};
