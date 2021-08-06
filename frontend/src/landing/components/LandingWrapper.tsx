import React, { useState, useCallback, useEffect } from "react";
import { LandingPageDialogSelector } from "@landing/components/dialogSelector/LandingPageDialogSelector";
import { Header } from "@landing/components/header/Header";
import { GreenStrip } from "@landing/components/sliver/ThinStrip";
import { TitleContent } from "@landing/components/TitleContent";
import { Card, makeStyles, Typography } from "@material-ui/core";
import { CHANGE_PASSWORD, REGISTER, TERMS_OF_SERVICE } from "@constants";
import { DialogTypes } from "@landing/components/dialogSelector/dialogTypes";
import { YellowStrip } from "@common/components/YellowStrip";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Footer } from "@landing/components/footer/Footer";
import { Sliver } from "@landing/components/sliver/Sliver";
import { VideoMap } from "@Palavyr-Types";
import { BottomStrip } from "@landing/components/footer/BottomStrip";

const useStyles = makeStyles((theme) => ({
    wrapper: {
        backgroundColor: theme.palette.common.white,
        overflowX: "hidden",
    },
    primaryText: {
        color: theme.palette.success.main,
    },
    secondaryText: {
        color: theme.palette.success.dark,
    },
    button: {
        width: "18rem",
        alignSelf: "center",
        backgroundColor: theme.palette.background.default,
        color: theme.palette.common.black,
        "&:hover": {
            backgroundColor: theme.palette.success.light,
            color: theme.palette.common.black,
        },
    },
    contentPadding: {
        paddingTop: "2rem",
        paddingBottom: "3rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    media: {
        width: "825px",
        height: "508px",
        boxShadow: theme.shadows[10],
    },
    mediaSpan: {
        display: "flex",
        justifyContent: "center",
    },
}));

export interface ILandingWrapper {
    TitleContent: React.ReactNode;
    MainContent: React.ReactNode;
}

export const LandingWrapper = ({ TitleContent, MainContent }: ILandingWrapper) => {
    const cls = useStyles();

    const [dialogOpen, setDialogOpen] = useState<DialogTypes>(null);

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

    return (
        <div className={cls.wrapper}>
            <LandingPageDialogSelector
                openLoginDialog={openLoginDialog}
                dialogOpen={dialogOpen}
                onClose={closeDialog}
                openTermsDialog={openTermsDialog}
                openRegisterDialog={openRegisterDialog}
                openChangePasswordDialog={openChangePasswordDialog}
            />
            <YellowStrip />
            <Header openRegisterDialog={openRegisterDialog} openLoginDialog={openLoginDialog}>
                {TitleContent}
                {/* <TitleContent
                    title={
                        <Typography align="center" variant="h2" className={cls.primaryText}>
                            Palavyr Getting Started Tutorial series
                        </Typography>
                    }
                    subtitle={
                        <Typography align="center" variant="h6" className={cls.secondaryText}>
                            We are adding more tutorials regularly, so subscribe to our channel to receive updates!
                        </Typography>
                    }
                /> */}
            </Header>
            <GreenStrip />
            {MainContent}
            <Sliver />
            <Footer openLoginDialog={openLoginDialog} openRegisterDialog={openRegisterDialog} openTermsDialog={openTermsDialog} />
            <BottomStrip />
        </div>
    );
};