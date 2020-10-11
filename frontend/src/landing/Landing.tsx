import React, { useState, useCallback, useEffect } from "react";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import { CookieConsent } from "legal/cookies/CookieConsent";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { CookieRules } from "legal/cookies/CookieRules";
import { Header } from "./components/header/Header";
import { ThreeItemRow, ItemRowObject } from "./components/ThreeItemRow/ThreeItemRow";
import { Footer } from "./components/footer/Footer";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";
import Auth from "auth/Auth";
import { useHistory } from "react-router-dom";
import { TwoItemRow } from "./components/TwoItemRow/TwoItemRow";
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


const listOfThree: Array<ItemRowObject> = [
    {
        title: "Engage potential clients",
        text: "Collect all of the information that you need to engage, sort, and secure a potential client.",
        type: "pencil",
    },
    {
        title: "Direct Fee Estimate",
        text: "Deliver a competitive fee estimate and convince your prospective clients to sign.",
        type: "calculator",
    },
    {
        title: "Persuade clients to sign",
        text: "Anticipate your client's needs ahead of time and offer a competetive fee estimate.",
        type: "check",
    }
]

const listOfTwo: Array<ItemRowObject> = [
    {
        title: "Engage potential clients",
        text: "Collect all of the information that you need to engage, sort, and secure a potential client.",
        type: "pencil",
    },
    {
        title: "Direct Fee Estimate",
        text: "Deliver a competitive fee estimate and convince your prospective clients to sign.",
        type: "calculator",
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
                <TwoItemRow listOfTwo={listOfTwo} />
                <TwoItemRow listOfTwo={listOfTwo} />
                <TwoItemRow listOfTwo={listOfTwo} />
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

