import React from "react";
import { makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";

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
}

export const PricingCardHeader = ({ icon, title, currency, amount, priceInfo = true, per = false }: PricingCardHeaderProps) => {
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
                </>
            )}
        </>
    );
};
