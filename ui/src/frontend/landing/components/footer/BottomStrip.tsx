import React from "react";
import { socialIcons } from "./SocialIcons";
import { Box, Hidden, IconButton, makeStyles, Typography } from "@material-ui/core";
import { FooterWrapper } from "./FooterWrapper";
import OctopusLogo from "./octopusLogo.svg";
import classNames from "classnames";
import { LineSpacer } from "@common/components/typography/LineSpacer";

const useStylesBottom = makeStyles(theme => ({
    socialIcon: {
        fill: theme.palette.common.white,
        backgroundColor: "#33383b",
        borderRadius: theme.shape.borderRadius,
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
    },
    icons: {
        display: "flex",
        justifyContent: "center",
    },
    headRoom: {
        marginTop: "1rem",
    },
    smallContainer: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        color: theme.palette.common.white,
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
}));

export const BottomStrip = () => {
    const cls = useStylesBottom();

    return (
        <>
            <Hidden smDown>
                <FooterWrapper backgroundColor="#0D1C27">
                    <div className={cls.headRoom}>
                        <Typography gutterBottom display="inline">
                            info.palavyr@gmail.com
                        </Typography>
                        <Typography>© Palavyr.com 2021</Typography>
                    </div>
                    <span className={classNames(cls.icons, cls.headRoom)}>
                        <div style={{ display: "inline-block", marginRight: "2rem" }}>
                            <Typography align="center">Deployed with</Typography>
                            <OctopusLogo style={{ cursor: "pointer" }} onClick={() => window.open("https://www.octopus.com", "_blank")} />
                        </div>
                        {socialIcons.map((socialIcon, index) => (
                            <Box key={index} mr={index !== socialIcons.length - 1 ? 1 : 0}>
                                <IconButton aria-label={socialIcon.label} className={cls.socialIcon} href={socialIcon.href}>
                                    {socialIcon.icon}
                                </IconButton>
                            </Box>
                        ))}
                    </span>
                </FooterWrapper>
            </Hidden>
            <Hidden mdUp>
                <footer className={cls.smallContainer} style={{ backgroundColor: "#0D1C27" }}>
                    <div className={cls.headRoom}>
                        <Typography align="center" gutterBottom display="block">
                            info.palavyr@gmail.com
                        </Typography>
                    </div>
                    <LineSpacer />
                    <div style={{ width: "100%", display: "flex", justifyContent: "center" }}>
                        <div style={{ display: "inline-block", justifyContent: "center", width: "120px" }}>
                            <Typography align="center">Deployed with</Typography>
                            <OctopusLogo style={{ cursor: "pointer" }} onClick={() => window.open("https://www.octopus.com", "_blank")} />
                        </div>
                    </div>
                    <LineSpacer />
                    <span className={classNames(cls.icons, cls.headRoom)}>
                        {socialIcons.map((socialIcon, index) => (
                            <Box key={index} mr={index !== socialIcons.length - 1 ? 1 : 0}>
                                <IconButton aria-label={socialIcon.label} className={cls.socialIcon} href={socialIcon.href}>
                                    {socialIcon.icon}
                                </IconButton>
                            </Box>
                        ))}
                    </span>
                    <LineSpacer />
                    <Typography display="block" align="center">
                        © Palavyr.com 2021
                    </Typography>
                </footer>
            </Hidden>
        </>
    );
};
