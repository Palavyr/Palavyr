import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { WidgetPreferences } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { CollectDetailsForm } from "@widgetcore/UserDetailsDialog/CollectDetailsForm";
import { Widget } from "@widgetcore/widgets/Widget";

export const WidgetApp = () => {
    const [chatStarted, setChatStarted] = useState<boolean>(false);
    const [convoId, setConvoId] = useState<string | null>(null);
    const [isReady, setIsReady] = useState<boolean | null>(null);
    const [preferences, setWidgetPrefs] = useState<WidgetPreferences>();

    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo");

    const Client = new PalavyrWidgetRepository(secretKey);

    const runAppPrecheck = useCallback(async () => {
        const preCheckResult = await Client.Widget.Get.PreCheck(isDemo === "true" ? true : false);

        if (preCheckResult.isReady) {
            const prefs = await Client.Widget.Get.WidgetPreferences();
            setWidgetPrefs(prefs);
        }
        setTimeout(() => {
            setIsReady(preCheckResult.isReady);
        }, 5000);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        (async () => {
            const preCheckResult = await Client.Widget.Get.PreCheck(isDemo === "true" ? true : false);

            if (preCheckResult.isReady) {
                const prefs = await Client.Widget.Get.WidgetPreferences();
                setIsReady(preCheckResult.isReady);
                setWidgetPrefs(prefs);
            }
        })();
    }, [runAppPrecheck]);

    return (
        <>
            {preferences && (
                <WidgetContext.Provider value={{ preferences, chatStarted, setChatStarted, setConvoId, convoId }}>
                    {isReady ? (
                        <>
                            <CollectDetailsForm setKickoff={() => null} />
                            <Widget />
                        </>
                    ) : (
                        <NotReady />
                    )}
                </WidgetContext.Provider>
            )}
        </>
    );
};

export const NotReady = () => {
    return (
        <div style={{ textAlign: "center", paddingTop: "3rem" }}>
            <span>Not ready</span>
        </div>
    );
};
