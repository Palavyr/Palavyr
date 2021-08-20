import React, { useCallback, useEffect, useState } from "react";
import { makeStyles, useTheme, Theme } from "@material-ui/core/styles";
import Drawer from "@material-ui/core/Drawer";
import CssBaseline from "@material-ui/core/CssBaseline";
import Divider from "@material-ui/core/Divider";
import IconButton from "@material-ui/core/IconButton";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import { DashboardHeader } from "./header/DashboardHeader";

import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory, useLocation } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewAreaModal } from "./sidebar/AddNewAreaModal";
import { cloneDeep } from "lodash";
import { AlertType, AreaNameDetail, AreaNameDetails, Areas, AreaTable, EnquiryRow, ErrorResponse, PlanTypeMeta, PurchaseTypes, SnackbarPositions } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { defaultUrlForNewArea, DRAWER_WIDTH, MAIN_CONTENT_DIV_ID, MENU_DRAWER_STATE_COOKIE_NAME, WELCOME_TOUR_COOKIE_NAME } from "@constants";

import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { DashboardContext } from "./DashboardContext";
import { PalavyrSnackbar } from "@common/components/PalavyrSnackbar";
import { redirectToHomeWhenSessionNotEstablished } from "@api-client/clientUtils";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { ApiErrors } from "./Errors/ApiErrors";
import { Loaders } from "@api-client/Loaders";
import { IntroSteps } from "dashboard/content/welcome/OnboardingTour/IntroSteps";
import { welcomeTourSteps } from "dashboard/content/welcome/OnboardingTour/tours/welcomeTour";
import Cookies from "js-cookie";
import { GA4ReactResolveInterface } from "ga-4-react/dist/models/gtagModels";
import classNames from "classnames";
import { Typography } from "@material-ui/core";
import { enableBodyScroll } from "body-scroll-lock";
import $ from "jquery";

const fetchSidebarInfo = (areaData: Areas): AreaNameDetails => {
    const areaNameDetails = areaData.map((x: AreaTable) => {
        return {
            areaIdentifier: x.areaIdentifier,
            areaName: x.areaName,
        };
    });
    return areaNameDetails;
};

type StyleProps = {
    helpOpen: boolean;
};

