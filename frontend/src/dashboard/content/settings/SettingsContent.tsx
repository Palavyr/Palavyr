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
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        // backgroundColor: theme.palette.background.paper,
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

export const SettingsContent = () => {
    const [, setLoaded] = useState<boolean>(false);
    return <SettingsContentInner setLoaded={setLoaded} />
}

interface ISetLoaded {
    setLoaded(val: boolean): void;
}

const SettingsContentInner = ({ setLoaded }: ISetLoaded ) => {

    const [tab, setTab] = useState<PanelRange>(0); // tabs
    const classes = useStyles();

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
        <div className={classes.root}>
            <AppBar position="static" className={classes.appbar}>
                <Tabs centered value={tab} onChange={handleTabChange} aria-label="simple tabs">
                    <Tab className={classes.tabtext} icon={<LockOpenIcon className={classes.icon} />} label="Password" {...areaTabProps(0)} />
                    <Tab className={classes.tabtext} icon={<MailOutlineIcon className={classes.icon} />} label="Email" {...areaTabProps(1)} />
                    <Tab className={classes.tabtext} icon={<PermIdentityIcon className={classes.icon} />} label="Company Name" {...areaTabProps(2)} />
                    <Tab className={classes.tabtext} icon={<PhoneIcon className={classes.icon} />} label="Phone Number" {...areaTabProps(3)} />
                    <Tab className={classes.tabtext} icon={<BrandingWatermarkIcon className={classes.icon} />} label="Response Logo" {...areaTabProps(4)} />
                    <Tab className={classes.tabtext} icon={<PublicIcon className={classes.icon} />} label="Locale" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>

            <TabPanel value={tab} index={0}>
                <AlignCenter>
                    <ChangePassword />
                </AlignCenter>
            </TabPanel>
            <TabPanel value={tab} index={1}>
                <AlignCenter>
                    <ChangeEmail />
                </AlignCenter>
            </TabPanel>
            <TabPanel value={tab} index={2}>
                <AlignCenter>
                    <ChangeCompanyName />
                </AlignCenter>
            </TabPanel>
            <TabPanel value={tab} index={3}>
                <AlignCenter>
                    <ChangePhoneNumber />
                </AlignCenter>
            </TabPanel>
            <TabPanel value={tab} index={4}>
                <AlignCenter>
                    <ChangeLogoImage />
                </AlignCenter>
            </TabPanel>
            <TabPanel value={tab} index={5}>
                <AlignCenter>
                    <ChangeLocale />
                </AlignCenter>
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
