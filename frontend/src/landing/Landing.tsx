import React from "react";
import { makeStyles, Typography, useTheme } from "@material-ui/core";
import { TwoItemRow } from "./components/TwoItemRow/TwoItemRow";
import { PricingSection } from "./components/pricing/PricingSection";
import { rowOne, rowTwo, rowThree } from "./components/landingContent/twoItemRowContent";
import AOS from "aos";
import { GreenStrip } from "./components/sliver/ThinStrip";
import { landingWidgetApiKey, widgetUrl } from "@api-client/clientUtils";
import { IFrame } from "dashboard/content/demo/IFrame";
import { Align } from "dashboard/layouts/positioning/Align";
import { LandingWrapper } from "./components/LandingWrapper";
import { LangingPageTitleContent } from "./branding/headerTitleContent/LandingPageTitleContent";

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

    return (
        <LandingWrapper
            TitleContent={<LangingPageTitleContent />}
            MainContent={
                <>
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
                            Potential customers can then use the widget to get immediate information about your services and fees.
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
                </>
            }
        />
    );
};
