import { PanelRange, areaTabProps, TabPanel } from "@common/ContentUtils";
import React, { useState, useEffect } from "react";
import { ConvoTree } from "./conversation/ConvoTree";
import { ResponseConfiguration } from "./response/ResponseConfiguration";
import { EmailConfiguration } from "./uploadable/emailTemplates/EmailConfiguration";
import { AttachmentConfiguration } from "./uploadable/attachments/AttachmentConfiguration";
import { ConfigurationPreview } from "./previews/ConfigurationPreview";
import { AreaSettings } from "./areaSettings/AreaSettings";
import { AppBar, Tabs, Tab } from "@material-ui/core";
import { PleaseConfirmYourEmail } from "../welcome/PleaseConfirmYourEmail";
import { WelcomeToTheDashboard } from "../welcome/WelcomeToTheDashboard";
import { useLocation } from "react-router-dom";

interface ITabs {
    tab: PanelRange;
    areaName: string;
    areaIdentifier: string;
    setViewName: any
}

const TabPanels = ({ tab, areaName, areaIdentifier, setViewName }: ITabs) => {

    return (
        <>
            <TabPanel value={tab} index={0}>
                <ConvoTree
                    areaIdentifier={areaIdentifier}
                    treeName={areaName}
                />
            </TabPanel>

            <TabPanel value={tab} index={1}>
                <ResponseConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={2}>
                <EmailConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={3}>
                <AttachmentConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={4}>
                <ConfigurationPreview areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={5}>
                <AreaSettings areaIdentifier={areaIdentifier} areaName={areaName} setViewName={setViewName} />
            </TabPanel>

        </>
    )
}

export interface IAreaContent {
    active: boolean | null;
    areaName: string;
    areaIdentifier: string;
    setLoaded: any;
    setViewName: any;
    classes: any;
}


export const AreaContent = ({ active, areaIdentifier, areaName, setLoaded, setViewName, classes}: IAreaContent) => {
    const [tab, setTab] = useState<PanelRange>(0);
    const location = useLocation();

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false)
        }
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    const handleTabChange = (event: any, newValue: PanelRange) => {
        setTab(newValue);
    };

    const EditorInterface = () => {
        return (
            <div className={classes.contentRoot}>
                <AppBar position="static">
                    <Tabs value={tab} onChange={handleTabChange} aria-label="simple tabs example">
                        <Tab label="Conversation Builder" {...areaTabProps(0)} />
                        <Tab label="Estimate Configuration" {...areaTabProps(1)} />
                        <Tab label="Email Configuration" {...areaTabProps(2)} />
                        <Tab label="PDF Attachments" {...areaTabProps(3)} />
                        <Tab label="Estimate Preview" {...areaTabProps(4)} />
                        <Tab label="Settings" {...areaTabProps(5)} />
                    </Tabs>
                </AppBar>
                <TabPanels tab={tab} areaName={areaName} areaIdentifier={areaIdentifier} setViewName={setViewName} />
            </div>
        )
    }
    console.log(location.pathname);
    return active ?
        (
            (location.pathname === "/dashboard" || location.pathname === "/dashboard/editor") ? <WelcomeToTheDashboard /> : <EditorInterface />
        )
        :
        (
            (active === null) ? <div>Loading...</div> : <PleaseConfirmYourEmail />
        )
};


