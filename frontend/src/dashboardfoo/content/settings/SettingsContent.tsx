import React, { useState, useEffect } from "react";
import AppBar from "@material-ui/core/AppBar";
import Tab from "@material-ui/core/Tab";
import Tabs from "@material-ui/core/Tabs";
import { ChangePassword } from "./security/changePassword";
import { ChangeEmail } from "./account/ChangeEmail";
import { ChangeUserName } from "./account/ChangeUserName";
import { ChangeCompanyName } from "./account/ChangeCompanyName";
import { ChangePhoneNumber } from "./account/ChangePhoneNumber";
import { ChangeLogoImage } from "./account/ChangeLogoImage";
import { areaTabProps, TabPanel, PanelRange } from "@common/ContentUtils";

interface IAreaContent {
    areaName: string;
    areaIdentifier: string;
    classes: any;
    setLoaded: any;
}



export const SettingsContent = ({ areaIdentifier, areaName, classes, setLoaded }: IAreaContent) => {
    const [tab, setTab] = useState<PanelRange>(0); // tabs

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false)
        }
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    const handleTabChange = (event: any, newValue: PanelRange) => {
        setTab(newValue);
    };

    return (
        <div className={classes.contentRoot}>
            <AppBar position="static">
                <Tabs value={tab} onChange={handleTabChange} aria-label="simple tabs example">
                    <Tab label="Password" {...areaTabProps(0)} />
                    <Tab label="Email" {...areaTabProps(1)} />
                    <Tab label="UserName" {...areaTabProps(2)} />
                    <Tab label="Company Name" {...areaTabProps(3)} />
                    <Tab label="Phone Number" {...areaTabProps(4)} />
                    <Tab label="Response Logo" {...areaTabProps(5)} />

                </Tabs>
            </AppBar>

            <TabPanel value={tab} index={0}>
                <ChangePassword />
            </TabPanel>

            <TabPanel value={tab} index={1}>
                <ChangeEmail />
            </TabPanel>
            <TabPanel value={tab} index={2}>
                <ChangeUserName />
            </TabPanel>
            <TabPanel value={tab} index={3}>
                <ChangeCompanyName />
            </TabPanel>
            <TabPanel value={tab} index={4}>
                <ChangePhoneNumber />
            </TabPanel>
            <TabPanel value={tab} index={5}>
                <ChangeLogoImage />
            </TabPanel>

            {/* <TabPanel value={tab} index={5}>
                <GroupTree
                    areaIdentifier={areaIdentifier}
                    treeName={areaName}
                    NodeInterface={NodeInterface}
                />
            </TabPanel> */}
            {/* <TabPanel value={tab} index={2}>
                ToDo: Response configuration, such as the email address that shows up when sending a response.
                <EmailSubject />
            </TabPanel> */}
        </div >
    )
};
