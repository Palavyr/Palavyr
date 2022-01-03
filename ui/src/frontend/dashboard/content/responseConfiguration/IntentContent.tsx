import { PanelRange, intentTabProps } from "@common/ContentUtils";
import React, { useState, useEffect } from "react";
import { AppBar, Tabs, Tab, makeStyles, Tooltip } from "@material-ui/core";
import { useHistory, useLocation, useParams } from "react-router-dom";
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

export interface IntentContentProps {
    children: JSX.Element[] | JSX.Element;
}

export interface IntentContentInnerProps extends IntentContentProps {
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
    tabbehavior: {
        pointer: "cursor",
        "&:hover": {
            backgroundColor: theme.palette.primary.dark,
        },
    },
    active: {
        backgroundColor: theme.palette.success.main,
    },
}));

export const IntentContent = ({ children }: IntentContentProps) => {
    const [, setLoaded] = useState<boolean>(false);
    return <IntentContentInner setLoaded={setLoaded} children={children} />;
};

export const IntentContentInner = ({ setLoaded, children }: IntentContentInnerProps) => {
    const history = useHistory();
    const location = useLocation();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const searchParams = new URLSearchParams(location.search);
    const rawTab = searchParams.get("tab");
    const tab = rawTab ? (parseInt(rawTab) as PanelRange) : 0;

    const cls = useStyles();
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

    const tabMetas = [
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
                <Tabs className={"editor-tabs-tour"} centered value={tab} TabIndicatorProps={{ style: { height: "0px" } }}>
                    {tabMetas.map((tabMeta, index) => {
                        return (
                            <Tooltip key={tabMeta.label} title={tabMeta.label}>
                                <Tab
                                    className={classNames(`${tabMeta.tourClassName}`, cls.tabtext, cls.tabbehavior, {
                                        [cls.active]: index === tab,
                                    })}
                                    onClick={() => {
                                        const spot = (tabMeta.label as string).replace(" ", "").toLowerCase();
                                        const loz = sendTo(spot, index);
                                        return loz;
                                    }}
                                    icon={tabMeta.icon}
                                    {...intentTabProps(index as PanelRange)}
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
