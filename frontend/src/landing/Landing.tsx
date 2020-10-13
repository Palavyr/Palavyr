import React, { useState, useCallback, useEffect } from "react";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import { CookieConsent } from "legal/cookies/CookieConsent";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { CookieRules } from "legal/cookies/CookieRules";
import { Header } from "./components/header/Header";
import { Footer } from "./components/footer/Footer";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";
import Auth from "auth/Auth";
import { useHistory } from "react-router-dom";
import { ItemRowObject, TwoItemRow } from "./components/TwoItemRow/TwoItemRow";
import { PricingSection } from "./components/pricing/PricingSection";

const useStyles = makeStyles((theme) => ({
    wrapper: {
        backgroundColor: theme.palette.common.white,
        overflowX: "hidden",
    },
    frame: {
        position: "static",
        right: "40px",
        height: "500px",
        width: "320px",
        borderRadius: "9px",
        border: "0px",
    },
    sliver: {
        fontSize: "21pt"
    },
    body: {
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)"
    }
}));


const rowOne: Array<ItemRowObject> = [
    {
        title: "Engage potential clients",
        text: "Collect all of the information that you need to engage, sort, and secure a potential client.",
        type: "pencil",
        color: "navy"

    },
    {
        title: "Direct Fee Estimate",
        text: "Deliver a competitive fee estimate and convince your prospective clients to sign.",
        type: "calculator",
        color: "navy"
    }
]
const rowTwo: Array<ItemRowObject> = [
    {
        title: "Simple Integration",
        text: "Integrate the Palavyr.com widget into your site with as little a single line of code.",
        type: "star",
        color: "navy"

    },
    {
        title: "Persuade clients to sign",
        text: "Anticipate your client's needs ahead of time and offer a competetive fee estimate.",
        type: "check",
        color: "navy"
    }
]
const rowThree: Array<ItemRowObject> = [
    {
        title: "Be Transparent",
        text: "Automate detail collection using the fully customizable Palavyr chat widget and provide transparency for your future clients.",
        type: "rocket",
        color: "navy"
    },
    {
        title: "Cut to the Chase",
        text: "Collect inquiry details and use the equiry dashboard to preemptively learn what your clients are asking about.",
        type: "docs",
        color: "navy"
    }
]


export const LandingPage = () => {
    const classes = useStyles();

    const [selectedTab, setSelectedTab] = useState<string | null>(null);
    const [isMobileDrawerOpen, setIsMobileDrawerOpen] = useState<boolean>(false);
    const [dialogOpen, setDialogOpen] = useState<DialogTypes>(null);
    const [isCookieRulesDialogOpen, setIsCookieRulesDialogOpen] = useState<boolean>(false);

    const history = useHistory();

    const openLoginDialog = useCallback(() => {
        setDialogOpen("login");
        setIsMobileDrawerOpen(false);
    }, [setDialogOpen, setIsMobileDrawerOpen]);


    const closeDialog = useCallback(() => {
        setDialogOpen(null);
    }, [setDialogOpen]);


    const openRegisterDialog = useCallback(() => {
        setDialogOpen("register");
        setIsMobileDrawerOpen(false);
    }, [setDialogOpen, setIsMobileDrawerOpen]);

    const handleMobileDrawerOpen = useCallback(() => {
        setIsMobileDrawerOpen(true);
    }, [setIsMobileDrawerOpen]);

    const handleMobileDrawerClose = useCallback(() => {
        setIsMobileDrawerOpen(false);
    }, [setIsMobileDrawerOpen]);

    const openChangePasswordDialog = useCallback(() => {
        setDialogOpen("changePassword");
    }, [setDialogOpen]);

    const openTermsDialog = useCallback(() => {
        setDialogOpen("termsOfService");
    }, [setDialogOpen]);

    const handleCookieRulesDialogOpen = useCallback(() => {
        setIsCookieRulesDialogOpen(true);
    }, [setIsCookieRulesDialogOpen]);

    const handleCookieRulesDialogClose = useCallback(() => {
        setIsCookieRulesDialogOpen(false);
    }, [setIsCookieRulesDialogOpen]);

    const attemptLogin = useCallback(async () => {
        const success = () => {
            setTimeout(() => {
                history.push("/dashboard");
            }, 150);
        }
        var response = await Auth.loginWithSessionToken(success, () => { });
        return false;
    }, [])

    useEffect(() => {
        attemptLogin();
        return () => {
        }
    }, [])

    return (
        <div className={classes.wrapper}>
            {!isCookieRulesDialogOpen && (
                <CookieConsent
                    handleCookieRulesDialogOpen={handleCookieRulesDialogOpen}
                />
            )}
            <LandingPageDialogSelector
                openLoginDialog={openLoginDialog}
                dialogOpen={dialogOpen}
                onClose={closeDialog}
                openTermsDialog={openTermsDialog}
                openRegisterDialog={openRegisterDialog}
                openChangePasswordDialog={openChangePasswordDialog}
            />
            <CookieRules
                open={isCookieRulesDialogOpen}
                onClose={handleCookieRulesDialogClose}
            />
            <Header
                openRegisterDialog={openRegisterDialog}
                openLoginDialog={openLoginDialog}
                handleMobileDrawerOpen={handleMobileDrawerOpen}
                handleMobileDrawerClose={handleMobileDrawerClose}
                isMobileDrawerOpen={isMobileDrawerOpen}
                selectedTab={selectedTab}
                setSelectedTab={setSelectedTab}
            />
            <Divider />
            <PricingSection />
            <div className={classes.body}>
                <TwoItemRow dataList={rowOne} />
                <TwoItemRow dataList={rowTwo} />
                <TwoItemRow dataList={rowThree} />
            </div>

            <div style={{ color: "lighgray", textAlign: "center", height: "2.8rem", width: "100%", backgroundColor: "gray" }}>
                <Typography className={classes.sliver}>
                    Questions? Get in touch: info.palavyr@gmail.com
                </Typography>
            </div>

            <Footer />
        </div>

    );
};

