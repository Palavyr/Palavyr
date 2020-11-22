import React, { useState, useEffect, useCallback } from "react";

import { SideBarHeader } from "./sidebar/SideBarHeader";
import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep } from "lodash";
import { Areas, AreaTable, HelpTypes } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { DashboardHeader } from "./header/DashboardHeader";
import { CssBaseline, IconButton, makeStyles, useTheme } from "@material-ui/core";
import { DRAWER_WIDTH } from "@common/constants";

import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { DashboardContext } from "./DashboardContext";

const fetchSidebarInfo = (areaData: Areas) => {
    const areaIdentifiers = areaData.map((x: AreaTable) => x.areaIdentifier);
    const areaNames = areaData.map((x: AreaTable) => x.areaName);
    return [areaIdentifiers, areaNames];
};

const useStyles = makeStyles((theme) => ({
    root: {
        position: "absolute", // Required - finalized
        display: "flex",
        width: "100%",
        top: "8px",
    },
    menuDrawer: {
        width: DRAWER_WIDTH,
        flexShrink: 0,
    },
    menuBorder: {
        // border: "3px solid black"
    },
    helpDrawer: (helpOpen: boolean) => {
        return {
            width: helpOpen ? DRAWER_WIDTH + 300 : 0,
            flexShrink: 0,
        };
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
        // backgroundColor: "#535c68",
        backgroundColor: "#c7ecee",
    },
    helpDrawerPaper: {
        width: DRAWER_WIDTH + 300,
    },
}));

interface IDashboardLayout {
    children: JSX.Element[] | JSX.Element;
    helpComponent: JSX.Element[] | JSX.Element;
}

export const DashboardLayout = ({ helpComponent, children }: IDashboardLayout) => {
    const { areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();

    const [sidebarNames, setSidebarNames] = useState<Array<string>>([]);
    const [sidebarIds, setSidebarIds] = useState<Array<string>>([]);

    const [, setLoaded] = useState<boolean>(false);

    const history = useHistory();

    const [open, setOpen] = useState<boolean>(true);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>("");

    const [numAreasAllowed, setNumAreasAllowed] = useState<number | undefined>();
    const [alertState, setAlertState] = useState<boolean>(false);

    const cls = useStyles(helpOpen);
    const theme = useTheme();

    const loadAreas = useCallback(async () => {

        const client = new ApiClient();
        const {data: numAllowedBySubscription} = await client.Settings.Subscriptions.getNumAreas();
        setNumAreasAllowed(numAllowedBySubscription);

        const {data: areas} = await client.Area.GetAreas();
        const [areaIdentifiers, areaNames] = fetchSidebarInfo(areas);
        setSidebarNames(areaNames);
        setSidebarIds(areaIdentifiers);

        if (areaIdentifier) {
            var currentView = areas.filter((x: AreaTable) => x.areaIdentifier === areaIdentifier).pop();

            if (currentView) {
                setViewName(currentView.areaName);
            }
        }
    }, [areaIdentifier]);

    useEffect(() => {
        loadAreas();
        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadAreas]);

    const setNewArea = (newArea: AreaTable) => {
        var newNames = cloneDeep(sidebarNames);

        newNames.push(newArea.areaName);
        setSidebarNames(newNames);

        var newIds = cloneDeep(sidebarIds);
        newIds.push(newArea.areaIdentifier);

        setSidebarIds(newIds);

        history.push(`/dashboard/editor/email/${newArea.areaIdentifier}?tab=0`);
    };

    const handleDrawerClose: () => void = () => {
        setOpen(false);
    };

    const handleDrawerOpen: () => void = () => {
        setOpen(true);
    };

    const handleHelpDrawerOpen: () => void = () => {
        setHelpOpen(true);
    };

    const handleHelpDrawerClose: () => void = () => {
        setHelpOpen(false);
    };

    const openModal = () => {
        setModalState(true);
    };
    const closeModal = () => {
        setModalState(false);
    };
    const checkAreaCount = () => {
        if (numAreasAllowed && sidebarIds.length >= numAreasAllowed) {
            setAlertState(true);
        } else {
            openModal();
        }
    };

    const alertDetails = {
        title: "Maximum areas reached",
        message: "Thanks for using Palavyr! Please consider purchasing a subscription to increase the number of areas you can provide.",
        link: "/dashboard/subscribe",
        linktext: "Subscriptions",
    };

    return (
        <DashboardContext.Provider value={{ numAreasAllowed, checkAreaCount, areaName: currentViewName, setViewName: setViewName }}>
            <div className={cls.root}>
                <CssBaseline />
                <DashboardHeader open={open} handleDrawerOpen={handleDrawerOpen} handleHelpDrawerOpen={handleHelpDrawerOpen} helpOpen={helpOpen} title={currentViewName} />
                <Drawer
                    className={classNames(cls.menuDrawer, cls.menuBorder)}
                    variant="persistent"
                    anchor="left"
                    open={open}
                    classes={{
                        paper: cls.menuDrawerPaper,
                        root: cls.menuBorder,
                        modal: cls.menuBorder,
                    }}
                >
                    <>
                        <SideBarHeader handleDrawerClose={handleDrawerClose} />
                        <Divider />
                        <SideBarMenu areaIdentifiers={sidebarIds} areaNames={sidebarNames} />
                    </>
                </Drawer>
                <ContentLoader open={open}>{children}</ContentLoader>
                <Drawer
                    className={cls.helpDrawer}
                    variant="persistent"
                    anchor="right"
                    open={helpOpen}
                    classes={{
                        paper: cls.helpDrawerPaper,
                    }}
                >
                    <div className={cls.helpDrawerHeader}>
                        <IconButton onClick={handleHelpDrawerClose}>
                            Close
                            {theme.direction === "rtl" ? <ChevronLeftIcon style={{ color: "white" }} /> : <ChevronRightIcon style={{ color: "white" }} />}
                        </IconButton>
                    </div>
                    <Divider />
                    {helpComponent}
                </Drawer>
                {numAreasAllowed && (sidebarIds.length < numAreasAllowed ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} /> : null)}
                <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
            </div>
        </DashboardContext.Provider>
    );
};
