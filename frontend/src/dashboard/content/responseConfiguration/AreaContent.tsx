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
import { editorTourSteps } from "../welcome/OnboardingTour/tours/editorTour";
import { IntroSteps } from "../welcome/OnboardingTour/IntroSteps";
import classNames from "classnames";

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
            <IntroSteps initialize={true} steps={editorTourSteps} />
            <AppBar position="static" className={cls.appbar}>
                <Tabs className={"editor-tabs-tour"} centered value={tab}>
                    <Tab
                        className={classNames("email-editor-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.email)}
                        icon={<SubjectIcon className={cls.icon} />}
                        label="1. Email"
                        {...areaTabProps(0)}
                    />
                    <Tab
                        className={classNames("response-editor-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.response)}
                        icon={<FilterFramesIcon className={cls.icon} />}
                        label="2. Response"
                        {...areaTabProps(1)}
                    />
                    <Tab
                        className={classNames("attachments-editor-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.attachments)}
                        icon={<PictureAsPdfIcon className={cls.icon} />}
                        label="3. Attachments"
                        {...areaTabProps(2)}
                    />
                    <Tab
                        className={classNames("conversation-editor-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.conversation)}
                        icon={<AccountTreeIcon className={cls.icon} />}
                        label="4. Conversation"
                        {...areaTabProps(3)}
                    />
                    <Tab
                        className={classNames("settings-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.settings)}
                        icon={<SettingsApplicationsIcon className={cls.icon} />}
                        label="5. Settings"
                        {...areaTabProps(4)}
                    />
                    <Tab
                        className={classNames("preview-tab-tour", cls.tabtext)}
                        onClick={() => sendTo(AreaSettingsLoc.preview)}
                        icon={<VisibilityIcon className={cls.icon} />}
                        label="6. Preview"
                        {...areaTabProps(5)}
                    />
                </Tabs>
            </AppBar>
            {children}
        </div>
    );
};
