import React, { useState, useEffect, useCallback } from "react";

import { SideBarHeader } from "./sidebar/SideBarHeader";
import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep } from "lodash";
import { AlertType, Areas, AreaTable, PlanType } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { DashboardHeader } from "./header/DashboardHeader";
import { CssBaseline, IconButton, makeStyles, useTheme } from "@material-ui/core";
import { DRAWER_WIDTH } from "@constants";

import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { DashboardContext } from "./DashboardContext";
import { webUrl } from "@api-client/clientUtils";

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
        backgroundColor: "rgb(253,236,234)",
    },
    helpDrawerPaper: {
        width: DRAWER_WIDTH + 300,
        paddingLeft: "1rem",
        paddingRight: "1rem",
        paddingTop: "1rem",
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

    const [numAreasAllowed, setNumAreasAllowed] = useState<number>(0);
    const [alertState, setAlertState] = useState<boolean>(false);

    const [widgetState, setWidgetState] = useState<boolean | undefined>();
    const [planType, setPlanType] = useState<PlanType>();
    const [currencySymbol, setCurrencySymbol] = useState<string>("");

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dashboardAreasLoading, setDashboardAreasLoading] = useState<boolean>(false);

    const cls = useStyles(helpOpen);
    const theme = useTheme();

    const loadAreas = useCallback(async () => {
        setDashboardAreasLoading(true);
        const client = new ApiClient();

        // todo: Deprecate this call in the future once we are confident
        await client.Conversations.EnsureDBIsValid();

        const { data: numAllowedBySubscription } = await client.Settings.Subscriptions.getNumAreas();
        const { data: currentPlanType } = await client.Settings.Account.getCurrentPlan();
        setPlanType(currentPlanType.status);
        setNumAreasAllowed(numAllowedBySubscription);

        const { data: areas } = await client.Area.GetAreas();
        const [areaIdentifiers, areaNames] = fetchSidebarInfo(areas);
        setSidebarNames(areaNames);
        setSidebarIds(areaIdentifiers);

        const { data: currentWidgetState } = await client.Configuration.WidgetState.GetWidgetState();
        setWidgetState(currentWidgetState);

        const { data: locale } = await client.Settings.Account.GetLocale();
        setCurrencySymbol(locale.localeCurrencySymbol);

        if (areaIdentifier) {
            var currentView = areas.filter((x: AreaTable) => x.areaIdentifier === areaIdentifier).pop();

            if (currentView) {
                setViewName(currentView.areaName);
            }
        }
        setDashboardAreasLoading(false);
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

    const updateWidgetIsActive = async () => {
        const client = new ApiClient();
        const { data: updatedWidgetState } = await client.Configuration.WidgetState.SetWidgetState(!widgetState);
        setWidgetState(updatedWidgetState);
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

    const createCustomerPortalSession = async () => {
        const client = new ApiClient();
        var returnUrl = `${webUrl}/dashboard/`;
        const { data: customerId } = await client.Purchase.Customer.GetCustomerId();
        const { data: portalUrl } = await client.Purchase.Customer.GetCustomerPortal(customerId, returnUrl);
        window.location.href = portalUrl;
    };

    const alertDetails: AlertType = {
        title: "Maximum areas reached",
        message: "Thanks for using Palavyr! Please consider purchasing a subscription to increase the number of areas you can provide.",
        link: "/dashboard/subscribe",
        linktext: "Subscriptions",
    };

    return (
        <DashboardContext.Provider value={{ setIsLoading: setIsLoading, currencySymbol: currencySymbol, subscription: planType, numAreasAllowed, checkAreaCount, areaName: currentViewName, setViewName: setViewName }}>
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
                        <SideBarMenu areaIdentifiers={sidebarIds} areaNames={sidebarNames} widgetIsActive={widgetState} updateWidgetIsActive={updateWidgetIsActive} createCustomerPortalSession={createCustomerPortalSession} />
                    </>
                </Drawer>
                <ContentLoader isLoading={isLoading} dashboardAreasLoading={dashboardAreasLoading} open={open}>
                    {children}
                </ContentLoader>
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
