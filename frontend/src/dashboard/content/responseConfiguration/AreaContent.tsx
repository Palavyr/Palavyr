import { PanelRange, areaTabProps } from "@common/ContentUtils";
import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import { AppBar, Tabs, Tab, makeStyles } from "@material-ui/core";
import { useHistory, useParams } from "react-router-dom";
import AccountTreeIcon from "@material-ui/icons/AccountTree";
import FilterFramesIcon from "@material-ui/icons/FilterFrames";
import SubjectIcon from "@material-ui/icons/Subject";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";
import VisibilityIcon from "@material-ui/icons/Visibility";
import SettingsApplicationsIcon from "@material-ui/icons/SettingsApplications";
import { AuthContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { AreaSettingsLoc } from "@Palavyr-Types";

export interface IAreaContent {
    children: JSX.Element[] | JSX.Element;
}

export interface IAreaContentInner extends IAreaContent {
    setLoaded: Dispatch<SetStateAction<boolean>>;
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

export const AreaContent = ({ children }: IAreaContent) => {
    const [, setLoaded] = useState<boolean>(false);
    return <AreaContentInner setLoaded={setLoaded} children={children} />;
};

export const AreaContentInner = ({ setLoaded, children }: IAreaContentInner) => {
    const history = useHistory();
    const classes = useTabsStyles();

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
        <div className={classes.root}>
            <AppBar position="static" className={classes.appbar}>
                <Tabs centered value={tab} aria-label="simple tabs example">
                    <Tab onClick={() => sendTo(AreaSettingsLoc.email)} className={classes.tabtext} icon={<SubjectIcon className={classes.icon} />} label="1. Email" {...areaTabProps(0)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.response)} className={classes.tabtext} icon={<FilterFramesIcon className={classes.icon} />} label="2. Response" {...areaTabProps(1)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.attachments)} className={classes.tabtext} icon={<PictureAsPdfIcon className={classes.icon} />} label="3. Attachments" {...areaTabProps(2)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.conversation)} className={classes.tabtext} icon={<AccountTreeIcon className={classes.icon} />} label="4. Conversation" {...areaTabProps(3)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.settings)} className={classes.tabtext} icon={<SettingsApplicationsIcon className={classes.icon} />} label="5. Settings" {...areaTabProps(4)} />
                    <Tab onClick={() => sendTo(AreaSettingsLoc.preview)} className={classes.tabtext} icon={<VisibilityIcon className={classes.icon} />} label="6. Preview" {...areaTabProps(5)} />
                </Tabs>
            </AppBar>
            {children}
        </div>
    );
};
