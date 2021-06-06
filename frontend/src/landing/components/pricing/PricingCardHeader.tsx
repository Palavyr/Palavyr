import React from "react";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import { DividerWithText } from "@common/components/DividerWithText";

const useStyles = makeStyles((theme) => ({
    title: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
    },
    money: {
        fontFamily: "'Noto Sans TC', sans-serif",
    },
    price: {
        paddingTop: "3.2rem",
        paddingBottom: "1.2rem",
        display: "inline-block",
    },
}));

export interface PricingCardHeaderProps {
    icon: React.ReactNode;
    title: string;
    currency: string;
    amount: string;
    per?: boolean;
    priceInfo?: boolean;
    showYearly?: boolean;
    yearlyAmount?: string;
}

export const PricingCardHeader = ({ icon, title, currency, amount, priceInfo = true, per = false, showYearly = false, yearlyAmount }: PricingCardHeaderProps) => {
    const cls = useStyles();
    return (
        <>
            {icon}
            <Typography className={cls.title} variant="h3">
                {title}
            </Typography>
            {priceInfo && (
                <>
                    <Typography className={classNames(cls.price, cls.money)} variant="h3">
                        {currency}
                    </Typography>
                    <Typography variant="h3" className={cls.price}>
                        {amount}
                    </Typography>
                    {per && (
                        <Typography className={cls.price} variant="h5">
                            / month
                        </Typography>
                    )}
                    {showYearly && (
                        <>
                            <DividerWithText text="Or" />
                            <Typography className={classNames(cls.price, cls.money)} variant="h4">
                                {currency}
                            </Typography>
                            <Typography display="block" variant="h4" className={classNames(cls.price, cls.money)}>
                                {yearlyAmount}
                            </Typography>
                            {per && (
                                <>
                                    <Typography display="block" className={cls.price} variant="h5">
                                        / year
                                    </Typography>
                                </>
                            )}
                        </>
                    )}
                </>
            )}
        </>
    );
};
