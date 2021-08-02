import React from "react";
import { socialIcons } from "./SocialIcons";
import { Grid, Box, IconButton, makeStyles } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";

const useStylesBottom = makeStyles((theme) => ({
    bottom: {
        height: "100px",
        backgroundColor: "#0D1C27",
        padding: " 2rem",
    },
    socialIcon: {
        fill: theme.palette.common.white,
        backgroundColor: "#33383b",
        borderRadius: theme.shape.borderRadius,
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
    },
}));

export const BottomStrip = () => {
    const cls = useStylesBottom();

    return (
        <div className={cls.bottom}>
            <Align >
                {/* <Grid container>
                    <Grid item xs={6} md={6} lg={4}> */}
                <Box display="flex">© Palavyr.com 2021</Box>
                {/* </Grid> */}
                {/* <Grid item xs={6} md={6} lg={4}>
                        <Box display="flex"> */}
                {socialIcons.map((socialIcon, index) => (
                    <Box key={index} mr={index !== socialIcons.length - 1 ? 1 : 0}>
                        <IconButton aria-label={socialIcon.label} className={cls.socialIcon} href={socialIcon.href}>
                            {socialIcon.icon}
                        </IconButton>
                    </Box>
                ))}
                {/* </Box>
                    </Grid>
                </Grid> */}
            </Align>
        </div>
    );
};
