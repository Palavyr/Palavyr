import React from "react";
import { makeStyles, useTheme } from "@material-ui/core";
import AOS from "aos";
import { LandingPage } from "./components/LandingPage";

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

export const LoginPage = () => {
    const cls = useStyles();
    const theme = useTheme();

    return <LandingPage />;
};
