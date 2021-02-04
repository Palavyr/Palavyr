import React from "react";
import { Card, Divider, FormControl, Grid, InputLabel, makeStyles, MenuItem, Paper, Select, Typography } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";
import { useEffect } from "react";
import { useCallback } from "react";
import { useState } from "react";
import { useLocation } from "react-router-dom";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Interval, Price, PriceMap, Prices } from "@Palavyr-Types";
import { Elements, useStripe } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import { stripeKey, webUrl } from "@api-client/clientUtils";
import { SubscribeStepper } from "./SubscribeStepper";
import { FrequencyCard } from "./FrequencyCard";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { DividerWithText } from "@common/components/DividerWithText";

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
    freeInformationDiv: { width: "100%", display: "flex", justifyContent: "center", flexDirection: "column" },
}));

const stripePromise = loadStripe(stripeKey);

export const Purchase = () => {
    return (
        <Elements stripe={stripePromise}>
            <PurchaseInner />
        </Elements>
    );
};

const PurchaseInner = () => {
    const client = new ApiClient();
    const cls = useStyles();
    const [prices, setPrices] = useState<Prices>([]);
    const location = useLocation();
    const [interval, setInterval] = useState<Interval>(Interval.yearly);
    const [priceMap, setPriceMap] = useState<PriceMap>({});
    const [priceId, setSelectedPriceId] = useState<string | number>("");
    const [currentPrice, setCurrentPrice] = useState<number>(0);
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
        if (productId == null) return;
        const { data: priceOptions } = await client.Purchase.Prices.GetPrices(productId);
        setPrices(priceOptions);

        const filledPriceMap: PriceMap = {};
        priceOptions.map((option: Price) => {
            filledPriceMap[option.recurring.interval] = option.unitAmountDecimal;
        });
        setPriceMap(filledPriceMap);
        setSelectedPriceId(filledPriceMap[Interval.yearly]);
    }, []);

    const handleResult = (res) => {
        if (res.error) {
            console.log("ERROR on session");
        }
    };

    const cancelSubscriptionOnClick = async () => {
        const { data: result } = await client.Purchase.Subscription.CancelSubscription();
    };

    const displayFreeInformation = () => {
        return (
            <div className={cls.freeInformationDiv}>
                <Typography align="center" variant="h4">
                    We're sad to see you go :( But if go you must...
                </Typography>
                <Divider />
                <SinglePurposeButton variant="outlined" color="primary" buttonText="Cancel your subscription" onClick={cancelSubscriptionOnClick} />
            </div>
        );
    };

    useEffect(() => {
        getProducts();
    }, []);

    const capitalize = (word: string) => word[0].toUpperCase() + word.slice(1);

    const singlePurposeButtonOnClick = async (priceId: string) => {
        const { data: sessionId } = await client.Purchase.Checkout.CreateCheckoutSession(priceId, cancelUrl, successUrl);
        if (stripe) stripe.redirectToCheckout({ sessionId: sessionId }).then(handleResult);
    };

    const intervalGetter = (x: Price) => x.recurring.interval;

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
                                <Grid container direction="row" justify="center" alignItems="center">
                                    <Grid item xs={12}>
                                        <DividerWithText text={productType} variant="h4" />
                                    </Grid>
                                    {sortByPropertyAlphabetical(intervalGetter, prices).map((price: Price, key) => {
                                        return (
                                            <Grid key={price.product + "-" + key.toString()} item xs={6}>
                                                <FrequencyCard key={price.productId + "-" + key.toString()} title={capitalize(price.recurring.interval) + "ly"} priceMap={priceMap} interval={price.recurring.interval} onClick={() => singlePurposeButtonOnClick(price.id)} />
                                            </Grid>
                                        );
                                    })}
                                </Grid>
                            )}
                        </Paper>
                    </div>
                </Grid>
            </Grid>
        </>
    );
};
