import React, { useState, useCallback, useEffect } from 'react';
import 'react-chat-widget/lib/styles.css';
import { CustomWidget } from './widget/CustomWidget';
import { OptionSelector } from './options/Options';
import { SelectedOption, AreaTable, WidgetPreferences } from './types';
import { useLocation } from 'react-router-dom';
import CreateClient from './client/Client';


type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>
}

export const App = () => {

    const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);
    const [widgetPrefs, setWidgetPrefs] = useState<WidgetPreferences>()

    var secretKey = (new URLSearchParams(useLocation().search)).get("key")

    const client = CreateClient(secretKey);
    console.log("SECRET: " + secretKey)

    const runAppPrecheck = useCallback(async () => {

        console.log(client)

        var preCheckResult = (await client.Widget.Access.runPreCheck()).data as PreCheckResult;
        setIsReady(preCheckResult.isReady);
        if(preCheckResult.isReady) {
            var prefs = (await client.Widget.Access.fetchPreferences()).data as WidgetPreferences;
            setWidgetPrefs(prefs);
        }
    }, [])

    useEffect(() => {
        runAppPrecheck();
    }, [runAppPrecheck])

    return (
        <>
            {(isReady === true) && (selectedOption === null) && <OptionSelector setSelectedOption={setSelectedOption} preferences={widgetPrefs} />}
            {(isReady === true) && (selectedOption !== null) && <CustomWidget option={selectedOption} preferences={widgetPrefs} />}
            {(isReady === false) && <span style={{textAlign: "center", margin: "2rem"}}>Not ready</span>}
        </>
    )
}
