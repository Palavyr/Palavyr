import React from "react";
import { makeStyles, Typography, useTheme } from "@material-ui/core";
import { TwoItemRow } from "./components/TwoItemRow/TwoItemRow";
import { PricingSection } from "./components/pricing/PricingSection";
import { rowOne, rowTwo, rowThree } from "./components/landingContent/twoItemRowContent";
import AOS from "aos";
import { GreenStrip } from "./components/sliver/ThinStrip";
import { landingWidgetApiKey, widgetUrl } from "@common/client/clientUtils";
import { IFrame } from "frontend/dashboard/content/demo/IFrame";
import { Align } from "@common/positioning/Align";
import { LandingWrapper } from "./components/LandingWrapper";
import { LangingPageTitleContent } from "./branding/headerTitleContent/LandingPageTitleContent";
import { LineSpacer } from "@common/components/typography/LineSpacer";
import { ComponentLandingSpotlight, LandingSpotlight } from "./components/ConversationDesignerCallout/ConversationDesignerCallout";
import LandingImageOne from "./landingImages/editor-1.gif";
import PricingImageOne from "./landingImages/pricing-1.gif";
import DesignerOne from "./landingImages/designer-2.gif";
import FineControlOne from "./landingImages/finecontrol-1.gif";

AOS.init({
    duration: 1000,
});

const useStyles = makeStyles(theme => ({
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
    borderClip: {
        borderRadius: "50%",
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
                    {/* <div className={cls.body}>
                        <TwoItemRow dataList={rowOne} />
                        <TwoItemRow dataList={rowTwo} />
                        <TwoItemRow dataList={rowThree} />
                    </div> */}
                    {/* <GreenStrip /> */}
                    {/* <div className={cls.strip} style={{ backgroundColor: theme.palette.primary.main, color: theme.palette.common.white }}>
                        <Typography gutterBottom align="center" variant="h2">
                            Elegant
                        </Typography>
                        <br></br>
                        <Typography variant="h5" gutterBottom align="center">
                            Palavyr is a no-code chat bot building platform used to build chatbots that say what you want them to say.
                        </Typography>
                        <LineSpacer numLines={1} />
                        <Typography variant="h5" gutterBottom align="center">
                            You craft the chats, configure your pricing strategies, and stylize the widget (which is embedded into your website).
                        </Typography>
                    </div>
                    <GreenStrip /> */}
                    <ComponentLandingSpotlight text="Elegent and Classic">
                        {/* <div className={cls.strip} style={{ backgroundColor: theme.palette.primary.light }}> */}
                            {/* <Typography align="center" variant="h3" style={{ color: theme.palette.common.white }}>
                                Try it out!
                            </Typography> */}
                            <Align>
                                <IFrame widgetUrl={widgetUrl} apiKey={landingWidgetApiKey} iframeRefreshed={true} preCheckErrors={[]} demo={false} shadow={true} />
                            </Align>
                        {/* </div>   */}
                    </ComponentLandingSpotlight>
                    <GreenStrip />

                    <LandingSpotlight text="Intuitive Conversation Design" imgSrc={LandingImageOne} />
                    <GreenStrip />
                    <LandingSpotlight text="Transparent Pricing Strategies" imgSrc={PricingImageOne} />
                    <GreenStrip />
                    <LandingSpotlight text="Brand Customization" imgSrc={DesignerOne} />
                    <GreenStrip />
                    <LandingSpotlight text="Fine Grain Control" imgSrc={FineControlOne} className={cls.borderClip} />
                    <GreenStrip />
                    <ComponentLandingSpotlight text="Reasonably Priced">
                        <PricingSection />
                    </ComponentLandingSpotlight>
                </>
            }
        />
    );
};