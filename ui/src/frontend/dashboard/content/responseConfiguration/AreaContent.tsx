import { PanelRange, areaTabProps } from "@common/ContentUtils";
import React, { useState, useEffect } from "react";
import { AppBar, Tabs, Tab, makeStyles, Tooltip } from "@material-ui/core";
import { useHistory, useParams } from "react-router-dom";
import AccountTreeIcon from "@material-ui/icons/AccountTree";
import FilterFramesIcon from "@material-ui/icons/FilterFrames";
import SubjectIcon from "@material-ui/icons/Subject";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";
import VisibilityIcon from "@material-ui/icons/Visibility";
import SettingsApplicationsIcon from "@material-ui/icons/SettingsApplications";
import { AuthContext } from "frontend/dashboard/layouts/DashboardContext";
import { SetState } from "@Palavyr-Types";
import { editorTourSteps } from "../welcome/OnboardingTour/tours/editorTour";
import { IntroSteps } from "../welcome/OnboardingTour/IntroSteps";
import classNames from "classnames";
import Cookies from "js-cookie";
import { EDITOR_TOUR_COOKIE_NAME } from "@constants";

export interface IAreaContent {
    children: JSX.Element[] | JSX.Element;
}

export interface IAreaContentInner extends IAreaContent {
    setLoaded: SetState<boolean>;
}

const useStyles = makeStyles(theme => ({
    root: {},
    appbar: {
        backgroundColor: theme.palette.secondary.main,
        top: theme.mixins.toolbar.minHeight,
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

    const sendTo = (loc: string, dest: number) => {
        history.push(`/dashboard/editor/${loc}/${areaIdentifier}?tab=${dest}`);
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

    const [editorTour, setEditorTour] = useState<boolean>(false);

    useEffect(() => {
        if (Cookies.get(EDITOR_TOUR_COOKIE_NAME) === undefined) {
            setEditorTour(true);
        }
    }, []);

    const editorTourOnBlur = () => {
        Cookies.set(EDITOR_TOUR_COOKIE_NAME, "", {
            expires: 9999,
        });
    };

    const orderedTables = [
        {
            tourClassName: "response-editor-tab-tour",
            icon: <FilterFramesIcon className={cls.icon} />,
            label: "Pricing",
        },
        {
            tourClassName: "preview-tab-tour",
            icon: <VisibilityIcon className={cls.icon} />,
            label: "Pricing Preview",
        },
        {
            tourClassName: "conversation-editor-tab-tour",
            icon: <AccountTreeIcon className={cls.icon} />,
            label: "Conversation",
        },
        {
            tourClassName: "email-editor-tab-tour",
            icon: <SubjectIcon className={cls.icon} />,
            label: "Email",
        },

        {
            tourClassName: "attachments-editor-tab-tour",
            icon: <PictureAsPdfIcon className={cls.icon} />,
            label: "Attachments",
        },
        {
            tourClassName: "settings-tab-tour",
            icon: <SettingsApplicationsIcon className={cls.icon} />,
            label: "Settings",
        },
    ];

    return (
        <div className={cls.root}>
            {editorTour && <IntroSteps initialize={editorTour} onBlur={editorTourOnBlur} steps={editorTourSteps} />}
            <AppBar position="static" className={cls.appbar}>
                <Tabs className={"editor-tabs-tour"} centered value={tab}>
                    {orderedTables.map((tab, index) => {
                        return (
                            <Tooltip key={tab.label} title={tab.label}>
                                <Tab
                                    className={classNames(`${tab.tourClassName}`, cls.tabtext)}
                                    onClick={() => {
                                        const spot = (tab.label as string).replace(" ", "").toLowerCase();
                                        const loz = sendTo(spot, index);
                                        return loz;
                                    }}
                                    icon={tab.icon}
                                    {...areaTabProps(index as PanelRange)}
                                />
                            </Tooltip>
                        );
                    })}
                </Tabs>
            </AppBar>
            {children}
        </div>
    );
};
