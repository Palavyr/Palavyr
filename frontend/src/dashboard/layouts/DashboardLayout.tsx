import React, { useState, useEffect, useCallback } from "react";

import { SideBarHeader } from "./sidebar/SideBarHeader";
import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep } from "lodash";
import { AlertType, AreaNameDetails, Areas, AreaTable, PlanType } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { DashboardHeader } from "./header/DashboardHeader";
import { IconButton, makeStyles, Typography } from "@material-ui/core";
import { DRAWER_WIDTH } from "@constants";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { DashboardContext } from "./DashboardContext";
import { UserDetails } from "./sidebar/UserDetails";

const fetchSidebarInfo = (areaData: Areas): AreaNameDetails => {
    const areaNameDetails = areaData.map((x: AreaTable) => {
        return {
            areaIdentifier: x.areaIdentifier, areaName: x.areaName
        }
    });
    return areaNameDetails;
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
        backgroundColor: theme.palette.primary.light,
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

    const [areaNameDetails, setAreaNameDetails] = useState<AreaNameDetails>([]);
    const [, setLoaded] = useState<boolean>(false);

    const history = useHistory();

    const [open, setOpen] = useState<boolean>(true);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>("");

    const [numAreasAllowed, setNumAreasAllowed] = useState<number>(0);
    const [alertState, setAlertState] = useState<boolean>(false);

    const [planType, setPlanType] = useState<PlanType>();
    const [currencySymbol, setCurrencySymbol] = useState<string>("");

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dashboardAreasLoading, setDashboardAreasLoading] = useState<boolean>(false);

    const cls = useStyles(helpOpen);

    const loadAreas = useCallback(async () => {
        setDashboardAreasLoading(true);
        const client = new PalavyrRepository();

        // todo: Deprecate this call in the future once we are confident
        await client.Conversations.EnsureDBIsValid();

        const numAllowedBySubscription = await client.Settings.Subscriptions.getNumAreas();
        const currentPlanType = await client.Settings.Account.getCurrentPlan();
        setPlanType(currentPlanType.status);
        setNumAreasAllowed(numAllowedBySubscription);

        const areas = await client.Area.GetAreas();
        const areaNameDetails = fetchSidebarInfo(areas);
        setAreaNameDetails(areaNameDetails);

        const locale = await client.Settings.Account.GetLocale();
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
        var newNames = cloneDeep(areaNameDetails);

        newNames.push({areaName: newArea.areaName, areaIdentifier: newArea.areaIdentifier});
        setAreaNameDetails(newNames);

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
        if (numAreasAllowed && areaNameDetails.length >= numAreasAllowed) {
            setAlertState(true);
        } else {
            openModal();
        }
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
                <DashboardHeader open={open} handleDrawerOpen={handleDrawerOpen} handleHelpDrawerOpen={handleHelpDrawerOpen} helpOpen={helpOpen} title={currentViewName} />
                <Drawer
                    className={classNames(cls.menuDrawer)}
                    variant="persistent"
                    anchor="left"
                    open={open}
                    classes={{
                        paper: cls.menuDrawerPaper,
                    }}
                >
                    <SideBarHeader handleDrawerClose={handleDrawerClose} />
                    <UserDetails />
                    <Divider />
                    <SideBarMenu areaNameDetails={areaNameDetails} />
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
                            <Typography>Close</Typography>
                            <ChevronRightIcon style={{ color: "black" }} />
                        </IconButton>
                    </div>
                    <Divider />
                    {helpComponent}
                </Drawer>
                {numAreasAllowed && (areaNameDetails.length < numAreasAllowed ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} /> : null)}
                <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
            </div>
        </DashboardContext.Provider>
    );
};
