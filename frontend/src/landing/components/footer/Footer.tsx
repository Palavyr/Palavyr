import * as React from "react";
import { withWidth, Typography, makeStyles, useTheme, Button } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import transitions from "@material-ui/core/styles/transitions";
import { FooterWrapper } from "./FooterWrapper";
import { Link, useHistory } from "react-router-dom";
import classNames from "classnames";
import { BrandName } from "@landing/branding/BrandName";

const useStyles = makeStyles((theme) => ({
    // link: {
    //     cursor: "Pointer",
    //     color: theme.palette.common.white,
    //     transition: transitions.create(["color"], {
    //         duration: theme.transitions.duration.shortest,
    //         easing: theme.transitions.easing.easeIn,
    //     }),
    //     "&:hover": {
    //         color: theme.palette.primary.light,
    //     },
    // },

    title2: {
        marginTop: "3rem",
        marginBottom: "3rem",
        width: "70%",
    },
    items: {
        marginTop: "3rem",
        marginBottom: "3rem",
        width: "100%",
        display: "flex",
        justifyContent: "space-around",
        paddingLeft: "2rem",
        paddingRight: "2rem",
        "& li": {
            listStyleType: "none",
            marginBottom: ".7rem",
        },
    },
    noDecoration: {
        textDecoration: "none !important",
    },
    menuButtonText: {
        color: theme.palette.common.white,
        "&:hover": {
            color: theme.palette.success.main,
            cursor: "pointer",
        },
    },
}));

export interface IFooter extends IHaveWidth {
    openLoginDialog: any;
    openRegisterDialog: any;
    openTermsDialog: any;
}

export const Footer = withWidth()(({ width, openLoginDialog, openRegisterDialog, openTermsDialog }: IFooter) => {
    const cls = useStyles();
    const theme = useTheme();

    return (
        <FooterWrapper backgroundColor={theme.palette.primary.main}>
            <div className={cls.title2}>
                <BrandName />
                <Typography variant="h6">The no-code platform for building chatbots</Typography>
            </div>
            <div className={cls.items}>
                <ul>
                    <li>
                        <Typography display="block" variant="h5">
                            Website
                        </Typography>
                    </li>
                    <li>
                        <Typography variant="body1" onClick={openLoginDialog} className={cls.menuButtonText}>
                            Login
                        </Typography>
                    </li>
                    <li>
                        <Typography variant="body1" onClick={openRegisterDialog} className={cls.menuButtonText}>
                            Create new account
                        </Typography>
                    </li>
                </ul>
                <ul>
                    <li>
                        <Typography variant="h5">Learn Palavyr</Typography>
                    </li>
                    <li>
                        <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                            <Typography variant="body1" className={cls.menuButtonText}>
                                Tutorial Series
                            </Typography>
                        </Link>
                    </li>
                </ul>
                <ul>
                    <li>
                        <Typography variant="h5">Company</Typography>
                    </li>
                    <li>Team</li>
                    <li>Our Story</li>
                    <li>Blog</li>
                    <li>
                        <Typography variant="body1" onClick={openTermsDialog} className={cls.menuButtonText}>
                            Terms of Use
                        </Typography>
                    </li>
                    <li>Privacy Policy</li>
                </ul>
            </div>
        </FooterWrapper>
    );
});
