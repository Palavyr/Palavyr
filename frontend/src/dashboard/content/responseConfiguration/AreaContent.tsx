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
import { Redirect, useHistory, useLocation, useParams } from "react-router-dom";
import AccountTreeIcon from "@material-ui/icons/AccountTree";
import FilterFramesIcon from "@material-ui/icons/FilterFrames";
import SubjectIcon from "@material-ui/icons/Subject";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";
import VisibilityIcon from "@material-ui/icons/Visibility";
import SettingsApplicationsIcon from "@material-ui/icons/SettingsApplications";
import { IGetHelp } from "@Palavyr-Types";
import { AuthContext, DashboardContext } from "dashboard/layouts/DashboardContext";

interface ITabs {
    tab: PanelRange;
    areaName: string;
    areaIdentifier: string;
}

const TabPanels = ({ tab, areaName, areaIdentifier }: ITabs) => {
    return (
        <>
            <TabPanel value={tab} index={0}>
                <EmailConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={1}>
                <ResponseConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={2}>
                <AttachmentConfiguration areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={3}>
                <ConvoTree areaIdentifier={areaIdentifier} treeName={areaName} />
            </TabPanel>

            <TabPanel value={tab} index={4}>
                <AreaSettings areaIdentifier={areaIdentifier} />
            </TabPanel>

            <TabPanel value={tab} index={5}>
                <ConfigurationPreview areaIdentifier={areaIdentifier} />
            </TabPanel>
        </>
    );
};

export interface IAreaContent {
    setLoaded: any;
}

const useTabsStyles = makeStyles((theme) => ({
    root: {
        width: "100%",
        position: "absolute",
        flexGrow: 1,
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
        height: "100%",
    },
    appbar: {
        background: "#c7ecee",
        width: "100%",
        top: theme.mixins.toolbar.minHeight,
        height: "72px",
    },
    icon: {
        color: "navy",
    },
    tabtext: {
        color: "navy",
    },
}));

export const AreaContent = () => {
    const [, setLoaded] = useState<boolean>(false);
    return <AreaContentInner setLoaded={setLoaded} />;
};

export const AreaContentInner = ({ setLoaded }: IAreaContent) => {
    const [tab, setTab] = useState<PanelRange>(0);
    const history = useHistory();
    const classes = useTabsStyles();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { areaName } = React.useContext(DashboardContext);
    const { isActive } = React.useContext(AuthContext);
    // dashboard/editor/response/
    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false);
            // setTab()
        };
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    if (!isActive) {
        history.push("/dashboard/confirm");
    }

    return (
        <div className={classes.root}>
            <AppBar position="static" className={classes.appbar}>
                <Tabs centered value={tab} onChange={(event: any, newValue: PanelRange) => setTab(newValue)} aria-label="simple tabs example">
                    <Tab className={classes.tabtext} icon={<SubjectIcon className={classes.icon} />} label="1. Email" {...areaTabProps(0)} />
                    <Tab className={classes.tabtext} icon={<FilterFramesIcon className={classes.icon} />} label="2. Response" {...areaTabProps(1)} />
                    <Tab className={classes.tabtext} icon={<PictureAsPdfIcon className={classes.icon} />} label="3. Attachments" {...areaTabProps(2)} />
                    <Tab className={classes.tabtext} icon={<AccountTreeIcon className={classes.icon} />} label="4. Conversation" {...areaTabProps(3)} />
                    <Tab className={classes.tabtext} icon={<SettingsApplicationsIcon className={classes.icon} />} label="5. Settings" {...areaTabProps(4)} />
                    <Tab className={classes.tabtext} icon={<VisibilityIcon className={classes.icon} />} label="6. Preview" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>
            <TabPanels tab={tab} areaName={areaName ?? ""} areaIdentifier={areaIdentifier} />
        </div>
    );
};
