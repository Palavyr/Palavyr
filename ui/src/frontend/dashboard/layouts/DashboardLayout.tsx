import React, { useCallback, useEffect, useState } from "react";
import { makeStyles, useTheme, Theme } from "@material-ui/core/styles";
import Drawer from "@material-ui/core/Drawer";
import Divider from "@material-ui/core/Divider";
import IconButton from "@material-ui/core/IconButton";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import { DashboardHeader } from "./header/DashboardHeader";

import { SideBarMenu } from "./sidebar/SideBarMenu";
import { useParams, useHistory } from "react-router-dom";
import { ContentLoader } from "./ContentLoader";
import { AddNewIntentModal } from "./sidebar/AddNewIntentModal";
import { cloneDeep } from "lodash";
import { AlertType, IntentNameDetail, IntentNameDetails, ErrorResponse, PlanTypeMeta, SnackbarPositions } from "@Palavyr-Types";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { defaultUrlForNewIntent, DRAWER_WIDTH, MAIN_CONTENT_DIV_ID, MENU_DRAWER_STATE_COOKIE_NAME, WELCOME_TOUR_COOKIE_NAME } from "@constants";

import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { DashboardContext } from "./DashboardContext";
import { PalavyrSnackbar } from "@common/components/PalavyrSnackbar";
import { redirectToHomeWhenSessionNotEstablished } from "@common/client/clientUtils";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { ApiErrors } from "./Errors/ApiErrors";
import { Loaders } from "@common/client/Loaders";
import { IntroSteps } from "frontend/dashboard/content/welcome/OnboardingTour/IntroSteps";
import { welcomeTourSteps } from "frontend/dashboard/content/welcome/OnboardingTour/tours/welcomeTour";
import Cookies from "js-cookie";
import { GA4ReactResolveInterface } from "ga-4-react/dist/models/gtagModels";
import classNames from "classnames";
import { Typography } from "@material-ui/core";
import { enableBodyScroll } from "body-scroll-lock";
import $ from "jquery";
import { IntentResource, IntentResources } from "@common/types/api/EntityResources";
import { PurchaseTypes } from "@common/types/api/Enums";

const fetchSidebarInfo = (intentData: IntentResources): IntentNameDetails => {
    const intentNameDetails = intentData.map((x: IntentResource) => {
        return {
            intentId: x.intentId,
            intentName: x.intentName,
        };
    });
    return intentNameDetails;
};

type StyleProps = {
    helpOpen: boolean;
};

const useStyles = makeStyles<{}>((theme: any) => ({
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
    toolbar: () => {
        return {
            display: "flex",
            alignItems: "center",
            justifyContent: "flex-end",
            padding: theme.spacing(0, 1),
            // necessary for content to be below app bar
            ...theme.mixins.toolbar,
        };
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
        flexGrow: 1,
    },
    iconButton: {
        borderRadius: "5px",
        width: "10ch",
        "&:hover": {
            backgroundColor: theme.palette.grey[200],
        },
    },
}));

interface IDashboardLayout {
    children: JSX.Element[] | JSX.Element;
    helpComponent: JSX.Element[] | JSX.Element;
    ga4?: GA4ReactResolveInterface;
}

