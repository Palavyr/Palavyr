import React, { useState, useEffect, useCallback } from "react";

import { SideBarHeader } from "./sidebar/SideBarHeader";
import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep, truncate } from "lodash";
import { AlertType, AreaNameDetail, AreaNameDetails, Areas, AreaTable, EnquiryRow, ErrorResponse, PlanTypeMeta, PurchaseTypes, SnackbarPositions } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { DashboardHeader } from "./header/DashboardHeader";
import { makeStyles, Typography } from "@material-ui/core";
import { defaultUrlForNewArea, DRAWER_WIDTH } from "@constants";
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { DashboardContext } from "./DashboardContext";
import { PalavyrSnackbar } from "@common/components/PalavyrSnackbar";
import { redirectToHomeWhenSessionNotEstablished } from "@api-client/clientUtils";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { ApiErrors } from "./Errors/ApiErrors";
import { Loaders } from "@api-client/Loaders";
import { IntroSteps } from "dashboard/content/welcome/OnboardingTour/IntroSteps";
import { welcomeTourSteps } from "dashboard/content/welcome/OnboardingTour/tours/welcomeTour";

const fetchSidebarInfo = (areaData: Areas): AreaNameDetails => {
    const areaNameDetails = areaData.map((x: AreaTable) => {
        return {
            areaIdentifier: x.areaIdentifier,
            areaName: x.areaName,
        };
    });
    return areaNameDetails;
};

