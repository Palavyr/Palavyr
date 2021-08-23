import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { WidgetPreferences } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { WidgetContext } from "widget/context/WidgetContext";
import { SmoothWidget } from "widget/smoothWidget/SmoothWidget";

export const App = () => {
    // const [kickoff, setKickoff] = useState<boolean>(false);
    // const [chatStarted, setChatStarted] = useState<boolean>(false);
    // const userDetailsVisible = useSelector((state: GlobalState) => state.behaviorReducer.userDetailsVisible);

    const [isReady, setIsReady] = useState<boolean>(false);
    const [preferences, setWidgetPrefs] = useState<WidgetPreferences>();

    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const isDemo = new URLSearchParams(useLocation().search).get("demo");

    const Client = new PalavyrWidgetRepository(secretKey);

    const runAppPrecheck = useCallback(async () => {
        const preCheckResult = await Client.Widget.Get.PreCheck(isDemo === "true" ? true : false);
        setIsReady(preCheckResult.isReady);

        if (preCheckResult.isReady) {
            const prefs = await Client.Widget.Get.WidgetPreferences();
            setWidgetPrefs(prefs);
            // toggleUserDetails();
            // setWidgetPreferences(prefs);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    // const [options, setOptions] = useState<Array<SelectedOption>>();
    // const [, setUseGroups] = useState<boolean>();

    // const loadAreas = useCallback(async () => {
    //     setUseGroups(false);

    //     var areas = await Client.Widget.Get.Areas();
    //     var options = areas.map((area: AreaTable) => {
    //         return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier };
    //     });

    //     setOptions(options);
    // }, []);

    useEffect(() => {
        // runAppPrecheck();
        // loadAreas();

        (async () => {
            const preCheckResult = await Client.Widget.Get.PreCheck(isDemo === "true" ? true : false);

            if (preCheckResult.isReady) {
                const prefs = await Client.Widget.Get.WidgetPreferences();
                setIsReady(preCheckResult.isReady);
                setWidgetPrefs(prefs);
                // toggleUserDetails();
                // setWidgetPreferences(prefs);
            }
        })();
    }, [runAppPrecheck]);

    // const history = useHistory();
    // const onChange = (event: any, newOption: SelectedOption) => {
    //     setSelectedOption(newOption);
    //     history.push(`/widget?key=${secretKey}`);
    //     openUserDetails();
    // };

    return (
        <>{preferences ? <WidgetContext.Provider value={{ preferences }}>{isReady ? <SmoothWidget /> : <NotReady />}</WidgetContext.Provider> : <NotReady />}</>

        // <>
        //     {preferences && (
        //         <WidgetContext.Provider value={{ preferences }}>
        //             {isReady === true && selectedOption === null && preferences && !userDetailsVisible && <>{options && <OptionSelector onChange={onChange} options={options} setSelectedOption={setSelectedOption} />}</>}
        //             {isReady === true && selectedOption !== null && (
        //                 <>
        //                     <CollectDetailsForm chatStarted={chatStarted} setChatStarted={setChatStarted} setKickoff={setKickoff} />
        //                     {preferences && kickoff && <Widget option={selectedOption} />}
        //                 </>
        //             )}
        //             {isReady === false && (
        //                 <div style={{ textAlign: "center", paddingTop: "3rem" }}>
        //                     <span>Not ready</span>
        //                 </div>
        //             )}
        //         </WidgetContext.Provider>
        //     )}
        // </>
    );
};

export const NotReady = () => {
    return (
        <div style={{ textAlign: "center", paddingTop: "3rem" }}>
            <span>Not ready</span>
        </div>
    );
};
