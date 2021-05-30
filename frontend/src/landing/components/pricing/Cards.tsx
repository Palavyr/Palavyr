import React from "react";
import { makeStyles, Paper, useTheme } from "@material-ui/core";
import { PricingCard } from "./PricingCard";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import NotInterestedIcon from "@material-ui/icons/NotInterested";
import { green, red } from "theme";
import classnames from "classnames";
import { PricingCardHeader } from "./PricingCardHeader";
import AccountBalanceIcon from "@material-ui/icons/AccountBalance";
import FreeBreakfastIcon from "@material-ui/icons/FreeBreakfast";
import CardMembershipIcon from "@material-ui/icons/CardMembership";
import { PricingCardBody } from "./PricingCardBody";
import AllInclusiveIcon from "@material-ui/icons/AllInclusive";
import WhatshotIcon from '@material-ui/icons/Whatshot';

const useStyles = makeStyles((theme) => ({
    yes: {
        color: green,
    },
    no: {
        color: red,
    },
    unlimited: {
        color: theme.palette.success.main,
        margin: "-4px",
        padding: "0px",
    },
    icon: {
        marginTop: "2rem",
        marginBottom: "1rem",
        fontSize: "45pt",
    },
    paperCommon: {
        margin: "1rem",
        textAlign: "center",
        borderRadius: "5px",
        alignContent: "center",
        marginTop: "3rem",
        boxShadow: theme.shadows[10],
        color: "white",
        paddingBottom: "1rem",
    },
    paperFree: {
        backgroundColor: theme.palette.primary.light, //"#0093CB",
    },
    paperPremium: {
        backgroundColor: theme.palette.primary.main, //"#014B91",
    },
    paperPro: {
        backgroundColor: theme.palette.primary.dark, //"#011E6D",
    },
}));

const checkIcon = () => {
    const cls = useStyles();
    return <CheckCircleIcon className={cls.yes} />;
};

const crossIcon = () => {
    const cls = useStyles();
    return <NotInterestedIcon className={cls.no} />;
};

const unlimitedIcon = () => {
    const cls = useStyles();
    return <AllInclusiveIcon className={cls.unlimited} />;
};

export const Free = () => {
    const cls = useStyles();

    const header = <PricingCardHeader icon={<FreeBreakfastIcon className={cls.icon} />} title="Free" currency="Free" amount="" />;
    const body = (
        <PricingCardBody
            response={checkIcon()}
            perAreaEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            imageUpload={crossIcon()}
            emailNotifications={crossIcon()}
            inlineEmailEditor={crossIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerArea="0"
            staticFeeTables="1"
            dynamicFeeTables="1"
            numberOfAreas="2"
        />
    );

    return (
        <Paper className={classnames(cls.paperFree, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};

export interface PriceInfoProps {
    priceInfo?: boolean;
    showYearly?: boolean;
}
export const Lyte = ({ priceInfo, showYearly }: PriceInfoProps) => {
    const cls = useStyles();
    const header = <PricingCardHeader priceInfo={priceInfo} icon={<CardMembershipIcon className={cls.icon} />} title="Lyte" currency="$" amount="20" per showYearly={showYearly} yearlyAmount="240" />;
    const body = (
        <PricingCardBody
            response={checkIcon()}
            perAreaEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            imageUpload={crossIcon()}
            emailNotifications={crossIcon()}
            inlineEmailEditor={crossIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerArea="0"
            staticFeeTables="2"
            dynamicFeeTables="2"
            numberOfAreas="4"
        />
    );
    return (
        <Paper className={classnames(cls.paperPremium, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};

export const Premium = ({ priceInfo, showYearly }: PriceInfoProps) => {
    const cls = useStyles();
    const theme = useTheme();
    const header = <PricingCardHeader priceInfo={priceInfo} icon={<WhatshotIcon className={cls.icon} />} title="Premium" currency="$" amount="50" per showYearly={showYearly} yearlyAmount="550" />;
    const body = (
        <PricingCardBody
            response={checkIcon()}
            perAreaEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            imageUpload={checkIcon()}
            emailNotifications={checkIcon()}
            inlineEmailEditor={checkIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerArea="2"
            staticFeeTables="2"
            dynamicFeeTables="2"
            numberOfAreas="8"
            rowStyle={{ borderBottom: "2px solid black" }}
            textStyle={{ color: "black" }}
        />
    );

    return (
        <Paper style={{ backgroundColor: theme.palette.success.light, color: "black" }} className={classnames(cls.paperPro, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};

export const Pro = ({ priceInfo, showYearly }: PriceInfoProps) => {
    const cls = useStyles();

    const header = <PricingCardHeader priceInfo={priceInfo} icon={<AccountBalanceIcon className={cls.icon} />} title="Pro" currency="$" amount="75" per showYearly={showYearly} yearlyAmount="800" />;
    const body = (
        <PricingCardBody
            response={checkIcon()}
            perAreaEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            imageUpload={checkIcon()}
            emailNotifications={checkIcon()}
            inlineEmailEditor={checkIcon()}
            smsNotifications={checkIcon()}
            attachmentsPerArea={unlimitedIcon()}
            staticFeeTables={unlimitedIcon()}
            dynamicFeeTables={unlimitedIcon()}
            numberOfAreas={unlimitedIcon()}
        />
    );
    return (
        <Paper style={{ borderColor: "gold", borderWidth: "3px" }} className={classnames(cls.paperPro, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};
