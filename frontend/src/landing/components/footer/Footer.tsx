import * as React from 'react';
import { withWidth, useTheme, Divider, Grid, isWidthUp, Box, TextField, Hidden, IconButton, Typography, makeStyles } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import { useFooterStyles } from "./Footer.styles";
import { infos } from "./Infos";
import { socialIcons } from "./SocialIcons";
import transitions from "@material-ui/core/styles/transitions";


const useStyles = makeStyles(theme => ({
    footer: {
        paddingLeft: "15%",
        paddingRight: "15%",
        paddingTop: "3rem",
        paddingBottom: "3rem",
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
    brandText: {
        fontFamily: "'Baloo Bhaijaan', cursive",
        fontWeight: 400,
        color: theme.palette.common.white
    },
    footerLinks: {
        marginTop: theme.spacing(2.5),
        marginBot: theme.spacing(1.5),
        color: theme.palette.common.white
    },
    infoIcon: {
        color: `${theme.palette.common.white} !important`,
        backgroundColor: "#33383b !important"
    },
    socialIcon: {
        fill: theme.palette.common.white,
        backgroundColor: "#33383b",
        borderRadius: theme.shape.borderRadius,
        "&:hover": {
            backgroundColor: theme.palette.primary.light
        }
    },
    link: {
        cursor: "Pointer",
        color: theme.palette.common.white,
        transition: transitions.create(["color"], {
            duration: theme.transitions.duration.shortest,
            easing: theme.transitions.easing.easeIn
        }),
        "&:hover": {
            color: theme.palette.primary.light
        }
    },
    whiteBg: {
        backgroundColor: "#efedf4"
    }
}))



export const Footer = withWidth()(({ width }: IHaveWidth) => {
    const classes = useStyles();
    // const classes = useFooterStyles();
    const theme = useTheme();

    return (
        <footer className={classes.footer}>
                <Grid container justify="space-around">
                    <Grid item xs={6} md={6} lg={4}>
                        <Box display="flex" justifyContent="center">
                            <div>
                                {infos.map((info, index) => (
                                    <Box display="flex" mb={1} key={index}>
                                        <Box mr={2}>
                                            <IconButton
                                                className={classes.infoIcon}
                                                tabIndex={-1}
                                                disabled
                                            >
                                                {info.icon}
                                            </IconButton>
                                        </Box>
                                        <Box
                                            display="flex"
                                            flexDirection="column"
                                            justifyContent="center"
                                        >
                                            <Typography variant="h6">
                                                {info.description}
                                            </Typography>
                                        </Box>
                                    </Box>
                                ))}
                            </div>
                        </Box>
                    </Grid>
                    <Grid item xs={6} md={6} lg={4}>
                        <Typography variant="h3">
                            Palaver
                        </Typography>
                        <Typography variant="h6" paragraph>
                            pa·​lav·​er | \ pə-ˈla-vər
                        </Typography>
                        <Typography paragraph>
                            palaver noun
                            : a long parley usually between persons of different cultures or levels of sophistication.

                        </Typography>
                        <Box display="flex">
                            {socialIcons.map((socialIcon, index) => (
                                <Box key={index} mr={index !== socialIcons.length - 1 ? 1 : 0}>
                                    <IconButton
                                        aria-label={socialIcon.label}
                                        className={classes.socialIcon}
                                        href={socialIcon.href}
                                    >
                                        {socialIcon.icon}
                                    </IconButton>
                                </Box>
                            ))}
                        </Box>
                    </Grid>
                </Grid>
        </footer>
    );
})
