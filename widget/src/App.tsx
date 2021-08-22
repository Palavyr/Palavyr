import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { OptionSelector } from "./options/Options";
import { useLocation } from "react-router-dom";
import { Widget } from "./widget/Widget";
import { CollectDetailsForm } from "./common/UserDetailsDialog/CollectDetailsForm";
import { useSelector } from "react-redux";
import { AreaTable, GlobalState, SelectedOption, WidgetPreferences } from "@Palavyr-Types";
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

    const Client = new PalavyrWidgetRepository(secretKey);

    const runAppPrecheck = useCallback(async () => {
        const preCheckResult = await Client.Widget.Get.PreCheck(isDemo === "true" ? true : false);
        setIsReady(preCheckResult.isReady);
        if (preCheckResult.isReady) {
            const prefs = await Client.Widget.Get.WidgetPreferences();
            setWidgetPrefs(prefs);
            setWidgetPreferences(prefs);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const [options, setOptions] = useState<Array<SelectedOption>>();
    const [, setUseGroups] = useState<boolean>();

    const loadAreas = useCallback(async () => {
        setUseGroups(false);

        var areas = await Client.Widget.Get.Areas();
        var options = areas.map((area: AreaTable) => {
            return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier };
        });

        setOptions(options);
    }, []);

    useEffect(() => {
        runAppPrecheck();
        loadAreas();
    }, [runAppPrecheck, loadAreas]);

    return (
        <>
            {preferences && (
                <WidgetContext.Provider value={{ preferences }}>
                    {isReady === true && selectedOption === null && preferences && !userDetailsVisible && <>{options && <OptionSelector options={options} setSelectedOption={setSelectedOption} />}</>}
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
