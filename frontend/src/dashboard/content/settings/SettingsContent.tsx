import React, { useState, useEffect } from "react";
import AppBar from "@material-ui/core/AppBar";
import Tab from "@material-ui/core/Tab";
import Tabs from "@material-ui/core/Tabs";
import { areaTabProps, PanelRange } from "@common/ContentUtils";
import { makeStyles } from "@material-ui/core";
import LockOpenIcon from '@material-ui/icons/LockOpen';
import MailOutlineIcon from '@material-ui/icons/MailOutline';
import PermIdentityIcon from '@material-ui/icons/PermIdentity';
import PhoneIcon from '@material-ui/icons/Phone';
import BrandingWatermarkIcon from '@material-ui/icons/BrandingWatermark';
import PublicIcon from '@material-ui/icons/Public';
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import { useHistory } from "react-router-dom";
import { GeneralSettingsLoc } from "@Palavyr-Types";

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
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

interface ISettingsContent {
    children: JSX.Element[] | JSX.Element;
}

interface ISettingsContentInner extends ISettingsContent {
    setLoaded(val: boolean): void;
}

export const SettingsContent = ({children}: ISettingsContent ) => {
    const [, setLoaded] = useState<boolean>(false);
    return <SettingsContentInner setLoaded={setLoaded} children={children} />
}


const SettingsContentInner = ({ setLoaded, children}: ISettingsContentInner ) => {

    const classes = useStyles();
    const history = useHistory();

    const searchParams = new URLSearchParams(location.search);
    const rawTab = searchParams.get("tab");
    const tab = rawTab ? (parseInt(rawTab) as PanelRange) : 0;

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false)
        }
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    const sendTo = (dest: GeneralSettingsLoc) => {
        history.push(`/dashboard/settings/${GeneralSettingsLoc[dest]}?tab=${dest}`)
    }

    return (
        <div className={classes.root}>
            <AppBar position="static" className={classes.appbar}>
                <Tabs centered value={tab}  aria-label="simple tabs">
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.password)} className={classes.tabtext} icon={<LockOpenIcon className={classes.icon} />} label="Password" {...areaTabProps(0)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.email)} className={classes.tabtext} icon={<MailOutlineIcon className={classes.icon} />} label="Email" {...areaTabProps(1)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyName)} className={classes.tabtext} icon={<PermIdentityIcon className={classes.icon} />} label="Company Name" {...areaTabProps(2)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.phoneNumber)} className={classes.tabtext} icon={<PhoneIcon className={classes.icon} />} label="Phone Number" {...areaTabProps(3)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyLogo)} className={classes.tabtext} icon={<BrandingWatermarkIcon className={classes.icon} />} label="Company Logo" {...areaTabProps(4)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.locale)} className={classes.tabtext} icon={<PublicIcon className={classes.icon} />} label="Locale" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>
            <AlignCenter>
                {children}
            </AlignCenter>
        </div >
    )
};


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


