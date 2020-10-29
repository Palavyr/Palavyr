import React, { useState, useEffect } from "react";
import AppBar from "@material-ui/core/AppBar";
import Tab from "@material-ui/core/Tab";
import Tabs from "@material-ui/core/Tabs";
import { ChangePassword } from "./security/changePassword";
import { ChangeEmail } from "./account/ChangeEmail";
import { ChangeCompanyName } from "./account/ChangeCompanyName";
import { ChangePhoneNumber } from "./account/ChangePhoneNumber";
import { ChangeLogoImage } from "./account/ChangeLogoImage";
import { areaTabProps, TabPanel, PanelRange } from "@common/ContentUtils";
import { ChangeLocale } from "./account/ChangeLocale";
import { makeStyles } from "@material-ui/core";
import LockOpenIcon from '@material-ui/icons/LockOpen';
import MailOutlineIcon from '@material-ui/icons/MailOutline';
import PermIdentityIcon from '@material-ui/icons/PermIdentity';
import PhoneIcon from '@material-ui/icons/Phone';
import BrandingWatermarkIcon from '@material-ui/icons/BrandingWatermark';
import PublicIcon from '@material-ui/icons/Public';
import { HelpTypes } from "dashboard/layouts/DashboardLayout";



interface IAreaContent {
    areaName: string;
    areaIdentifier: string;
    setLoaded: any;
    setHelpType(helpType: HelpTypes): void;
}

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
}));

export const SettingsContent = ({ areaIdentifier, areaName, setLoaded, setHelpType }: IAreaContent) => {

    const [tab, setTab] = useState<PanelRange>(2); // tabs
    const classes = useStyles();

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false)
        }
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    const handleTabChange = (event: any, newValue: PanelRange) => {
        switch (newValue){
            case 0:
                setHelpType("password");
                break;
            case 1:
                setHelpType("email");
                break;
            case 2:
                setHelpType("companyname");
                break;
            case 3:
                setHelpType("phonenumber");
                break;
            case 4:
                // setHelpType("responselogo");
                break;
            case 5:
                setHelpType("locale");
            default:
                break;
        }
        setTab(newValue);
    };

    return (
        <div className={classes.root}>
            <AppBar position="static">
                <Tabs centered value={tab} onChange={handleTabChange} aria-label="simple tabs">
                    <Tab icon={<LockOpenIcon />} label="Password" {...areaTabProps(0)} />
                    <Tab icon={<MailOutlineIcon />} label="Email" {...areaTabProps(1)} />
                    <Tab icon={<PermIdentityIcon />} label="Company Name" {...areaTabProps(2)} />
                    <Tab icon={<PhoneIcon />} label="Phone Number" {...areaTabProps(3)} />
                    <Tab icon={<BrandingWatermarkIcon />} label="Response Logo" {...areaTabProps(4)} />
                    <Tab icon={<PublicIcon />} label="Locale" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>

            <TabPanel value={tab} index={0}>
                <ChangePassword />
            </TabPanel>
            <TabPanel value={tab} index={1}>
                <ChangeEmail />
            </TabPanel>
            <TabPanel value={tab} index={2}>
                <ChangeCompanyName />
            </TabPanel>
            <TabPanel value={tab} index={3}>
                <ChangePhoneNumber />
            </TabPanel>
            <TabPanel value={tab} index={4}>
                <ChangeLogoImage />
            </TabPanel>
            <TabPanel value={tab} index={5}>
                <ChangeLocale />
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
