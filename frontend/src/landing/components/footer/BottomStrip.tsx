import React from "react";
import { socialIcons } from "./SocialIcons";
import { Box, IconButton, makeStyles, Typography } from "@material-ui/core";
import { FooterWrapper } from "./FooterWrapper";
import OctopusLogo from "./octopusLogo.svg";
import classNames from "classnames";

const useStylesBottom = makeStyles((theme) => ({
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
    },
    headRoom: {
        marginTop: "1rem",
    },
}));

export const BottomStrip = () => {
    const cls = useStylesBottom();

    return (
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
    );
};
