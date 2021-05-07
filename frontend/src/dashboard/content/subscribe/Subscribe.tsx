import * as React from "react";
import { Grid, makeStyles } from "@material-ui/core";
import { useHistory } from "react-router-dom";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { useCallback, useEffect, useState } from "react";
import { SubscribeStepper } from "../purchse/SubscribeStepper";
import { PurchaseTypes, ProductOptions, ProductOption, ProductIds, PlanStatus } from "@Palavyr-Types";
import { PURCHASE_ROUTE } from "@constants";
import { Premium, Pro } from "@landing/components/pricing/Cards";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import classnames from "classnames";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";

const useStyles = makeStyles((theme) => ({
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        paddingRight: "5%",
        paddingLeft: "5%",
        borderRadius: "4px",
    },
    card: {
        marginTop: "3rem",
        borderRadius: "5px",
        transition: "all ease-in-out 0.5s",
        "&:hover": {
            transition: "ease-in-out 0.5s",
            borderRadius: "20px",
            marginTop: "1rem",
            cursor: "pointer",
        },
    },

    width: { width: "40%" },
}));

export const Subscribe = () => {
    const [currentPlan, setCurrentPlan] = useState<PlanStatus | null>(null);
    const [productList, setProductList] = useState<ProductIds>();

    const cls = useStyles();
    const repository = new PalavyrRepository();

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

    const getCurrentPlan = useCallback(async () => {
        const plan = await repository.Settings.Account.getCurrentPlan();
        setCurrentPlan(plan);
    }, []);

    const getProducts = useCallback(async () => {
        const products = await repository.Products.getProducts();
        setProductList(products);
    }, []);

    useEffect(() => {
        getProducts();
        getCurrentPlan();
    }, []);

    const orderedProductOptions: ProductOptions = [
        {
            card: <Premium priceInfo={false} />,
            purchaseType: PurchaseTypes.Premium,
            productId: productList?.premiumProductId || null,
            currentplan: currentPlan?.status === PurchaseTypes.Premium,
        },
        {
            card: <Pro priceInfo={false} />,
            purchaseType: PurchaseTypes.Pro,
            productId: productList?.proProductId || null,
            currentplan: currentPlan?.status === PurchaseTypes.Pro,
        },
    ];

    return (
        <>
            <AreaConfigurationHeader title="Select a subscription plan" subtitle="You won't be charged yet." divider />
            <SubscribeStepper activeStep={0} />
            {currentPlan !== null && (
                <div className={cls.body}>
                    <Grid container>
                        <Grid item xs={12}>
                            <SpaceEvenly center >
                                {orderedProductOptions.map((product: ProductOption, key: number) => {
                                    return (
                                        <div onClick={() => (product.currentplan || currentPlan.hasUpgraded ? null : goToPurchase(product.purchaseType, product.productId))} className={classnames(cls.width, cls.card)}>
                                            {product.card}
                                        </div>
                                    );
                                })}
                            </SpaceEvenly>
                        </Grid>
                    </Grid>
                </div>
            )}
        </>
    );
};
