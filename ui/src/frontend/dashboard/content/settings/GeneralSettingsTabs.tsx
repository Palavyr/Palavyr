import React, { useState, useEffect } from "react";
import AppBar from "@material-ui/core/AppBar";
import Tab from "@material-ui/core/Tab";
import Tabs from "@material-ui/core/Tabs";
import { intentTabProps, PanelRange } from "@common/ContentUtils";
import { makeStyles } from "@material-ui/core";
import LockOpenIcon from "@material-ui/icons/LockOpen";
import MailOutlineIcon from "@material-ui/icons/MailOutline";
import PermIdentityIcon from "@material-ui/icons/PermIdentity";
import PhoneIcon from "@material-ui/icons/Phone";
import BrandingWatermarkIcon from "@material-ui/icons/BrandingWatermark";
import PublicIcon from "@material-ui/icons/Public";
import { Align } from "@common/positioning/Align";
import { useHistory } from "react-router-dom";
import { GeneralSettingsLoc } from "@Palavyr-Types";
import DeleteSweepIcon from "@material-ui/icons/DeleteSweep";
import SubjectIcon from "@material-ui/icons/Subject";

type StyleProps = {
    accountTypeNeedsPassword: boolean;
};

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    root: {
        flexGrow: 1,
    },
    appbar: {
        background: theme.palette.secondary.light,
        width: "100%",
        top: theme.mixins.toolbar.minHeight,
        height: "72px",
    },
    icon: {
        color: theme.palette.primary.dark,
    },
    tabtext: {
        color: theme.palette.primary.dark,
    },

    passicon: {
        color: "navy",
        cursor: "pointer",
    },
    passtabtext: {
        color: "navy",
        cursor: "pointer",
    },
}));

interface GeneralSettingsContentProps {
    children: JSX.Element[] | JSX.Element;
}

interface GeneralSettingsContentInner extends GeneralSettingsContentProps {
    setLoaded(val: boolean): void;
}

export const GeneralSettingsTabs = ({ children }: GeneralSettingsContentProps) => {
    const [, setLoaded] = useState<boolean>(false);
    return <GeneralSettingsTabsInner setLoaded={setLoaded} children={children} />;
};

const GeneralSettingsTabsInner = ({ setLoaded, children }: GeneralSettingsContentInner) => {
    const history = useHistory();

    const cls = useStyles();
    const searchParams = new URLSearchParams(location.search);
    const rawTab = searchParams.get("tab");
    const tab = rawTab ? (parseInt(rawTab) as PanelRange) : 0;

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    const sendTo = (dest: GeneralSettingsLoc) => {
        history.push(`/dashboard/settings/${GeneralSettingsLoc[dest]}?tab=${dest}`);
    };

    return (
        <div className={cls.root}>
            <AppBar position="static" className={cls.appbar}>
                <Tabs centered value={tab} aria-label="simple tabs">
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.email)} className={cls.tabtext} icon={<MailOutlineIcon className={cls.icon} />} label="Email Address" {...intentTabProps(0)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyName)} className={cls.tabtext} icon={<PermIdentityIcon className={cls.icon} />} label="Company Name" {...intentTabProps(1)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.phoneNumber)} className={cls.tabtext} icon={<PhoneIcon className={cls.icon} />} label="Phone Number" {...intentTabProps(2)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.companyLogo)} className={cls.tabtext} icon={<BrandingWatermarkIcon className={cls.icon} />} label="Company Logo" {...intentTabProps(3)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.locale)} className={cls.tabtext} icon={<PublicIcon className={cls.icon} />} label="Locale" {...intentTabProps(4)} />
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.default_email_template)} className={cls.tabtext} icon={<SubjectIcon className={cls.icon} />} label="Fallback Email" {...intentTabProps(5)} />
                    {<Tab onClick={() => sendTo(GeneralSettingsLoc.password)} className={cls.passtabtext} icon={<LockOpenIcon className={cls.passicon} />} label="Password" {...intentTabProps(6)} />}
                    <Tab onClick={() => sendTo(GeneralSettingsLoc.deleteaccount)} className={cls.tabtext} icon={<DeleteSweepIcon className={cls.icon} />} label="Delete" {...intentTabProps(7)} />
                </Tabs>
            </AppBar>
            <Align>{children}</Align>
        </div>
    );
};
