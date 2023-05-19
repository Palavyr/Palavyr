import React, { useState, useCallback, useEffect } from "react";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { Header } from "@landing/components/header/Header";
import { makeStyles } from "@material-ui/core";
import { CHANGE_PASSWORD, LOGIN, PRIVACY_POLICY, REGISTER, TERMS_OF_SERVICE } from "@constants";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";
import { YellowStrip } from "@common/components/YellowStrip";
import { useLocation } from "react-router-dom";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    wrapper: {
        backgroundColor: theme.palette.common.white,
        overflowX: "hidden",
        height: "100%",
    },
}));

// Refactor this as well to use pages. This is so silly. Works, but yeah.
export const LandingPage = () => {
    const cls = useStyles();

    const location = useLocation();

    useEffect(() => {
        if (location.pathname === "/") {
            setDialogOpen(LOGIN);
        } else if (location.pathname === "/login") {
            setDialogOpen(LOGIN);
        } else if (location.pathname === "/signup") {
            setDialogOpen(REGISTER);
        }
    }, []);

    const [dialogOpen, setDialogOpen] = useState<DialogTypes>(null);
    const [isMobileDrawerOpen, setIsMobileDrawerOpen] = useState(false);

    const openLoginDialog = useCallback(() => {
        setDialogOpen("login");
    }, [setDialogOpen]);

    const closeDialog = useCallback(() => {
        setDialogOpen(null);
    }, [setDialogOpen]);

    const openRegisterDialog = useCallback(() => {
        setDialogOpen(REGISTER);
    }, [setDialogOpen]);

    const openChangePasswordDialog = useCallback(() => {
        setDialogOpen(CHANGE_PASSWORD);
    }, [setDialogOpen]);

    const openTermsDialog = useCallback(() => {
        setDialogOpen(TERMS_OF_SERVICE);
    }, [setDialogOpen]);

    const openPrivacyDialog = useCallback(() => {
        setDialogOpen(PRIVACY_POLICY);
    }, [setDialogOpen]);

    const handleMobileDrawerOpen = useCallback(() => {
        setIsMobileDrawerOpen(true);
    }, [setIsMobileDrawerOpen]);

    const handleMobileDrawerClose = useCallback(() => {
        setIsMobileDrawerOpen(false);
    }, [setIsMobileDrawerOpen]);

    return (
        <div className={cls.wrapper}>
            <YellowStrip />
            <Header
                openRegisterDialog={openRegisterDialog}
                openLoginDialog={openLoginDialog}
                handleMobileDrawerOpen={handleMobileDrawerOpen}
                handleMobileDrawerClose={handleMobileDrawerClose}
                mobileDrawerOpen={isMobileDrawerOpen}
            />
            <YellowStrip />
            <div style={{ height: "100%", margin: "0 auto", flexGrow: 1, maxWidth: "424px", marginTop: "2rem", marginBottom: "2rem" }}>
                <LandingPageDialogSelector
                    openLoginDialog={openLoginDialog}
                    dialogOpen={dialogOpen}
                    onClose={closeDialog}
                    openTermsDialog={openTermsDialog}
                    openPrivacyDialog={openPrivacyDialog}
                    openRegisterDialog={openRegisterDialog}
                    openChangePasswordDialog={openChangePasswordDialog}
                />
            </div>
        </div>
    );
};
