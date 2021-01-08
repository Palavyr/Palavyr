import { DividerWithText } from "@common/components/DividerWithText";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Card, Divider, makeStyles, Typography } from "@material-ui/core";
import { PriceMap } from "@Palavyr-Types";
import React from "react";

export interface IFrequencyCard {
    title: string;
    interval: string;
    priceMap: PriceMap;
    onClick(): void;
}

const useStyles = makeStyles((theme) => ({
    outerCard: {
        margin: "2rem",
        padding: "1rem",
    },
    row: {
        textAlign: "center",
        padding: "1rem",
    },
    center: {
        width: "100%",
        textAlign: "center"
    },
}));

export const FrequencyCard = ({ title, priceMap, interval, onClick }: IFrequencyCard) => {
    const cls = useStyles();

    return (
        <Card className={cls.outerCard} onClick={onClick}>
            <div>
                <Typography align="center" variant="h4">
                    {title}
                </Typography>
                <Divider />
                <Typography variant="h5" className={cls.row}>
                    Price: ${parseFloat(priceMap[interval] as string) / 100}.00
                </Typography>
                <div style={{ width: "100%" }}>
                    <SinglePurposeButton classes={cls.center} variant="outlined" color="primary" buttonText="Go to Stripe Purchase Page" disabled={false} onClick={onClick} />
                </div>
            </div>
        </Card>
    );
};
