import { PanelRange, areaTabProps } from "@common/ContentUtils";
import React, { useState, useEffect } from "react";
import { AppBar, Tabs, Tab, makeStyles } from "@material-ui/core";
import { useHistory, useParams } from "react-router-dom";
import AccountTreeIcon from "@material-ui/icons/AccountTree";
import FilterFramesIcon from "@material-ui/icons/FilterFrames";
import SubjectIcon from "@material-ui/icons/Subject";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";
import VisibilityIcon from "@material-ui/icons/Visibility";
import SettingsApplicationsIcon from "@material-ui/icons/SettingsApplications";
import { AuthContext } from "dashboard/layouts/DashboardContext";
import { AreaSettingsLoc, SetState } from "@Palavyr-Types";

export interface IAreaContent {
    children: JSX.Element[] | JSX.Element;
}

export interface IAreaContentInner extends IAreaContent {
    setLoaded: SetState<boolean>;
}

const useStyles = makeStyles((theme) => ({
    root: {
        width: "100%",
        position: "absolute",
        flexGrow: 1,
        background: "none",
        height: "100%",
    },
    appbar: {
        backgroundColor: theme.palette.secondary.main,
        width: "100%",
        top: theme.mixins.toolbar.minHeight,
        height: "72px",
    },
    icon: {
        color: theme.palette.getContrastText(theme.palette.primary.main),
    },
    tabtext: {
        color: theme.palette.getContrastText(theme.palette.primary.main),
    },
}));

export const AreaContent = ({ children }: IAreaContent) => {
    const [, setLoaded] = useState<boolean>(false);
    return <AreaContentInner setLoaded={setLoaded} children={children} />;
};

export const AreaContentInner = ({ setLoaded, children }: IAreaContentInner) => {
    const history = useHistory();
    const cls = useStyles();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const searchParams = new URLSearchParams(location.search);
    const rawTab = searchParams.get("tab");
    const tab = rawTab ? (parseInt(rawTab) as PanelRange) : 0;

    const { isActive } = React.useContext(AuthContext);

    const sendTo = (dest: AreaSettingsLoc) => {
        history.push(`/dashboard/editor/${AreaSettingsLoc[dest]}/${areaIdentifier}?tab=${dest}`);
    };

    useEffect(() => {
        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, [tab, setLoaded]); // probably need to add a tracker for when the table is saved so can reload and update

    if (!isActive) {
        history.push("/dashboard/confirm");
    }

    return (
        <div className={cls.root}>
            <AppBar position="static" className={cls.appbar}>
                <Tabs centered value={tab} aria-label="simple tabs example">
                    <Tab onClick={() => sendTo(AreaSettingsLoc.email)} className={cls.tabtext} icon={<SubjectIcon className={cls.icon} />} label="1. Email" {...areaTabProps(0)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.response)} className={cls.tabtext} icon={<FilterFramesIcon className={cls.icon} />} label="2. Response" {...areaTabProps(1)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.attachments)} className={cls.tabtext} icon={<PictureAsPdfIcon className={cls.icon} />} label="3. Attachments" {...areaTabProps(2)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.conversation)} className={cls.tabtext} icon={<AccountTreeIcon className={cls.icon} />} label="4. Conversation" {...areaTabProps(3)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.settings)} className={cls.tabtext} icon={<SettingsApplicationsIcon className={cls.icon} />} label="5. Settings" {...areaTabProps(4)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.preview)} className={cls.tabtext} icon={<VisibilityIcon className={cls.icon} />} label="6. Preview" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>
            {children}
        </div>
    );
};
