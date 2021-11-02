import React from "react";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { ShareButton } from "@landing/blog/components/shareButtons/ShareButton";
import { makeStyles, Box, Grid, Typography, Card } from "@material-ui/core";
import classNames from "classnames";

const useStyles = makeStyles((theme) => ({
    blogContentWrapper: {
        marginLeft: theme.spacing(1),
        marginRight: theme.spacing(1),
        marginBottom: theme.spacing(4),
        [theme.breakpoints.up("sm")]: {
            marginLeft: theme.spacing(4),
            marginRight: theme.spacing(4),
        },
        maxWidth: 1280,
        width: "100%",
    },
    wrapper: {
        minHeight: "60vh",
        paddingTop: "3rem",
    },
    img: {
        width: "100%",
        height: "auto",
    },
    card: {
        boxShadow: theme.shadows[4],
    },
}));

export interface OurStoryWrapperProps {
    children: React.ReactChild;
}

export const OurStoryWrapper = ({ children }: OurStoryWrapperProps) => {
    const cls = useStyles();
    return (
        <Box className={classNames(cls.wrapper)} display="flex" justifyContent="center">
            <div className={cls.blogContentWrapper}>
                <Grid container spacing={5}>
                    <Grid item md={12}>
                        <Card className={cls.card}>
                            <Box pt={3} pr={3} pl={3} pb={2}>
                                <Typography align="center" variant="h4">
                                    <b>Palavyr wants to make your world a better place</b>
                                </Typography>
                            </Box>
                            <ZoomImage className={cls.img} imgSrc={""} alt="" />
                            <Box p={3}>
                                {children}
                                <Box pt={5}>
                                    <Grid spacing={1} container>
                                        {["Facebook", "Twitter", "Reddit", "Tumblr"].map((type, index) => (
                                            <Grid item key={index}>
                                                <ShareButton type={type} title="About Palavyr" description="Check out this meaningful story about Palavyr!" disableElevation={true} variant="contained" />
                                            </Grid>
                                        ))}
                                    </Grid>
                                </Box>
                            </Box>
                        </Card>
                    </Grid>
                </Grid>
            </div>
        </Box>
    );
};
