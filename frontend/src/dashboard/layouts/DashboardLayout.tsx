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
        flexShrink: 0,

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
}))


export const DashboardLayout = () => {

    const { contentType, areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();

    const [sidebarNames, setSidebarNames] = useState<Array<string>>([]);
    const [sidebarIds, setSidebarIds] = useState<Array<string>>([]);

    const [, setLoaded] = useState<boolean>(false);

    const history = useHistory();

    const [open, setOpen] = useState<boolean>(true);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>('');
    const [active, setIsActive] = useState<boolean | null>(null);
    const [numAreasAllowed, setNumAreasAllowed] = useState<number | undefined>()

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

        setLoaded(true)
        loadAreas()
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
    const thema = useTheme();
    console.log("Dash: " + thema.mixins.toolbar.minHeight);
    return (
        <div className={classes.root}>
            <CssBaseline />
            <DashboardHeader open={open} handleDrawerOpen={handleDrawerOpen} handleHelpDrawerOpen={handleHelpDrawerOpen} helpOpen={helpOpen} title={currentViewName} />
            <Drawer
                className={classes.menuDrawer}
                variant="persistent"
                anchor="left"
                open={open}
                classes={{
                    paper: classes.menuDrawerPaper,
                }}
            >
                <SideBarHeader handleDrawerClose={handleDrawerClose} />
                <Divider />
                <SideBarMenu active={active ?? false} areaIdentifiers={sidebarIds} areaNames={sidebarNames} toggleModal={openModal} />
            </Drawer>

            {/* Any type of content should be loaded here */}
            <ContentLoader open={open}>
                {contentType === "editor" && <AreaContent active={active} areaIdentifier={areaIdentifier} areaName={currentViewName} setLoaded={setLoaded} setViewName={setViewName} />}
                {active && contentType === "settings" && <SettingsContent areaIdentifier={areaIdentifier} areaName={currentViewName} setLoaded={setLoaded} />}
                {active && contentType === "demo" && <ChatDemo />}
                {active && contentType === "enquiries" && <Enquires />}
                {active && contentType === "getwidget" && <GetWidget />}
                {active && contentType === "subscribe" && <Subscribe />}

                {contentType === undefined && <WelcomeToTheDashboard />}
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
                        {theme.direction === 'rtl' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                    </IconButton>
                </div>
                <Divider />
                {(contentType === "editor") && <EditorHelp />}
            </Drawer>

            {
                numAreasAllowed && (
                    sidebarIds.length < numAreasAllowed
                        ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} />
                        : null
                )
            }
        </div>
    );
};
