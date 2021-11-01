import * as React from "react";
import { useMediaQuery, withWidth, Typography, makeStyles, useTheme, Button, Hidden } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import transitions from "@material-ui/core/styles/transitions";
import { FooterWrapper } from "./FooterWrapper";
import { Link, useHistory } from "react-router-dom";
import { BrandName } from "@landing/branding/BrandName";
import { FooterListTitle } from "./FooterListTitle";
import { FooterListItem } from "./FooterListItem";
import { FooterUList } from "./FooterUList";
import { Align } from "dashboard/layouts/positioning/Align";
import { LineSpacer } from "@common/components/typography/LineSpacer";

const useStyles = makeStyles(theme => ({
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
        marginBottom: "2rem",
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
    smallFooterContainer: {
        display: "flex",
        flexDirection: "column",
    },
    smallItems: {
        width: "100%",

        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        textAlign: "center",
        marginLeft: "-1rem",
        "& li": {
            listStyleType: "none",
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
            <Hidden smDown>
                <div className={cls.title2}>
                    <BrandName />
                    <Typography variant="h6">The small business platform for building chatbots</Typography>
                </div>
                <div className={cls.items}>
                    <FooterUList>
                        <FooterListTitle>Website</FooterListTitle>
                        <FooterListItem onClick={openLoginDialog}>Log in</FooterListItem>
                        <FooterListItem onClick={openRegisterDialog}>Create new account</FooterListItem>
                    </FooterUList>

                    <FooterUList>
                        <FooterListTitle>Learn</FooterListTitle>
                        <FooterListItem>
                            <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                                <Typography variant="body1" className={cls.menuButtonText}>
                                    Tutorial Series
                                </Typography>
                            </Link>
                        </FooterListItem>
                    </FooterUList>
                    <FooterUList>
                        <FooterListTitle>Company</FooterListTitle>
                        <FooterListItem>
                            <FooterListItem>
                                <Link key="Blog" to="/blog" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Blog
                                    </Typography>
                                </Link>
                            </FooterListItem>
                            <Link key="Team" to="/team" className={cls.noDecoration}>
                                <Typography variant="body1" className={cls.menuButtonText}>
                                    Team
                                </Typography>
                            </Link>
                        </FooterListItem>
                        <FooterListItem>
                            <Link key="Our Story" to="/our-story" className={cls.noDecoration}>
                                <Typography variant="body1" className={cls.menuButtonText}>
                                    Our Story
                                </Typography>
                            </Link>
                        </FooterListItem>
                        <FooterListItem>
                            <Link key="Tutorial" to="/terms-of-use" className={cls.noDecoration}>
                                <Typography variant="body1" className={cls.menuButtonText}>
                                    Terms Of Use
                                </Typography>
                            </Link>
                        </FooterListItem>
                        <FooterListItem>
                            <Link key="Tutorial" to="/privacy-policy" className={cls.noDecoration}>
                                <Typography variant="body1" className={cls.menuButtonText}>
                                    Privacy Policy
                                </Typography>
                            </Link>
                        </FooterListItem>
                    </FooterUList>
                </div>
            </Hidden>
            <Hidden mdUp>
                <div className={cls.smallFooterContainer}>
                    <Align>
                        <BrandName />
                    </Align>
                    <div>
                        <Typography display="block" align="center" variant="h6">
                            The small business platform for building chatbots
                        </Typography>
                    </div>
                    <div className={cls.smallItems}>
                        <FooterUList>
                            <FooterListTitle>Website</FooterListTitle>
                            <FooterListItem onClick={openLoginDialog}>Login</FooterListItem>
                            <FooterListItem onClick={openRegisterDialog}>Create Free account</FooterListItem>
                        </FooterUList>
                        <LineSpacer numLines={1} />
                        <FooterUList>
                            <FooterListTitle>Learn</FooterListTitle>
                            <FooterListItem>
                                <Link key="Blog" to="/blog" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Blog
                                    </Typography>
                                </Link>
                            </FooterListItem>
                            <FooterListItem>
                                <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Tutorial Series
                                    </Typography>
                                </Link>
                            </FooterListItem>
                        </FooterUList>
                        <LineSpacer numLines={1} />
                        <FooterUList>
                            <FooterListTitle>Company</FooterListTitle>
                            <FooterListItem>
                                <Link key="Our Story" to="/our-story" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Our Story
                                    </Typography>
                                </Link>
                            </FooterListItem>
                            <FooterListItem>
                                <Link key="Team" to="/team" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Team
                                    </Typography>
                                </Link>
                            </FooterListItem>
                            <FooterListItem>
                                <Link key="Tutorial" to="/terms-of-use" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Terms Of Use
                                    </Typography>
                                </Link>
                            </FooterListItem>
                            <FooterListItem>
                                <Link key="Tutorial" to="/privacy-policy" className={cls.noDecoration}>
                                    <Typography variant="body1" className={cls.menuButtonText}>
                                        Privacy Policy
                                    </Typography>
                                </Link>
                            </FooterListItem>
                        </FooterUList>
                    </div>
                </div>
            </Hidden>
        </FooterWrapper>
    );
});
