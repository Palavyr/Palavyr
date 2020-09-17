import * as React from 'react';
import { withWidth, useTheme, Divider, Grid, isWidthUp, Box, TextField, Hidden, IconButton, Typography } from "@material-ui/core";
import { IHaveWidth } from "@Palavyr-Types";
import { useFooterStyles } from "./Footer.styles";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { infos } from "./Infos";
import { socialIcons } from "./SocialIcons";


export const Footer = withWidth()(({ width }: IHaveWidth) => {

    const classes = useFooterStyles();
    const theme = useTheme();

    return (
        <footer className="lg-p-top">
            <Divider />
            <div className={classes.footerInner}>
                <Grid container spacing={isWidthUp("md", width) ? 10 : 5}>
                    <Grid item xs={12} md={6} lg={4}>
                        <form>
                            <Box display="flex" flexDirection="column">
                                <Box mb={1}>
                                    <TextField
                                        variant="outlined"
                                        multiline
                                        placeholder="Get in touch with us"
                                        inputProps={{ "aria-label": "Get in Touch" }}
                                        InputProps={{
                                            className: classes.whiteBg
                                        }}
                                        rows={4}
                                        fullWidth
                                        required
                                    />
                                </Box>
                                <ColoredButton
                                    color="primary"
                                    variant="outlined"
                                    onClick={() => { console.log("TODO: send a message to meeeee") }}
                                    type="submit"
                                >
                                    Send Message
                                </ColoredButton>
                            </Box>
                        </form>
                    </Grid>
                    <Hidden mdDown>
                        <Grid item xs={12} md={6} lg={4}>
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
                                                <Typography variant="h6" className="text-white">
                                                    {info.description}
                                                </Typography>
                                            </Box>
                                        </Box>
                                    ))}
                                </div>
                            </Box>
                        </Grid>
                    </Hidden>
                    <Grid item xs={12} md={6} lg={4}>
                        <Typography variant="h3" className="text-white">
                            Palaver
                        </Typography>
                        <Typography variant="h6" paragraph className="text-white">
                            pa·​lav·​er | \ pə-ˈla-vər
                        </Typography>
                        <Typography style={{ color: "#8f9296" }} paragraph>
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
            </div>
        </footer>
    );
})
