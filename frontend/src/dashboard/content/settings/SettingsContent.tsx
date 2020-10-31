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
    appbar: {
        background: "#c7ecee",
        width: "100%",
        top: theme.mixins.toolbar.minHeight,
        height: "72px"
    },
    icon: {
        color: "navy"
    },
    tabtext: {
        color: "navy"
    }
}));

export const SettingsContent = ({ areaIdentifier, areaName, setLoaded, setHelpType }: IAreaContent) => {

    const [tab, setTab] = useState<PanelRange>(4); // tabs
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
            <AppBar position="static" className={classes.appbar}>
                <Tabs centered value={tab} onChange={handleTabChange} aria-label="simple tabs">
                    <Tab className={classes.tabtext} icon={<LockOpenIcon  className={classes.icon} />} label="Password" {...areaTabProps(0)} />
                    <Tab className={classes.tabtext} icon={<MailOutlineIcon  className={classes.icon} />} label="Email" {...areaTabProps(1)} />
                    <Tab className={classes.tabtext} icon={<PermIdentityIcon  className={classes.icon} />} label="Company Name" {...areaTabProps(2)} />
                    <Tab className={classes.tabtext} icon={<PhoneIcon  className={classes.icon} />} label="Phone Number" {...areaTabProps(3)} />
                    <Tab className={classes.tabtext} icon={<BrandingWatermarkIcon  className={classes.icon} />} label="Response Logo" {...areaTabProps(4)} />
                    <Tab className={classes.tabtext} icon={<PublicIcon  className={classes.icon} />} label="Locale" {...areaTabProps(5)} />
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
