import React, { useState, useEffect, useCallback } from "react";

import { SideBarHeader } from "./sidebar/SideBarHeader";
import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AreaContent } from "../content/responseConfiguration/AreaContent";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep } from "lodash";
import { Areas, AreaTable } from "@Palavyr-Types";
import { SettingsContent } from "../content/settings/SettingsContent";
import { ChatDemo } from "../content/demo/ChatDemo";
import { Enquires } from "dashboard/content/enquiries/Enquiries";
import { GetWidget } from "dashboard/content/getWidget/GetWidget";
import { Subscribe } from "dashboard/content/subscribe/Subscribe";
import { ApiClient } from "@api-client/Client";
import { DashboardHeader } from "./header/DashboardHeader";
import { CssBaseline, IconButton, makeStyles, useTheme } from "@material-ui/core";
import { EditorHelp } from "dashboard/content/help/EditorHelp";
import { WelcomeToTheDashboard } from "dashboard/content/welcome/WelcomeToTheDashboard";
import { DRAWER_WIDTH } from "@common/constants";

import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import ChevronRightIcon from '@material-ui/icons/ChevronRight';
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";
import { EmailHelp } from "dashboard/content/help/EmailHelp";
import { EstimateHelp } from "dashboard/content/help/EstimateHelp";
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp";
import { AreaSettingsHelp } from "dashboard/content/help/AreaSettingsHelp";
import { PreviewHelp } from "dashboard/content/help/PreviewHelp";
import { PleaseConfirmYourEmail } from "dashboard/content/welcome/PleaseConfirmYourEmail";


const fetchSidebarInfo = (areaData: Areas) => {
    const areaIdentifiers = areaData.map((x: AreaTable) => x.areaIdentifier);
    const areaNames = areaData.map((x: AreaTable) => x.areaName);
    return [areaIdentifiers, areaNames];
};


const useStyles = makeStyles(theme => ({
    root: {
        position: "absolute", // Required - finalized
        display: "flex",
        width: "100%",
        top: "8px"
    },
    menuDrawer: {
        width: DRAWER_WIDTH,
        flexShrink: 0
    },
    menuBorder: {
        // border: "3px solid black"
    },
    helpDrawer: (helpOpen: boolean) => {
        return {
            width: helpOpen ? DRAWER_WIDTH + 300 : 0,
            flexShrink: 0,
        }
    },
    helpDrawerHeader: {
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        justifyContent: "flex-end",
        ...theme.mixins.toolbar,
    },
    menuDrawerPaper: {
        width: DRAWER_WIDTH,
        backgroundColor: "#535c68"
    },
    helpDrawerPaper: {
        width: DRAWER_WIDTH + 300,
    }
}));

export type HelpTypes =
    "editor"
    | "settings"
    | "demo"
    | "enquiries"
    | "getwidget"
    | "subscribe"
    | "conversation"
    | "estimate"
    | "email"
    | "attachments"
    | "preview"
    | "areasettings"
    | "password"
    | "email"
    | "companyname"
    | "phonenumber"
    | "logo"
    | "locale"



