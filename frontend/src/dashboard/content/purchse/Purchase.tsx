import React from "react";
import { Card, Divider, FormControl, Grid, InputLabel, makeStyles, MenuItem, Paper, Select, Typography } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";
import { useEffect } from "react";
import { useCallback } from "react";
import { useState } from "react";
import { useLocation } from "react-router-dom";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Prices } from "@Palavyr-Types";
import { Elements, useStripe } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import { stripeKey, webUrl } from "@api-client/clientUtils";
import { SubscribeStepper } from "./SubscribeStepper";

const useStyles = makeStyles((theme) => ({
    root: {
        height: "100%",
    },
    body: {
        width: "100%",
        height: "100%",
        display: "flex",
        justifyContent: "center",
    },
    paper: {
        width: "75%",
        padding: "3rem",
        height: "100%",
        marginTop: "2rem",
        marginBottom: "3rem",
        backgroundColor: "#C7ECEE",
    },
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
        width: "100%",
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
    justifyCenter: {
        display: "flex",
        justifyContent: "center",
    },
    card: {
        margin: "2rem",
        padding: "2rem",
    },
    selectbox: {
        border: "1px solid gray",
        borderBottom: "0px solid black",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        backgroundColor: "white",
        padding: ".2rem",
        textAlign: "center",
    },
}));

export enum Interval {
    free = "free",
    monthly = "month",
    yearly = "year",
}

type PriceMap = {
    [key: string]: string;
};

const PurchaseInner = () => {
    const client = new ApiClient();
    const cls = useStyles();
    const [prices, setPrices] = useState<Prices>([]);
    const location = useLocation();
    const [interval, setInterval] = useState<Interval>(Interval.yearly);
    const [priceMap, setPriceMap] = useState<PriceMap>({});
    const [priceId, setSelectedPriceId] = useState<string>("");
    const stripe = useStripe();

    const searchParams = new URLSearchParams(location.search);
    const productType = searchParams.get("productType") as string;
    const productId = searchParams.get("productId") as string | null;

    var successUrl = `${webUrl}/dashboard/subscribe/success?session_id={CHECKOUT_SESSION_ID}`;
    var cancelUrl = `${webUrl}/dashboard/subscribe/canceled`;

    const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
        const interval = event.target.value as Interval;
        setInterval(interval);
        const selectedPriceId = priceMap[interval];
        setSelectedPriceId(selectedPriceId);
    };

    const getProducts = useCallback(async () => {
        const priceOptions = (await client.Purchase.Prices.GetPrices(productId!)).data as Prices;
        setPrices(priceOptions);

        const filledPriceMap: PriceMap = {};
        priceOptions.map((option) => {
            filledPriceMap[option.recurring.interval] = option.id;
        });
        setPriceMap(filledPriceMap);
        setSelectedPriceId(filledPriceMap[Interval.yearly]);
    }, []);

    const handleResult = (res) => {
        if (res.error) {
            console.log("ERROR on session");
        }
    };

    const displayFreeInformation = () => {
        return (
            <>
                <div style={{ width: "100%", display: "flex", justifyContent: "center", flexDirection: "column" }}>
                    <Typography align="center" variant="h4">
                        We're sad to see you go :( But if go you must...
                    </Typography>
                    <Divider />
                    <SinglePurposeButton
                        variant="outlined"
                        color="primary"
                        buttonText="Cancel your subscription"
                        onClick={async () => {
                            var result = (await client.Purchase.Subscription.CancelSubscription()).data as string;
                        }}
                    />
                </div>
            </>
        );
    };

    useEffect(() => {
        getProducts();
    }, []);

    const capitalize = (word: string) => word[0].toUpperCase() + word.slice(1);

    return (
        <>
            {productId !== null && productId !== "null" ? <SubscribeStepper activeStep={1} /> : <></>}
            <Grid container>
                <Grid item xs={12}>
                    {productId !== null && productId !== "null" ? (
                        <div style={{ display: "flex", justifyContent: "center", marginTop: "4rem" }}>
                            <Typography variant="h4" align="center">
                                Billing Frequency
                                <Divider />
                            </Typography>
                        </div>
                    ) : (
                        <></>
                    )}
                </Grid>

                <Grid item xs={12}>
                    <div style={{ display: "flex", justifyContent: "center" }}>
                        <Paper className={cls.paper}>
                            {productId === null || productId === "null" ? (
                                displayFreeInformation()
                            ) : (
                                <>
                                    <Grid container direction="row" justify="center" alignItems="center">
                                        <Grid item xs={6}>
                                            <Grid container direction="column" justify="space-between" alignItems="center">
                                                <Grid item xs={9}>
                                                    <Typography>Select Billing Frequency</Typography>
                                                </Grid>
                                                <Grid item xs={9}>
                                                    <FormControl className={cls.formControl}>
                                                        <InputLabel id="demo-simple-select-label-billing">Billing Frequency</InputLabel>
                                                        <Select className={cls.selectbox} fullWidth labelId="demo-simple-select-label-billing" id="demo-simple-select-billing" value={interval} onChange={handleChange}>
                                                            {prices.map((price, key) => {
                                                                return (
                                                                    <MenuItem key={key} value={price.recurring.interval}>
                                                                        {capitalize(price.recurring.interval) + "ly"}
                                                                    </MenuItem>
                                                                );
                                                            })}
                                                        </Select>
                                                    </FormControl>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                        <Grid item xs={6} style={{ display: "flex", justifyContent: "center" }}>
                                            <Card className={cls.card}>
                                                <div className={cls.justifyCenter}>
                                                    <Typography variant="body1">
                                                        <strong>Selected Plan:</strong> {productType}
                                                    </Typography>
                                                    <br />
                                                    <Typography variant="body1">
                                                        <strong>Billing Frequency:</strong> {capitalize(interval) + "ly"}
                                                    </Typography>
                                                </div>
                                            </Card>
                                        </Grid>
                                    </Grid>
                                    <Divider />
                                    <div style={{ width: "100%", display: "flex", justifyContent: "flex-end" }}>
                                        <SinglePurposeButton
                                            variant="outlined"
                                            color="primary"
                                            buttonText="Proceed to Stripe Checkout"
                                            onClick={async () => {
                                                var sessionId = (await client.Purchase.Checkout.CreateCheckoutSession(priceId, cancelUrl, successUrl)).data.sessionId as string;
                                                stripe!.redirectToCheckout({ sessionId: sessionId }).then(handleResult);
                                            }}
                                        />
                                    </div>
                                </>
                            )}
                        </Paper>
                    </div>
                </Grid>
            </Grid>
        </>
    );
};

const stripePromise = loadStripe(stripeKey);

export const Purchase = () => {
    return (
        <Elements stripe={stripePromise}>
            <PurchaseInner />
        </Elements>
    );
};
