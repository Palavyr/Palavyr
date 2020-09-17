import React, { useState, useCallback, useEffect } from "react";
import { makeStyles } from "@material-ui/core";
import { CookieConsent } from "legal/cookies/CookieConsent";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { CookieRules } from "legal/cookies/CookieRules";
import { NavBar } from "./components/navbar/NavBar";
import { Header } from "./components/header/Header";
import { ThreeItemRow, ThreeItemRowObject } from "./components/ThreeItemRow/ThreeItemRow";
import { Footer } from "./components/footer/Footer";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";


const useStyles = makeStyles((theme) => ({
    wrapper: {
        backgroundColor: theme.palette.common.white,
        overflowX: "hidden",
    },
    frame: {
        height: "500px",
        width: "320px",
        borderRadius: "9px",
        border: "0px",
        position: "fixed",
        zIndex: 999
    }
}));


const listOfThree: Array<ThreeItemRowObject> = [
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

    const testCallback = useCallback(async () => {
        console.log("Attempting reach the server...")

    }, [])


    const attemptLogin = () => {
        return false; // TODO: implement this with Auth.ts
    }

    useEffect(() => {
        testCallback()
        attemptLogin();
    })

    return (
        <>
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
                <NavBar
                    openRegisterDialog={openRegisterDialog}
                    openLoginDialog={openLoginDialog}
                    handleMobileDrawerOpen={handleMobileDrawerOpen}
                    handleMobileDrawerClose={handleMobileDrawerClose}
                    mobileDrawerOpen={isMobileDrawerOpen}
                    selectedTab={selectedTab}
                    selectTab={setSelectedTab}
                />
                <Header />
                <ThreeItemRow listOfThree={listOfThree} />
                <Footer />
            </div>
            {/* <div> */}
                {/* <iframe className={classes.frame} title="demo" src="http://localhost:3400/widget/abc123"></iframe> */}
            {/* </div> */}
        </>
    );
};

