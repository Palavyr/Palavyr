import * as React from "react";
import { withWidth, Grid, Box, IconButton, Typography, makeStyles } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import { infos } from "./Infos";
import transitions from "@material-ui/core/styles/transitions";
import { DeployedWith } from "./DeployedWith";

const useStyles = makeStyles((theme) => ({
    footer: {
        paddingLeft: "12%",
        paddingRight: "12%",
        paddingTop: "3rem",
        paddingBottom: "3rem",
        color: theme.palette.success.main,
        background: theme.palette.primary.main,
    },
    brandText: {
        fontFamily: "'Baloo Bhaijaan', cursive",
        fontWeight: 400,
        color: theme.palette.common.white,
    },
    footerLinks: {
        marginTop: theme.spacing(2.5),
        marginBot: theme.spacing(1.5),
        color: theme.palette.common.white,
    },
    infoIcon: {
        color: `${theme.palette.common.white} !important`,
        backgroundColor: "#33383b !important",
    },

    link: {
        cursor: "Pointer",
        color: theme.palette.common.white,
        transition: transitions.create(["color"], {
            duration: theme.transitions.duration.shortest,
            easing: theme.transitions.easing.easeIn,
        }),
        "&:hover": {
            color: theme.palette.primary.light,
        },
    },
    whiteBg: {
        backgroundColor: "#efedf4",
    },
}));

export const Footer = withWidth()(({ width }: IHaveWidth) => {
    const classes = useStyles();

    return (
        <footer className={classes.footer}>
            <Grid container justify="space-around">
                <Grid item xs={6} md={6} lg={4}>
                    <Typography variant="h3">Palavyr</Typography>
                    <Typography display="inline" variant="h6">
                        The best no-code platform for building chatbots
                    </Typography>
                </Grid>
                <Grid item xs={6} md={6} lg={4}>
                    <Box display="flex">
                        {infos.map((info, index) => (
                            <>
                                <Box mr={2}>
                                    <IconButton className={classes.infoIcon} tabIndex={-1} disabled>
                                        {info.icon}
                                    </IconButton>
                                </Box>
                                <Box display="flex" flexDirection="column" justifyContent="center">
                                    <Typography variant="h6">{info.description}</Typography>
                                </Box>
                            </>
                        ))}
                    </Box>
                    <DeployedWith />
                </Grid>
            </Grid>
        </footer>
    );
});
