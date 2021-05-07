import React from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Card, Divider, makeStyles, Typography } from "@material-ui/core";
import { PriceMap } from "@Palavyr-Types";
import { Align } from "dashboard/layouts/positioning/Align";
import { green } from "theme";
import { green50 } from "colorTheme/base";

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
        backgroundColor: theme.palette.primary.dark,
        width: "100%",
    },
    row: {
        textAlign: "center",
        padding: "1rem",
    },
    center: {
        width: "100%",
        textAlign: "center",
    },
    text: {
        color: theme.palette.common.white,
    },
    buttonHover: {
        "&:hover": {
            backgroundColor: green50
        }
    }
}));

export const FrequencyCard = ({ title, priceMap, interval, onClick }: IFrequencyCard) => {
    const cls = useStyles();

    return (
        <Card className={cls.outerCard} onClick={onClick}>
            <div>
                <Typography variant="h5" align="center" gutterBottom className={cls.text}>
                    {title}
                </Typography>
                <Divider />
                <Typography variant="h4" align="center" gutterBottom className={cls.text}>
                    Price: ${parseFloat(priceMap[interval] as string) / 100}.00
                </Typography>
                <Align>
                    <SinglePurposeButton classes={cls.buttonHover} variant="outlined" color="primary" buttonText="Go to Stripe Purchase Page" disabled={false} onClick={onClick} />
                </Align>
            </div>
        </Card>
    );
};
