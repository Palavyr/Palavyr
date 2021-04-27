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
import { Align } from "dashboard/layouts/positioning/AlignCenter";
import { useHistory } from "react-router-dom";
import { GeneralSettingsLoc } from "@Palavyr-Types";
import DeleteSweepIcon from '@material-ui/icons/DeleteSweep';
import SubjectIcon from '@material-ui/icons/Subject';

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

    const cls = useStyles();
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
        <div className={cls.root}>
            <AppBar position="static" className={cls.appbar}>
                <Tabs centered value={tab}  aria-label="simple tabs">
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.password)} className={cls.tabtext} icon={<LockOpenIcon className={cls.icon} />} label="Password" {...areaTabProps(0)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.email)} className={cls.tabtext} icon={<MailOutlineIcon className={cls.icon} />} label="Email Address" {...areaTabProps(1)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyName)} className={cls.tabtext} icon={<PermIdentityIcon className={cls.icon} />} label="Company Name" {...areaTabProps(2)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.phoneNumber)} className={cls.tabtext} icon={<PhoneIcon className={cls.icon} />} label="Phone Number" {...areaTabProps(3)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyLogo)} className={cls.tabtext} icon={<BrandingWatermarkIcon className={cls.icon} />} label="Company Logo" {...areaTabProps(4)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.locale)} className={cls.tabtext} icon={<PublicIcon className={cls.icon} />} label="Locale" {...areaTabProps(5)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.default_email_template)} className={cls.tabtext} icon={<SubjectIcon className={cls.icon} />} label="Fallback Email" {...areaTabProps(6)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.deleteaccount)} className={cls.tabtext} icon={<DeleteSweepIcon className={cls.icon} />} label="Delete" {...areaTabProps(7)} />
                </Tabs>
            </AppBar>
            <Align>
                {children}
            </Align>
        </div >
    )
};
