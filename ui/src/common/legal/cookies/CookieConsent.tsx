import React, { Fragment, useState, useCallback, useEffect } from "react";
import Cookies from "js-cookie";

import { europeanCountryCodes } from "./CountryCodes";
import fetchIpData from "./FetchIP";
import { makeStyles, Snackbar, Typography, Box, Button } from "@material-ui/core";

const useStyles = makeStyles<{}>((theme: any) => ({
    snackbarContent: {
        borderBotttomLeftRadius: 0,
        borderBottomRightRadius: 0,
        paddingLeft: theme.spacing(3),
        paddingRight: theme.spacing(3),
    },
}));

export interface ICookieConsent {
    handleCookieRulesDialogOpen: any;
    setOpen?: boolean;
}

export const CookieConsent = ({ handleCookieRulesDialogOpen, setOpen }: ICookieConsent) => {

    const classes = useStyles();
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        if (setOpen === true) {
            setIsVisible(true);
        } else {
            setIsVisible(false);
        }
    }, [setOpen])

    const openOnEuCountry = useCallback(() => {
        fetchIpData
            .then((data) => {
                if (
                    data &&
                    data.country &&
                    !europeanCountryCodes.includes(data.country)
                ) {
                    setIsVisible(false);
                } else {
                    setIsVisible(true);
                }
            })
            .catch(() => {
                setIsVisible(true);
                setOpen = false;
            });
    }, [setIsVisible]);


    const onAccept = useCallback(() => {
        Cookies.set("remember-cookie-snackbar", "", {
            expires: 365,
        });
        setIsVisible(false);
        setOpen = false;
    }, [setIsVisible]);

    useEffect(() => {
        if (Cookies.get("remember-cookie-snackbar") === undefined) {
            openOnEuCountry();
        }
    }, [openOnEuCountry]);

    return (
        <Snackbar
            className={classes.snackbarContent}
            open={isVisible}
            message={
                <Typography className="text-white">
                    We use cookies to ensure you get the best experience on our website.{" "}
                </Typography>
            }
            action={
                <Fragment>
                    <Box mr={1}>
                        <Button color="primary" onClick={handleCookieRulesDialogOpen}>
                            More details
                        </Button>
                    </Box>
                    <Button color="primary" onClick={onAccept}>
                        Got it!
          </Button>
                </Fragment>
            }
        />
    );
}