const useStyles = makeStyles((theme: Theme) =>({

    root: {
        display: "flex",

    },
    menuButton: {
        marginRight: 36,
    },
    hide: {
        display: "none",
    },
    drawer: {
        width: DRAWER_WIDTH,
        flexShrink: 0,
        whiteSpace: "nowrap",
        backgroundColor: theme.palette.primary.main,
    },
    drawerOpen: {
        width: DRAWER_WIDTH,
        transition: theme.transitions.create("width", {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    drawerClose: {
        transition: theme.transitions.create("width", {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        overflowX: "hidden",
        width: theme.spacing(7) + 1,
        [theme.breakpoints.up("sm")]: {
            width: theme.spacing(9) + 1,
        },
    },
    toolbar: {
        display: "flex",
        alignItems: "center",
        justifyContent: "flex-end",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        ...theme.mixins.toolbar,
    },

    content: {
        flexGrow: 1,
        padding: theme.spacing(3),
    },
    menuDrawer: {
        width: DRAWER_WIDTH,
        flexShrink: 0,
    },
    helpDrawer: (props: StyleProps) => {
        return {
            zIndex: 99999,
            width: props.helpOpen ? DRAWER_WIDTH + 300 : 0,
            flexShrink: 0,
        };
    },
    menuDrawerPaper: {
        width: DRAWER_WIDTH,
        backgroundColor: theme.palette.primary.main,
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
    name: {
        color: theme.palette.success.main,
    },
    drawerFiller: {
        backgroundColor: theme.palette.primary.main,
        flexGrow: 1
    }
}));

interface IDashboardLayout {
    children: JSX.Element[] | JSX.Element;
    helpComponent: JSX.Element[] | JSX.Element;
    ga4?: GA4ReactResolveInterface;
}

export const DashboardLayout = ({ helpComponent, ga4, children }: IDashboardLayout) => {

    const theme = useTheme();

    const history = useHistory();
    const location = useLocation();
    const { areaIdentifier } = useParams<{ contentType: string; areaIdentifier: string }>();

    const [areaNameDetails, setAreaNameDetails] = useState<AreaNameDetails>([]);
    const [, setLoaded] = useState<boolean>(false);

    const [menuOpen, setMenuOpen] = useState<boolean>(false);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>("");

    const [alertState, setAlertState] = useState<boolean>(false);

    const [currencySymbol, setCurrencySymbol] = useState<string>("");
    const [planTypeMeta, setPlanTypeMeta] = useState<PlanTypeMeta>();

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dashboardAreasLoading, setDashboardAreasLoading] = useState<boolean>(false);
    const cls = useStyles({ helpOpen });

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

    const [genericRerender, setGenericRerender] = useState<boolean>(true);

    const loaders = new Loaders(setIsLoading);
    const apiErrors = new ApiErrors(setSuccessOpen, setSuccessText, setWarningOpen, setWarningText, setErrorOpen, setErrorText, setPanelErrors);
    const repository = new PalavyrRepository(apiErrors, loaders);

    const reRenderDashboard = () => {
        setGenericRerender(!genericRerender);
    };

    useEffect(() => {
        (async () => {
            await redirectToHomeWhenSessionNotEstablished(history, repository);
        })();
    }, []);

    const loadAreas = useCallback(async () => {
        ga4?.pageview(location.pathname);
        setDashboardAreasLoading(true);

        const planTypeMeta = await repository.Settings.Subscriptions.getCurrentPlanMeta();
        setPlanTypeMeta(planTypeMeta);

        const areas = await repository.Area.GetAreas();
        setAreaNameDetails(sortByPropertyAlphabetical((x: AreaNameDetail) => x.areaName, fetchSidebarInfo(areas)));

        const locale = await repository.Settings.Account.GetLocale();
        setCurrencySymbol(locale.currentLocale.currencySymbol);

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
        const menuStateString = Cookies.get(MENU_DRAWER_STATE_COOKIE_NAME)
        if (menuStateString !== undefined){
            if (menuStateString === "true"){
                setMenuOpen(true);
            } else if (menuStateString === "false") {
                setMenuOpen(false);
            }
        }
        const MAIN_DIV = `#${MAIN_CONTENT_DIV_ID}`;
        enableBodyScroll($(MAIN_DIV));
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
        Cookies.set(MENU_DRAWER_STATE_COOKIE_NAME, "false")
        setMenuOpen(false);
    };

    const handleDrawerOpen: () => void = () => {
        Cookies.set(MENU_DRAWER_STATE_COOKIE_NAME, "true")
        setMenuOpen(true);
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

    const [welcomeTourActive, setWelcomeTourActive] = useState<boolean>(false);

    useEffect(() => {
        if (Cookies.get(WELCOME_TOUR_COOKIE_NAME) === undefined) {
            setWelcomeTourActive(true);
        }
    }, []);

    const welcomeTourOnBlur = () => {
        Cookies.set(WELCOME_TOUR_COOKIE_NAME, "", {
            expires: 9999,
        });
    };

    return (
    <>
        {welcomeTourActive && <IntroSteps initialize={welcomeTourActive} steps={welcomeTourSteps} onBlur={welcomeTourOnBlur} />}
        <DashboardContext.Provider
            value={{
                reRenderDashboard,
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
                handleDrawerClose,
                handleDrawerOpen,
                menuOpen
            }}
        >
            <div className={cls.root}>
                <CssBaseline />
                <DashboardHeader
                    open={menuOpen}
                    unseenNotifications={unseenNotifications}
                    handleDrawerOpen={handleDrawerOpen}
                    handleHelpDrawerOpen={handleHelpDrawerOpen}
                    helpOpen={helpOpen}
                    title={currentViewName}
                    isLoading={isLoading}
                    dashboardAreasLoading={dashboardAreasLoading}
                />
                <Drawer
                    variant="permanent"
                    className={classNames(cls.drawer, {
                        [cls.drawerOpen]: menuOpen,
                        [cls.drawerClose]: !menuOpen,
                    })}
                    classes={{
                        paper: classNames({
                            [cls.drawerOpen]: menuOpen,
                            [cls.drawerClose]: !menuOpen,
                        }),
                    }}
                >
                <div className={classNames(cls.toolbar, cls.menuDrawerPaper)}>
                    <Typography className={cls.name} variant="h4">
                        Palavyr.com
                    </Typography>
                    <IconButton onClick={handleDrawerClose}>{theme.direction === "rtl" ? <ChevronRightIcon style={{color: "white"}} /> : <ChevronLeftIcon style={{color: "white"}}  />}</IconButton>
                </div>
                <Divider />
                <SideBarMenu areaNameDetails={areaNameDetails} menuOpen={menuOpen} />
                <div className={cls.drawerFiller}></div>
                </Drawer>
                <ContentLoader open={menuOpen}>{children}</ContentLoader>
                <Drawer
                    className={cls.helpDrawer}
                    variant="persistent"
                    anchor="right"
                    open={helpOpen}
                    classes={{
                        paper: cls.helpDrawerPaper,
                    }}
                >
                    <IconButton onClick={handleHelpDrawerClose}>{theme.direction === "rtl" ? <ChevronLeftIcon /> : <ChevronRightIcon />}</IconButton>
                    <Divider />
                    {helpComponent}
                </Drawer>
                {planTypeMeta && (areaNameDetails.length < planTypeMeta.allowedAreas ? <AddNewAreaModal open={modalState} handleClose={closeModal} setNewArea={setNewArea} /> : null)}
                <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
                {successOpen && <PalavyrSnackbar position={snackPosition} successText={successText} successOpen={successOpen} setSuccessOpen={setSuccessOpen} />}
                {warningOpen && <PalavyrSnackbar position={snackPosition} warningText={warningText} warningOpen={warningOpen} setWarningOpen={setWarningOpen} />}
                {errorOpen && <PalavyrSnackbar position={snackPosition} errorText={errorText} errorOpen={errorOpen} setErrorOpen={setErrorOpen} />}
            </div>
        </DashboardContext.Provider>
    </>
    )
};