export const DashboardLayout = () => {

    const { contentType, areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();

    const [sidebarNames, setSidebarNames] = useState<Array<string>>([]);
    const [sidebarIds, setSidebarIds] = useState<Array<string>>([]);

    const [loaded, setLoaded] = useState<boolean>(false);

    const history = useHistory();

    const [open, setOpen] = useState<boolean>(true);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>('');
    const [active, setIsActive] = useState<boolean | null>(null);
    const [numAreasAllowed, setNumAreasAllowed] = useState<number | undefined>()
    const [alertState, setAlertState] = useState<boolean>(false);
    const [helpType, setHelpType] = useState<HelpTypes | null>(null);


    const classes = useStyles(helpOpen);
    const theme = useTheme();

    const loadAreas = useCallback(async () => {
        var client = new ApiClient();

        var isActive = (await client.Settings.Account.checkIsActive()).data as boolean;
        setIsActive(isActive);

        var numAllowedBySubscription = (await client.Settings.Subscriptions.getNumAreas()).data as number;
        setNumAreasAllowed(numAllowedBySubscription);

        var res = await client.Area.GetAreas();
        const [areaIdentifiers, areaNames] = fetchSidebarInfo(res.data);
        setSidebarNames(areaNames);
        setSidebarIds(areaIdentifiers);

        if (areaIdentifier) {
            var currentView = res.data.filter((x: AreaTable) => x.areaIdentifier === areaIdentifier).pop()
            setViewName(currentView.areaName)
        }
    }, [areaIdentifier])

    useEffect(() => {

        loadAreas()
        setLoaded(true)
        return () => {
            setLoaded(false)
            setViewName("");
        }
    }, [areaIdentifier, loadAreas]);

    const setNewArea = (newArea: AreaTable) => {
        var newNames = cloneDeep(sidebarNames);

        newNames.push(newArea.areaName);
        setSidebarNames(newNames);

        var newIds = cloneDeep(sidebarIds);
        newIds.push(newArea.areaIdentifier);

        setSidebarIds(newIds);

        history.push(`/dashboard/editor/${newArea.areaIdentifier}`);
    };

    const handleDrawerClose: () => void = () => {
        setOpen(false);
    };

    const handleDrawerOpen: () => void = () => {
        setOpen(true);
    };

    const handleHelpDrawerOpen: () => void = () => {
        setHelpOpen(true);
    }

    const handleHelpDrawerClose: () => void = () => {
        setHelpOpen(false);
    }

    const openModal = () => {
        setModalState(true);
    };
    const closeModal = () => {
        setModalState(false);
    };
    const checkAreaCount = () => {
        if (numAreasAllowed && (sidebarIds.length >= numAreasAllowed)) {
            setAlertState(true);
        } else {
            openModal();
        }
    }

    const alertDetails = {
        title: "Maximum areas reached",
        message: "Thanks for using Palavyr! Please consider purchasing a subscription to increase the number of areas you can provide.",
        link: "/dashboard/subscribe",
        linktext: "Subscriptions"
    }

    const thema = useTheme();
    return (
        <div className={classes.root}>
            <CssBaseline />
            <DashboardHeader open={open} handleDrawerOpen={handleDrawerOpen} handleHelpDrawerOpen={handleHelpDrawerOpen} helpOpen={helpOpen} title={currentViewName} />
            <Drawer
                className={classNames(classes.menuDrawer, classes.menuBorder)}
                variant="persistent"
                anchor="left"
                open={open}
                classes={{
                    paper: classes.menuDrawerPaper,
                    root: classes.menuBorder,
                    modal: classes.menuBorder
                }}
            >
                <SideBarHeader handleDrawerClose={handleDrawerClose} />
                <Divider />
                <SideBarMenu checkAreaCount={checkAreaCount} setViewName={setViewName} active={active ?? false} areaIdentifiers={sidebarIds} areaNames={sidebarNames} toggleModal={openModal} setAlertState={setAlertState} />
            </Drawer>

            {/* Any type of content should be loaded here */}
            <ContentLoader open={open}>
                {/* {loaded === true && (!active || active === undefined || active === null) && <PleaseConfirmYourEmail />} */}
                {contentType === "editor" && (active === true) && <AreaContent checkAreaCount={checkAreaCount} setHelpType={setHelpType} active={active} areaIdentifier={areaIdentifier} areaName={currentViewName} setLoaded={setLoaded} setViewName={setViewName} />}
                {active && contentType === "settings" && <SettingsContent setHelpType={setHelpType} areaIdentifier={areaIdentifier} areaName={currentViewName} setLoaded={setLoaded} />}
                {active && contentType === "demo" && <ChatDemo setHelpType={setHelpType} />}
                {active && contentType === "enquiries" && <Enquires setHelpType={setHelpType} />}
                {active && contentType === "getwidget" && <GetWidget setHelpType={setHelpType} />}
                {active && contentType === "subscribe" && <Subscribe setHelpType={setHelpType} />}

                {contentType === undefined && active === true && <WelcomeToTheDashboard checkAreaCount={checkAreaCount} />}
            </ContentLoader>
            <Drawer
                className={classes.helpDrawer}
                variant="persistent"
                anchor="right"
                open={helpOpen}
                classes={{
                    paper: classes.helpDrawerPaper
                }}
            >
                <div className={classes.helpDrawerHeader}>
                    <IconButton onClick={handleHelpDrawerClose}>
                        Close
                        {theme.direction === 'rtl' ? <ChevronLeftIcon style={{color: "white"}} /> : <ChevronRightIcon style={{color: "white"}} />}
                    </IconButton>
                </div>
                <Divider />
                {(helpType === "conversation") && <ConversationHelp defaultOpen />}
                {(helpType === "editor") && <EditorHelp defaultOpen/>}
                {(helpType === "email") && <EmailHelp defaultOpen/>}

                {(helpType === "estimate") && <EstimateHelp defaultOpen />}
                {(helpType === "attachments") && <AttachmentsHelp defaultOpen/>}
                {(helpType === "areasettings") && <AreaSettingsHelp defaultOpen/>}
                {(helpType === "preview") && <PreviewHelp defaultOpen/>}


                {/* {(helpType === "settings") && <SettingsHelp />}
                {(helpType === "demo") && <DemoHelp />}
                {(helpType === "enquiries") && <EnquiryHelp />}
                {(helpType === "getwidget") && <GetWidgetHelp />}
                {(helpType === "subscrible") && <SubscribeHelp />}
                {(helpType === "password") && <PasswordHelp />}
                {(helpType === "companyname") && <CompanyNameHelp />}
                {(helpType === "phonenumber") && <PhoneNumberHelp />}
                {(helpType === "logo") && <LogoHelp />}
                {(helpType === "locale") && <LocaleHelp />} */}

            </Drawer>
            {
                numAreasAllowed && (
                    (sidebarIds.length < numAreasAllowed)
                        ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} />
                        : null
                )
            }
            <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
        </div>
    );
};
