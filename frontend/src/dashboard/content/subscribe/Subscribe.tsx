import * as React from "react";
import { Grid, makeStyles } from "@material-ui/core";
import { useHistory } from "react-router-dom";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { useCallback, useContext, useEffect, useState } from "react";
import { SubscribeStepper } from "../purchse/SubscribeStepper";
import { PurchaseTypes, ProductOptions, ProductOption, ProductIds, PlanStatus } from "@Palavyr-Types";
import { PURCHASE_ROUTE } from "@constants";
import { Lyte, Premium, Pro } from "@landing/components/pricing/Cards";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import classnames from "classnames";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

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
    const [productList, setProductList] = useState<ProductIds>();
    const { planTypeMeta } = useContext(DashboardContext);

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

    const getProducts = useCallback(async () => {
        const products = await repository.Products.getProducts();
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
            <AreaConfigurationHeader title="Select a subscription plan" subtitle="You won't be charged yet." divider />
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
