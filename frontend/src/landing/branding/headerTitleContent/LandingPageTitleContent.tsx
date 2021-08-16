import { TitleContent } from "@landing/components/TitleContent";
import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { SubtitleTypography } from "./components/SubtitleTypography";
import { TitleTypography } from "./components/TitleTypography";
import Fade from "react-reveal/Fade";
import BorrowedCarousel from "common/components/carousel/carousel";

const useStyles = makeStyles(theme => ({
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

export interface LandingPageTitleContentProps {}

export const LangingPageTitleContent = ({  }: LandingPageTitleContentProps) => {
    const items = [
        <Fade bottom>
            <TitleTypography>Chat Bot</TitleTypography>
        </Fade>,
        <Fade bottom>
            <TitleTypography>Personal Assistant</TitleTypography>
        </Fade>,
        <Fade bottom>
            <TitleTypography>Engagement Tool</TitleTypography>
        </Fade>,
        <Fade bottom>
            <TitleTypography>Automatic Lead Generator</TitleTypography>
        </Fade>,
    ];

    return (
        <Fade>
            <TitleContent
                title={
                    <>
                        <TitleTypography>Build Your own </TitleTypography>
                        <BorrowedCarousel timeout={1050}>{items.map(x => x)}</BorrowedCarousel>
                    </>
                }
                subtitle={<SubtitleTypography>Simple. No programing required</SubtitleTypography>}
            ></TitleContent>
            ;
        </Fade>
    );
};