const useStyles = makeStyles((theme) => ({
    root: {
        position: "absolute", // Required - finalized
        display: "flex",
        width: "100%",
        top: "8px",
        paddingBottom: "5rem",
    },
    menuDrawer: {
        width: DRAWER_WIDTH,
        flexShrink: 0,
    },
    helpDrawer: (helpOpen: boolean) => {
        return {
            zIndex: 99999,
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
    helpDrawerHeaderText: {
        color: theme.palette.common.white,
    },
}));

interface IDashboardLayout {
    children: JSX.Element[] | JSX.Element;
    helpComponent: JSX.Element[] | JSX.Element;
}

export const DashboardLayout = ({ helpComponent, children }: IDashboardLayout) => {
    const history = useHistory();

    const { areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();

    const [areaNameDetails, setAreaNameDetails] = useState<AreaNameDetails>([]);
    const [, setLoaded] = useState<boolean>(false);

    const [open, setOpen] = useState<boolean>(true);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>("");

    const [alertState, setAlertState] = useState<boolean>(false);

    const [currencySymbol, setCurrencySymbol] = useState<string>("");
    const [planTypeMeta, setPlanTypeMeta] = useState<PlanTypeMeta>();

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dashboardAreasLoading, setDashboardAreasLoading] = useState<boolean>(false);
    const cls = useStyles(helpOpen);

    const [panelErrors, setPanelErrors] = useState<ErrorResponse | null>(null);

    const [successOpen, setSuccessOpen] = useState<boolean>(false);
    const [successText, setSuccessText] = useState<string>("Success");

    const [warningOpen, setWarningOpen] = useState<boolean>(false);
    const [warningText, setWarningText] = useState<string>("Warning");

    const [errorOpen, setErrorOpen] = useState<boolean>(false);
    const [errorText, setErrorText] = useState<string>("Error");

    const [snackPosition, setSnackPosition] = useState<SnackbarPositions>("br");
    const [accountTypeNeedsPassword, setAccountTypeNeedsPassword] = useState<boolean>(false);

    const [unseenNotifications, setUnseenNotifications] = useState<number>(0);

    const loaders = new Loaders(setIsLoading);
    const apiErrors = new ApiErrors(setSuccessOpen, setSuccessText, setWarningOpen, setWarningText, setErrorOpen, setErrorText, setPanelErrors);
    const repository = new PalavyrRepository(apiErrors, loaders);

    useEffect(() => {
        (async () => {
            await redirectToHomeWhenSessionNotEstablished(history, repository);
        })();
    }, []);

    const loadAreas = useCallback(async () => {
        setDashboardAreasLoading(true);

        const planTypeMeta = await repository.Settings.Subscriptions.getCurrentPlanMeta();
        setPlanTypeMeta(planTypeMeta);

        const areas = await repository.Area.GetAreas();
        setAreaNameDetails(sortByPropertyAlphabetical((x: AreaNameDetail) => x.areaName, fetchSidebarInfo(areas)));

        const locale = await repository.Settings.Account.GetLocale();
        setCurrencySymbol(locale.localeCurrencySymbol);

        const needsPassword = await repository.Settings.Account.CheckNeedsPassword();
        setAccountTypeNeedsPassword(needsPassword);

        const enqs = await repository.Enquiries.getEnquiries();
        const numUnseen = enqs.filter((x: EnquiryRow) => !x.seen).length;
        setUnseenNotifications(numUnseen);

        if (areaIdentifier) {
            const currentView = areas.filter((x: AreaTable) => x.areaIdentifier === areaIdentifier).pop();

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

        newNames.push({ areaName: newArea.areaName, areaIdentifier: newArea.areaIdentifier });
        setAreaNameDetails(newNames);
        history.push(defaultUrlForNewArea(newArea.areaIdentifier));
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
        if (planTypeMeta && areaNameDetails.length >= planTypeMeta.allowedAreas) {
            setAlertState(true);
        } else {
            openModal();
        }
    };

    const alertDetails: AlertType = {
        title: `Maximum areas reached for your current plan (${planTypeMeta ? planTypeMeta.planType : ""})`,
        message:
            planTypeMeta && planTypeMeta.planType === PurchaseTypes.Free
                ? "Thanks for using Palavyr! Please consider purchasing a subscription to increase the number of areas you can provide."
                : "Thanks for using Palavyr! To increase the number of areas you can provide, please consider upgrading your subscription the Manage link in the side bar menu.",
        link: planTypeMeta && planTypeMeta.isFreePlan ? "/dashboard/subscribe" : "/dashboard",
        linktext: "Subscriptions",
    };

    return (
        <>
            <IntroSteps initialize={false} steps={welcomeTourSteps} />
            <DashboardContext.Provider
                value={{
                    accountTypeNeedsPassword,
                    successOpen,
                    setSuccessOpen,
                    successText,
                    setSuccessText,
                    warningOpen,
                    setWarningOpen,
                    warningText,
                    setWarningText,
                    errorOpen,
                    errorText,
                    setErrorOpen,
                    setErrorText,
                    snackPosition,
                    setSnackPosition,
                    setIsLoading: setIsLoading,
                    currencySymbol: currencySymbol,
                    checkAreaCount,
                    areaName: currentViewName,
                    areaNameDetails: areaNameDetails,
                    setViewName: setViewName,
                    unseenNotifications: unseenNotifications,
                    setUnseenNotifications: setUnseenNotifications,
                    planTypeMeta: planTypeMeta,
                    panelErrors,
                    setPanelErrors,
                    repository,
                }}
            >
                <div className={cls.root}>
                    <DashboardHeader
                        open={open}
                        unseenNotifications={unseenNotifications}
                        handleDrawerOpen={handleDrawerOpen}
                        handleHelpDrawerOpen={handleHelpDrawerOpen}
                        helpOpen={helpOpen}
                        title={currentViewName}
                        isLoading={isLoading}
                        dashboardAreasLoading={dashboardAreasLoading}
                    />
                    <Drawer
                        className={classNames(cls.menuDrawer, "sidebar-tour")}
                        variant="persistent"
                        anchor="left"
                        open={open}
                        classes={{
                            paper: cls.menuDrawerPaper,
                        }}
                    >
                        <SideBarHeader handleDrawerClose={handleDrawerClose} />
                        <Divider />
                        <SideBarMenu areaNameDetails={areaNameDetails} />
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
                        <SideBarHeader handleDrawerClose={handleHelpDrawerClose} side="right" roundTop>
                            <Typography className={cls.helpDrawerHeaderText}>Close</Typography>
                        </SideBarHeader>
                        <Divider />
                        {helpComponent}
                    </Drawer>
                    {planTypeMeta && (areaNameDetails.length < planTypeMeta.allowedAreas ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} /> : null)}
                    <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
                </div>
                {successOpen && <PalavyrSnackbar position={snackPosition} successText={successText} successOpen={successOpen} setSuccessOpen={setSuccessOpen} />}
                {warningOpen && <PalavyrSnackbar position={snackPosition} warningText={warningText} warningOpen={warningOpen} setWarningOpen={setWarningOpen} />}
                {errorOpen && <PalavyrSnackbar position={snackPosition} errorText={errorText} errorOpen={errorOpen} setErrorOpen={setErrorOpen} />}
            </DashboardContext.Provider>
        </>
    );
};
