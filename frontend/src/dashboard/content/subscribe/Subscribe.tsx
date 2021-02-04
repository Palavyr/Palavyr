import * as React from "react";
import { FreeCard } from "@landing/components/pricing/FreeCard";
import { PremiumCard } from "@landing/components/pricing/PremiumCard";
import { ProCard } from "@landing/components/pricing/ProCard";
import { Card, Divider, Grid, makeStyles, Paper, Typography } from "@material-ui/core";
import classNames from "classnames";
import { pricingContainerStyles } from "@landing/components/pricing/cardStyles";
import { useHistory } from "react-router-dom";
import { ApiClient } from "@api-client/Client";
import { useCallback, useEffect, useState } from "react";
import { SubscribeStepper } from "../purchse/SubscribeStepper";
import { FreeProductId, PremiumProductId, ProProductId } from "./ProductIds";
import { PurchaseTypes, ProductOptions, ProductOption } from "@Palavyr-Types";
import { PURCHASE_ROUTE } from "@constants";

const useStyles = makeStyles((theme) => ({
    root: {},
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        paddingRight: "5%",
        paddingLeft: "5%",
        borderRadius: "4px",
    },
    card: {
        marginTop: "5rem",
        borderRadius: "5px",
        "&:hover": {
            background: "#5CADD8",
            transition: "margin-top 3s",
            borderRadius: "20px",
            border: "4px solid #AAFF97",
            marginTop: "1rem",
            cursor: "pointer",
        },
    },
    disabledCard: {
        border: "8px solid #567C4D",
        marginTop: "5rem",
        zIndex: 1,
    },
}));

export const Subscribe = () => {
    const [currentPlan, setCurrentPlan] = useState<string | null>(null);

    const cls = useStyles();
    const containerCls = pricingContainerStyles();
    const client = new ApiClient();

    const history = useHistory();

    const goToPurchase = (productType: PurchaseTypes, productId: string | null) => {
        let purchaseRoute = PURCHASE_ROUTE;
        if (productType !== null) {
            purchaseRoute += `?productType=${productType}`;
        }
        if (productId !== null) {
            purchaseRoute += `&productId=${productId}`;
        }
        history.push(purchaseRoute);
    };

    const OrderedProductOptions: ProductOptions = [
        {
            card: <FreeCard />,
            purchaseType: PurchaseTypes.Free,
            productId: FreeProductId,
            productClasses: containerCls.paperFree,
            currentplan: currentPlan === PurchaseTypes.Free,
        },
        {
            card: <PremiumCard />,
            purchaseType: PurchaseTypes.Premium,
            productId: PremiumProductId,
            productClasses: containerCls.paperPremium,
            currentplan: currentPlan === PurchaseTypes.Premium,
        },
        {
            card: <ProCard />,
            purchaseType: PurchaseTypes.Pro,
            productId: ProProductId,
            productClasses: containerCls.paperPro,
            currentplan: currentPlan === PurchaseTypes.Pro,
        },
    ];
    const getCurrentPlan = useCallback(async () => {
        var { data: plan } = await client.Settings.Account.getCurrentPlan();
        setCurrentPlan(plan);
    }, []);

    useEffect(() => {
        getCurrentPlan();
    }, []);

    return (
        <>
            <SubscribeStepper activeStep={0} />
            {currentPlan !== null && (
                <div className={classNames(cls.root, cls.body)}>
                    <Grid container>
                        <Grid item xs={12}>
                            <div style={{ display: "flex", justifyContent: "center", marginTop: "2rem" }}>
                                <Typography variant="h4" align="center">
                                    Select a subscription plan
                                    <Divider />
                                </Typography>
                            </div>
                        </Grid>

                        <Grid item xs={12}>
                            <div style={{ display: "flex", justifyContent: "space-evenly" }}>
                                {OrderedProductOptions.map((product: ProductOption, key: number) => {
                                    return (
                                        <Paper
                                            key={product.productId + "-" + key.toString()}
                                            onClick={() => (product.currentplan ? null : goToPurchase(product.purchaseType, product.productId))}
                                            data-aos="fade-down"
                                            data-aos-delay="100"
                                            className={classNames(product.currentplan ? cls.disabledCard : cls.card, containerCls.paperCommon, product.productClasses)}
                                            variant="outlined"
                                        >
                                            {product.currentplan ? (
                                                <Card key={product.currentplan + "-" + key.toString()} style={{ position: "relative", backgroundColor: "#567C4D", marginTop: "1.5rem", color: "black", zIndex: 2 }}>
                                                    <Typography variant="h4" align="center">
                                                        Your current plan
                                                    </Typography>
                                                </Card>
                                            ) : null}
                                            {product.card}
                                        </Paper>
                                    );
                                })}
                            </div>
                        </Grid>
                    </Grid>
                </div>
            )}
        </>
    );
};
