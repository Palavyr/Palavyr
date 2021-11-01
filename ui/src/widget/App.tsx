import * as React from "react";
import { useState, useCallback, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { WidgetPreferences } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { WidgetContext } from "widget/context/WidgetContext";
import { CollectDetailsForm } from "common/UserDetailsDialog/CollectDetailsForm";
import { SmoothWidget } from "widget/widgets/SmoothWidget";
import { toggleUserDetails } from "@store-dispatcher";
import { BookLoaderComponent, BoxesLoaderComponent } from "widget/components/Loaders/BoxLoader";
import { SpaceEvenly } from "widget/components/Footer/SpaceEvenly";
import { Fade } from "@material-ui/core";

export const App = () => {
    // const [kickoff, setKickoff] = useState<boolean>(false);
    const [chatStarted, setChatStarted] = useState<boolean>(false);
    // const userDetailsVisible = useSelector((state: GlobalState) => state.behaviorReducer.userDetailsVisible);
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
            // toggleUserDetails();
            // setWidgetPreferences(prefs);
        }
        setTimeout(() => {
            setIsReady(preCheckResult.isReady);
        }, 5000);
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
        <>
            {preferences && (
                <WidgetContext.Provider value={{ preferences, chatStarted, setChatStarted, setConvoId, convoId }}>
                    {isReady ? (
                        <>
                            <CollectDetailsForm setKickoff={() => null} />
                            <SmoothWidget />
                        </>
                    ) : (
                        <NotReady />
                    )}
                </WidgetContext.Provider>
            )}
            {/* {isReady === null && (
                <SpaceEvenly vertical>
                    <div style={{ height: "100%", width: "100%", display: "flex", justifyContent: "center", justifyItems: "center" }}>
                        <BookLoaderComponent />
                    </div>
                </SpaceEvenly>
            )} */}
        </>

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
