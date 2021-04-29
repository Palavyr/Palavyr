import React, { useState, useCallback } from "react";
import { Divider, makeStyles } from "@material-ui/core";
import { CookieConsent } from "legal/cookies/CookieConsent";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { CookieRules } from "legal/cookies/CookieRules";
import { Header } from "./components/header/Header";
import { Footer } from "./components/footer/Footer";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";
import { TwoItemRow } from "./components/TwoItemRow/TwoItemRow";
import { PricingSection } from "./components/pricing/PricingSection";
import { rowOne, rowTwo, rowThree } from "./components/landingContent/twoItemRowContent";

import AOS from "aos";
import { Sliver } from "./components/sliver/Sliver";
import { CHANGE_PASSWORD, REGISTER, TERMS_OF_SERVICE } from "@constants";
import { DevStagingStrip } from "@common/components/devIndicators/DevStagingStrip";
import { GreenStrip } from "./components/sliver/ThinStrip";
AOS.init({
    duration: 1000,
});

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
    body: {
        background: theme.palette.background.default,
    },
}));

export const LandingPage = () => {
    const classes = useStyles();

    const [selectedTab, setSelectedTab] = useState<string | null>(null);
    const [isMobileDrawerOpen, setIsMobileDrawerOpen] = useState<boolean>(false);
    const [dialogOpen, setDialogOpen] = useState<DialogTypes>(null);
    const [isCookieRulesDialogOpen, setIsCookieRulesDialogOpen] = useState<boolean>(false);

    const openLoginDialog = useCallback(() => {
        setDialogOpen("login");
        setIsMobileDrawerOpen(false);
    }, [setDialogOpen, setIsMobileDrawerOpen]);

    const closeDialog = useCallback(() => {
        setDialogOpen(null);
    }, [setDialogOpen]);

    const openRegisterDialog = useCallback(() => {
        setDialogOpen(REGISTER);
        setIsMobileDrawerOpen(false);
    }, [setDialogOpen, setIsMobileDrawerOpen]);

    const handleMobileDrawerOpen = useCallback(() => {
        setIsMobileDrawerOpen(true);
    }, [setIsMobileDrawerOpen]);

    const handleMobileDrawerClose = useCallback(() => {
        setIsMobileDrawerOpen(false);
    }, [setIsMobileDrawerOpen]);

    const openChangePasswordDialog = useCallback(() => {
        setDialogOpen(CHANGE_PASSWORD);
    }, [setDialogOpen]);

    const openTermsDialog = useCallback(() => {
        setDialogOpen(TERMS_OF_SERVICE);
    }, [setDialogOpen]);

    const handleCookieRulesDialogOpen = useCallback(() => {
        setIsCookieRulesDialogOpen(true);
    }, [setIsCookieRulesDialogOpen]);

    const handleCookieRulesDialogClose = useCallback(() => {
        setIsCookieRulesDialogOpen(false);
    }, [setIsCookieRulesDialogOpen]);

    return (
        <div className={classes.wrapper}>
            {!isCookieRulesDialogOpen && <CookieConsent handleCookieRulesDialogOpen={handleCookieRulesDialogOpen} />}
            <LandingPageDialogSelector openLoginDialog={openLoginDialog} dialogOpen={dialogOpen} onClose={closeDialog} openTermsDialog={openTermsDialog} openRegisterDialog={openRegisterDialog} openChangePasswordDialog={openChangePasswordDialog} />
            <CookieRules open={isCookieRulesDialogOpen} onClose={handleCookieRulesDialogClose} />
            <DevStagingStrip />
            <Header
                openRegisterDialog={openRegisterDialog}
                openLoginDialog={openLoginDialog}
                handleMobileDrawerOpen={handleMobileDrawerOpen}
                handleMobileDrawerClose={handleMobileDrawerClose}
                isMobileDrawerOpen={isMobileDrawerOpen}
                selectedTab={selectedTab}
                setSelectedTab={setSelectedTab}
            />
            <GreenStrip />
            <div className={classes.body}>
                <TwoItemRow dataList={rowOne} />
                <TwoItemRow dataList={rowTwo} />
                <TwoItemRow dataList={rowThree} />
            </div>
            <GreenStrip />
            <PricingSection />
            <Sliver />
            <Footer />
        </div>
    );
};
