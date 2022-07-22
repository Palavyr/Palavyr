import React, { useContext } from "react";
import { makeStyles, Paper } from "@material-ui/core";
import { useEffect } from "react";
import { useCallback } from "react";
import { useState } from "react";
import { useLocation } from "react-router-dom";
import { Interval, PriceResource, PriceMap, PriceResources } from "@Palavyr-Types";
import { Elements, useStripe } from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import { stripeKey, webUrl } from "@common/client/clientUtils";
import { SubscribeStepper } from "./SubscribeStepper";
import { FrequencyCard } from "./FrequencyCard";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { DividerWithText } from "@common/components/DividerWithText";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { Align } from "@common/positioning/Align";
import { SpaceEvenly } from "@common/positioning/SpaceEvenly";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles((theme) => ({
    container: {
        backgroundColor: theme.palette.background.default,
        marginTop: theme.spacing(2),
    },
    paper: {
        width: "75%",
        padding: "3rem",
        height: "100%",
        marginTop: "2rem",
        marginBottom: "3rem",
        backgroundColor: theme.palette.success.light,
    },
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
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const [prices, setPrices] = useState<PriceResources>([]);
    const location = useLocation();
    const [priceMap, setPriceMap] = useState<PriceMap>({});
    const [priceId, setSelectedPriceId] = useState<string | number>("");
    const stripe = useStripe();

    const searchParams = new URLSearchParams(location.search);
    const productType = searchParams.get("productType") as string;
    const productId = searchParams.get("productId") as string | null;

    var successUrl = `${webUrl}/dashboard/subscribe/success?session_id={CHECKOUT_SESSION_ID}`;
    var cancelUrl = `${webUrl}/dashboard/subscribe/cancelled`;

    const getProducts = useCallback(async () => {
        if (productId == null) return;
        const priceOptions = await repository.Purchase.Prices.GetPrices(productId);
        setPrices(priceOptions);

        const filledPriceMap: PriceMap = {};
        priceOptions.map((option: PriceResource) => {
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

    useEffect(() => {
        getProducts();
    }, []);

    const capitalize = (word: string) => word[0].toUpperCase() + word.slice(1);

    const singlePurposeButtonOnClick = async (priceId: string) => {
        const sessionId = await repository.Purchase.Checkout.CreateCheckoutSession(priceId, cancelUrl, successUrl);
        if (stripe) stripe.redirectToCheckout({ sessionId: sessionId }).then(handleResult);
    };

    const intervalGetter = (x: PriceResource) => x.recurring.interval;

    return (
        <div className={cls.container}>
            {productId !== null && productId !== "null" ? (
                <HeaderStrip title="Billing Frequency" subtitle="Select your preferred billing frequency. This will take to an external website (Stripe.com) for your purchase." />
            ) : (
                <></>
            )}
            {productId !== null && productId !== "null" ? <SubscribeStepper activeStep={1} /> : <></>}
            <Align>
                <Paper className={cls.paper}>
                    <DividerWithText text={productType} variant="h4" />
                    <SpaceEvenly>
                        {sortByPropertyAlphabetical(intervalGetter, prices).map((price: PriceResource, key: number) => {
                            return (
                                <FrequencyCard
                                    key={price.productId + "-" + key.toString()}
                                    title={capitalize(price.recurring.interval) + "ly"}
                                    priceMap={priceMap}
                                    interval={price.recurring.interval}
                                    onClick={() => singlePurposeButtonOnClick(price.id)}
                                />
                            );
                        })}
                    </SpaceEvenly>
                </Paper>
            </Align>
        </div>
    );
};
