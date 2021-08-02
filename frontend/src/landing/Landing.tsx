import React, { useState, useCallback } from "react";
import { Button, makeStyles, Typography, useTheme } from "@material-ui/core";
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
import { isDevelopmentStage, landingWidgetApiKey, widgetUrl } from "@api-client/clientUtils";
import { IFrame } from "dashboard/content/demo/IFrame";
import { Align } from "dashboard/layouts/positioning/Align";
import { YellowStrip } from "@common/components/YellowStrip";
import { TitleContent } from "./components/TitleContent";
import { BottomStrip } from "./components/footer/BottomStrip";

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
    strip: {
        paddingTop: "3.3rem",
        paddingBottom: "3.3rem",
        paddingLeft: "3rem",
        paddingRight: "3rem",
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
}));

export const LandingPage = () => {
    const cls = useStyles();
    const theme = useTheme();

    const [dialogOpen, setDialogOpen] = useState<DialogTypes>(null);
    const [isCookieRulesDialogOpen, setIsCookieRulesDialogOpen] = useState<boolean>(false);
    const [show, setShow] = useState<boolean>(isDevelopmentStage());

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

    const handleCookieRulesDialogOpen = useCallback(() => {
        setIsCookieRulesDialogOpen(true);
    }, [setIsCookieRulesDialogOpen]);

    const handleCookieRulesDialogClose = useCallback(() => {
        setIsCookieRulesDialogOpen(false);
    }, [setIsCookieRulesDialogOpen]);

    return (
        <div className={cls.wrapper}>
            {!isCookieRulesDialogOpen && <CookieConsent handleCookieRulesDialogOpen={handleCookieRulesDialogOpen} />}
            <LandingPageDialogSelector
                openLoginDialog={openLoginDialog}
                dialogOpen={dialogOpen}
                onClose={closeDialog}
                openTermsDialog={openTermsDialog}
                openRegisterDialog={openRegisterDialog}
                openChangePasswordDialog={openChangePasswordDialog}
            />
            <CookieRules open={isCookieRulesDialogOpen} onClose={handleCookieRulesDialogClose} />
            {!show && <YellowStrip />}
            <DevStagingStrip show={show} setShow={setShow} />
            <Header openRegisterDialog={openRegisterDialog} openLoginDialog={openLoginDialog}>
                <TitleContent
                    title={
                        <Typography align="center" variant="h2" className={cls.primaryText}>
                            Build Your Own Chat Bot
                        </Typography>
                    }
                    subtitle={
                        <Typography display="inline" align="center" variant="h5" className={cls.secondaryText}>
                            No Programing Required
                        </Typography>
                    }
                >
                    <Button className={cls.button} variant="contained" onClick={openRegisterDialog}>
                        <Typography variant="h6">Create a FREE account</Typography>
                    </Button>
                </TitleContent>
            </Header>
            <GreenStrip />
            <div className={cls.body}>
                <TwoItemRow dataList={rowOne} />
                <TwoItemRow dataList={rowTwo} />
                <TwoItemRow dataList={rowThree} />
            </div>
            <GreenStrip />
            <div className={cls.strip} style={{ backgroundColor: theme.palette.primary.main, color: theme.palette.common.white }}>
                <Typography gutterBottom align="center" variant="h2">
                    What is Palavyr?
                </Typography>
                <br></br>
                <Typography gutterBottom align="center">
                    Palavyr is a fully configurable automated chat system used to deliver information about your services and fees to potential customers.
                </Typography>
                <Typography gutterBottom align="center">
                    You craft the chats, configure your pricing strategies, and stylize the widget (which is embedded into your website).
                </Typography>
                <Typography gutterBottom align="center">
                    Potential customers will use the widget to get immediate information about your services and fees.
                </Typography>
            </div>
            <div className={cls.strip} style={{ backgroundColor: theme.palette.primary.main }}>
                <Typography align="center" variant="h3" style={{ color: theme.palette.common.white }}>
                    Try it out!
                </Typography>
                <Align>
                    <IFrame widgetUrl={widgetUrl} apiKey={landingWidgetApiKey} iframeRefreshed={true} preCheckErrors={[]} demo={false} shadow={true} />
                </Align>
            </div>
            <GreenStrip />
            <PricingSection />
            <Sliver />
            <Footer openLoginDialog={openLoginDialog} openRegisterDialog={openRegisterDialog} openTermsDialog={openTermsDialog}/>
            <BottomStrip />
        </div>
    );
};
