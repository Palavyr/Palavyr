import { withWidth, Box, Card, Grid, Typography, isWidthUp, Button, Hidden, Divider } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import { useHeaderStyles } from "./Header.styles";
import React from "react";
import classNames from "classnames";
import AOS from 'aos';

AOS.init({ once: true });


export const Header = withWidth()(({ width }: IHaveWidth) => {

    const classes = useHeaderStyles();

    return (
        <>
            <div className={classNames("lg-p-top", classes.wrapper)}>
                <div className={classNames("container-fluid", classes.container)}>
                    <Box display="flex" justifyContent="center" className="row">
                        <Card
                            className={classes.card}
                            data-aos-delay="200"
                            data-aos="zoom-in"
                        >
                            <div className={classNames(classes.containerFix, "container")}>
                                <Box justifyContent="space-between" className="row">
                                    <Grid item xs={12} md={5}>
                                        <Box
                                            display="flex"
                                            flexDirection="column"
                                            justifyContent="space-between"
                                            height="100%"
                                        >
                                            <Box mb={4}>
                                                <Typography
                                                    className={classes.mainBox}
                                                    variant={isWidthUp("lg", width) ? "h3" : "h4"}
                                                >
                                                    Build Incredible Conversations
                                                </Typography>
                                            </Box>
                                            <div>
                                                <Box mb={2}>
                                                    <Typography
                                                        className={classes.mainBox}
                                                        variant={isWidthUp("lg", width) ? "h6" : "body1"}
                                                        color="textSecondary"
                                                    >
                                                        Use Palavyr to configure a custom chat widget for your site to engage customers.
                                                </Typography>
                                                </Box>
                                                <Button
                                                    variant="contained"
                                                    color="secondary"
                                                    fullWidth
                                                    className={classes.extraLargeButton}
                                                    classes={{ label: classes.extraLargeButtonLabel }}
                                                >
                                                    Try Palavyn!
                                                </Button>
                                            </div>
                                        </Box>
                                    </Grid>
                                    <Hidden smDown>
                                        <Grid item md={6}>
                                        </Grid>
                                    </Hidden>
                                </Box>
                            </div>
                        </Card>
                    </Box>
                </div>
            </div>
            <Divider />
        </>
    );
})
