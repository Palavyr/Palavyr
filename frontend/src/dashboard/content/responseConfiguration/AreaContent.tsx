import { PanelRange, areaTabProps, TabPanel } from "@common/ContentUtils";
import React, { useState, useEffect } from "react";
import { ConvoTree } from "./conversation/ConvoTree";
import { ResponseConfiguration } from "./response/ResponseConfiguration";
import { EmailConfiguration } from "./uploadable/emailTemplates/EmailConfiguration";
import { AttachmentConfiguration } from "./uploadable/attachments/AttachmentConfiguration";
import { ConfigurationPreview } from "./previews/ConfigurationPreview";
import { AreaSettings } from "./areaSettings/AreaSettings";
import { AppBar, Tabs, Tab, makeStyles } from "@material-ui/core";
import { PleaseConfirmYourEmail } from "../welcome/PleaseConfirmYourEmail";
import { WelcomeToTheDashboard } from "../welcome/WelcomeToTheDashboard";
import { useLocation } from "react-router-dom";
import AccountTreeIcon from '@material-ui/icons/AccountTree';
import FilterFramesIcon from '@material-ui/icons/FilterFrames';
import SubjectIcon from '@material-ui/icons/Subject';
import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf';
import VisibilityIcon from '@material-ui/icons/Visibility';
import SettingsApplicationsIcon from '@material-ui/icons/SettingsApplications';


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
}

const useTabsStyles = makeStyles(theme => ({
    root: {
        width: "100%",
        position: "absolute",
        flexGrow: 1,
      },
      appbar: {
          width: "100%",
          top: theme.mixins.toolbar.minHeight,
          height: "72px"
      }
}))


export const AreaContent = ({ active, areaIdentifier, areaName, setLoaded, setViewName}: IAreaContent) => {

    const [tab, setTab] = useState<PanelRange>(0);
    const location = useLocation();
    const classes = useTabsStyles();

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
            <div className={classes.root}>
                <AppBar position="static" className={classes.appbar}>
                    <Tabs centered value={tab} onChange={handleTabChange} aria-label="simple tabs example">
                        <Tab icon={<AccountTreeIcon />} label="Conversation" {...areaTabProps(0)} />
                        <Tab icon={<FilterFramesIcon />} label="Estimate" {...areaTabProps(1)} />
                        <Tab icon={<SubjectIcon />} label="Email" {...areaTabProps(2)} />
                        <Tab icon={<PictureAsPdfIcon />} label="Attachments" {...areaTabProps(3)} />
                        <Tab icon={<VisibilityIcon />} label="Preview" {...areaTabProps(4)} />
                        <Tab icon={<SettingsApplicationsIcon />} label="Settings" {...areaTabProps(5)} />
                    </Tabs>
                </AppBar>
                <TabPanels tab={tab} areaName={areaName} areaIdentifier={areaIdentifier} setViewName={setViewName} />
            </div>
        )
    }
    return active ?
        (
            (location.pathname === "/dashboard" || location.pathname === "/dashboard/editor") ? <WelcomeToTheDashboard /> : <EditorInterface />
        )
        :
        (
            (active === null) ? <div>Loading...</div> : <PleaseConfirmYourEmail />
        )
};