export const DashboardLayout = ({ helpComponent, ga4, children }: IDashboardLayout) => {
    const theme = useTheme();

    const history = useHistory();
    const { intentId } = useParams<{ contentType: string; intentId: string }>();

    const [intentNameDetails, setIntentNameDetails] = useState<IntentNameDetails>([]);
    const [, setLoaded] = useState<boolean>(false);

    const [menuOpen, setMenuOpen] = useState<boolean>(false);
    const [helpOpen, setHelpOpen] = useState<boolean>(false);

    const [modalState, setModalState] = useState<boolean>(false);
    const [currentViewName, setViewName] = useState<string>("");

    const [alertState, setAlertState] = useState<boolean>(false);

    const [currencySymbol, setCurrencySymbol] = useState<string>("");
    const [planTypeMeta, setPlanTypeMeta] = useState<PlanTypeMeta>();

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dashboardIntentsLoading, setDashboardIntentsLoading] = useState<boolean>(false);
    const cls = useStyles({ helpOpen });

    const [panelErrors, setPanelErrors] = useState<ErrorResponse | null>(null);

    const [successOpen, setSuccessOpen] = useState<boolean>(false);
    const [successText, setSuccessText] = useState<string>("Success");

    const [warningOpen, setWarningOpen] = useState<boolean>(false);
    const [warningText, setWarningText] = useState<string>("Warning");

    const [errorOpen, setErrorOpen] = useState<boolean>(false);
    const [errorText, setErrorText] = useState<string>("Error");

    const [snackPosition, setSnackPosition] = useState<SnackbarPositions>("br");

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

    const loadIntents = useCallback(async () => {
        setDashboardIntentsLoading(true);

        const planTypeMeta = await repository.Settings.Subscriptions.GetCurrentPlanMeta();
        setPlanTypeMeta(planTypeMeta);

        const intents = await repository.Intent.GetAllIntents();
        setIntentNameDetails(sortByPropertyAlphabetical((x: IntentNameDetail) => x.intentName, fetchSidebarInfo(intents)));

        const locale = await repository.Settings.Account.GetLocale(true); // readonly == true
        setCurrencySymbol(locale.currentLocale.currencySymbol);

        const numUnseen = await repository.Enquiries.GetEnquiryCount();
        setUnseenNotifications(numUnseen);

        if (intentId) {
            const currentView = intents.filter((x: IntentResource) => x.intentId === intentId).pop();

            if (currentView) {
                setViewName(currentView.intentName);
            }
        }
        setDashboardIntentsLoading(false);
    }, [intentId]);

    useEffect(() => {
        const menuStateString = Cookies.get(MENU_DRAWER_STATE_COOKIE_NAME);
        if (menuStateString !== undefined) {
            if (menuStateString === "true") {
                setMenuOpen(true);
            } else if (menuStateString === "false") {
                setMenuOpen(false);
            }
        }
        const MAIN_DIV = `#${MAIN_CONTENT_DIV_ID}`;
        enableBodyScroll($(MAIN_DIV));
        loadIntents();
        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, [intentId, loadIntents]);

    const setNewIntent = async () => {
        const oldIntentIds = intentNameDetails.map(d => d.intentId);

        const intents = await repository.Intent.GetAllIntents();
        const newInt = intents.filter(x => !oldIntentIds.includes(x.intentId))[0];
        const udpatedIntents = intents.map(i => {
            return { intentName: i.intentName, intentId: i.intentId };
        });

        setIntentNameDetails(udpatedIntents);
        history.push(defaultUrlForNewIntent(newInt.intentId));
    };

    const handleDrawerClose: () => void = () => {
        Cookies.set(MENU_DRAWER_STATE_COOKIE_NAME, "false");
        setMenuOpen(false);
    };

    const handleDrawerOpen: () => void = () => {
        Cookies.set(MENU_DRAWER_STATE_COOKIE_NAME, "true");
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
    const checkIntentCount = () => {
        if (planTypeMeta && intentNameDetails.length >= planTypeMeta.allowedIntents) {
            setAlertState(true);
        } else {
            openModal();
        }
    };

    const alertDetails: AlertType = {
        title: `Maximum intents reached for your current plan (${planTypeMeta ? planTypeMeta.planType : ""})`,
        message:
            planTypeMeta && planTypeMeta.planType === PurchaseTypes.Free
                ? "Thanks for using Palavyr! Please consider purchasing a subscription to increase the number of intents you can provide."
                : "Thanks for using Palavyr! To increase the number of intents you can provide, please consider upgrading your subscription the Manage link in the side bar menu.",
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
            {planTypeMeta && (
                <DashboardContext.Provider
                    value={{
                        intentId,
                        reRenderDashboard,
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
                        checkIntentCount,
                        intentName: currentViewName,
                        intentNameDetails: intentNameDetails,
                        setViewName: setViewName,
                        unseenNotifications: unseenNotifications,
                        setUnseenNotifications: setUnseenNotifications,
                        planTypeMeta: planTypeMeta,
                        panelErrors,
                        setPanelErrors,
                        repository,
                        handleDrawerClose,
                        handleDrawerOpen,
                        menuOpen,
                    }}
                >
                    <div className={cls.root}>
                        <DashboardHeader
                            open={menuOpen}
                            unseenNotifications={unseenNotifications}
                            handleDrawerOpen={handleDrawerOpen}
                            handleHelpDrawerOpen={handleHelpDrawerOpen}
                            helpOpen={helpOpen}
                            title={currentViewName}
                            isLoading={isLoading}
                            dashboardIntentsLoading={dashboardIntentsLoading}
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
                                <IconButton onClick={handleDrawerClose}>
                                    {theme.direction === "rtl" ? <ChevronRightIcon style={{ color: "white" }} /> : <ChevronLeftIcon style={{ color: "white" }} />}
                                </IconButton>
                            </div>
                            <Divider />
                            <SideBarMenu intentNameDetails={intentNameDetails} menuOpen={menuOpen} />
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
                            <IconButton className={cls.iconButton} onClick={handleHelpDrawerClose}>
                                {theme.direction === "rtl" ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                            </IconButton>
                            <Divider />
                            {helpComponent}
                        </Drawer>
                        {planTypeMeta && (intentNameDetails.length < planTypeMeta.allowedIntents ? <AddNewIntentModal open={modalState} handleClose={closeModal} setNewIntent={setNewIntent} /> : null)}
                        <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />
                        {successOpen && <PalavyrSnackbar position={snackPosition} successText={successText} successOpen={successOpen} setSuccessOpen={setSuccessOpen} />}
                        {warningOpen && <PalavyrSnackbar position={snackPosition} warningText={warningText} warningOpen={warningOpen} setWarningOpen={setWarningOpen} />}
                        {errorOpen && <PalavyrSnackbar position={snackPosition} errorText={errorText} errorOpen={errorOpen} setErrorOpen={setErrorOpen} />}
                    </div>
                </DashboardContext.Provider>
            )}
        </>
    );
};
