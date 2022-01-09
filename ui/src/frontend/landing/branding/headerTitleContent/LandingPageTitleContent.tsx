import { TitleContent } from "@landing/components/TitleContent";
import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { SubtitleTypography } from "./components/SubtitleTypography";
import { TitleTypography } from "./components/TitleTypography";
import Fade from "react-reveal/Fade";
import BorrowedCarousel from "common/components/carousel/carousel";
import { LoginDialog } from "@landing/login/LoginDialog";

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

export const LoginPageTitleContent = ({}: LandingPageTitleContentProps) => {
    return (
        <TitleContent
        // title={
        //     <>
        //         {/* <LoginDialog /> */}
        //     </>
        // }
        // subtitle={<SubtitleTypography>Simple. No programing required</SubtitleTypography>}
        ></TitleContent>
    );
};
