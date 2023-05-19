import * as React from "react";
import { Grid, makeStyles } from "@material-ui/core";
import { useHistory } from "react-router-dom";
import { useCallback, useContext, useEffect, useState } from "react";
import { SubscribeStepper } from "../purchse/SubscribeStepper";
import {  ProductOptions, ProductOption, ProductIds } from "@Palavyr-Types";
import { PURCHASE_ROUTE } from "@constants";
import { Lyte, Premium, Pro } from "@landing/components/pricing/Cards";
import { SpaceEvenly } from "@common/positioning/SpaceEvenly";
import classnames from "classnames";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PurchaseTypes } from "@common/types/api/Enums";

const useStyles = makeStyles<{}>((theme: any) => ({
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
    const [productList, setProductList] = useState<ProductIds>();
    const { planTypeMeta, setViewName } = useContext(DashboardContext);
    setViewName("Subscriptions");

    const cls = useStyles();
    const { repository } = useContext(DashboardContext);

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

    const getProducts = useCallback(async () => {
        const products = await repository.Products.GetProducts();
        setProductList(products);
    }, []);

    useEffect(() => {
        getProducts();
    }, []);

    const orderedProductOptions: ProductOptions = [
        {
            card: <Lyte priceInfo={true} showYearly />,
            purchaseType: PurchaseTypes.Lyte,
            productId: productList?.lyteProductId || null,
            currentplan: planTypeMeta?.planType === PurchaseTypes.Lyte,
        },
        {
            card: <Premium priceInfo={true} showYearly />,
            purchaseType: PurchaseTypes.Premium,
            productId: productList?.premiumProductId || null,
            currentplan: planTypeMeta?.planType === PurchaseTypes.Premium,
        },
        {
            card: <Pro priceInfo={true} showYearly />,
            purchaseType: PurchaseTypes.Pro,
            productId: productList?.proProductId || null,
            currentplan: planTypeMeta?.planType === PurchaseTypes.Pro,
        },
    ];

    return (
        <>
            <HeaderStrip title="Select a subscription plan" subtitle="You won't be charged yet." divider />
            <SubscribeStepper activeStep={0} />
            {planTypeMeta !== null && (
                <div className={cls.body}>
                    <Grid container>
                        <Grid item xs={12}>
                            <SpaceEvenly center>
                                {planTypeMeta &&
                                    orderedProductOptions.map((product: ProductOption, key: number) => {
                                        return (
                                            <div onClick={() => planTypeMeta && goToPurchase(product.purchaseType, product.productId)} className={classnames(cls.width, cls.card)}>
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
