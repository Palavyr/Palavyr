import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { CollectDetailsForm } from "@widgetcore/UserDetailsDialog/CollectDetailsForm";
import { Widget } from "@widgetcore/widget/Widget";
import { useAppContext } from "./hook";
import { Dialog } from "@material-ui/core";
import { InitializeFonts } from "@frontend/dashboard/content/designer/fonts/Initializer";

export const WidgetApp = () => {
    const [convoId, setConvoId] = useState<string | null>(null);
    const [isReady, setIsReady] = useState<boolean | null>(null);
    const [preferences, setWidgetPrefs] = useState<WidgetPreferencesResource>();
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo") ?? false;

    const context = useAppContext();

    const client = new PalavyrWidgetRepository(secretKey);

    const runAppPrecheck = useCallback(async () => {
        const preCheckResult = await client.Widget.Get.PreCheck(isDemo === "true" ? true : false);

        if (preCheckResult.isReady) {
            const prefs = await client.Widget.Get.WidgetPreferences();
            setWidgetPrefs(prefs);
        }
        setTimeout(() => {
            setIsReady(preCheckResult.isReady);
        }, 2000);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        (async () => {
            const preCheckResult = await client.Widget.Get.PreCheck(isDemo === "true" ? true : false);

            if (preCheckResult.isReady) {
                const prefs = await client.Widget.Get.WidgetPreferences();
                setIsReady(preCheckResult.isReady);

                InitializeFonts(prefs);
                setWidgetPrefs(prefs);
            }
        })();
    }, [runAppPrecheck]);

    return (
        <div style={{ height: "100%", width: "100%" }}>
            {preferences && isDemo !== undefined && (
                <WidgetContext.Provider value={{ isDemo: isDemo === "true", context, preferences, setConvoId, convoId }}>
                    {isReady ? (
                        <>
                            <AreYouSureYouWantToGoBack />

                            <CollectDetailsForm setKickoff={() => null} />

                            <Widget />
                        </>
                    ) : (
                        <NotReady />
                    )}
                </WidgetContext.Provider>
            )}
        </div>
    );
};

export const NotReady = () => {
    return (
        <div style={{ textAlign: "center", paddingTop: "3rem" }}>
            <span id="palavyr-widget-not-ready">Not ready</span>
        </div>
    );
};

export const AreYouSureYouWantToGoBack = () => {
    const [open, setOpen] = useState(false);
    return (
        <Dialog open={open}>
            <div style={{ textAlign: "center", paddingTop: "3rem" }}>
                <span id="palavyr-widget-not-ready">Are you sure you want to go back?</span>
            </div>
        </Dialog>
    );
};
