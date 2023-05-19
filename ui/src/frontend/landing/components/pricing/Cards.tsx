import React from "react";
import { makeStyles, Paper, useTheme } from "@material-ui/core";
import { PricingCard } from "./PricingCard";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import NotInterestedIcon from "@material-ui/icons/NotInterested";
import { green, red } from "frontend/theme";
import classnames from "classnames";
import { PricingCardHeader } from "./PricingCardHeader";
import AccountBalanceIcon from "@material-ui/icons/AccountBalance";
import FreeBreakfastIcon from "@material-ui/icons/FreeBreakfast";
import CardMembershipIcon from "@material-ui/icons/CardMembership";
import WhatshotIcon from "@material-ui/icons/Whatshot";
import { PricingCardBody } from "./PricingCardBody";
import AllInclusiveIcon from "@material-ui/icons/AllInclusive";
import Filter1Icon from "@material-ui/icons/Filter1";
import Filter2Icon from "@material-ui/icons/Filter2";
import Filter4Icon from "@material-ui/icons/Filter4";
import Filter8Icon from "@material-ui/icons/Filter8";
import ExposureZeroIcon from "@material-ui/icons/ExposureZero";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
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
            perIntentEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            fileAssetUpload={crossIcon()}
            emailNotifications={crossIcon()}
            inlineEmailEditor={crossIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerIntent={<ExposureZeroIcon />}
            staticFeeTables={<Filter1Icon />}
            dynamicFeeTables={<Filter1Icon />}
            numberOfIntents={<Filter2Icon />}
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
            perIntentEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            fileAssetUpload={crossIcon()}
            emailNotifications={crossIcon()}
            inlineEmailEditor={crossIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerIntent={<ExposureZeroIcon />}
            staticFeeTables={<Filter2Icon />}
            dynamicFeeTables={<Filter2Icon />}
            numberOfIntents={<Filter4Icon />}
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
            perIntentEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            fileAssetUpload={checkIcon()}
            emailNotifications={checkIcon()}
            inlineEmailEditor={checkIcon()}
            smsNotifications={crossIcon()}
            attachmentsPerIntent={<Filter2Icon />}
            staticFeeTables={<Filter2Icon />}
            dynamicFeeTables={<Filter2Icon />}
            numberOfIntents={<Filter8Icon />}
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
            perIntentEmail={checkIcon()}
            enquiriesDashboard={checkIcon()}
            fileAssetUpload={checkIcon()}
            emailNotifications={checkIcon()}
            inlineEmailEditor={checkIcon()}
            smsNotifications={checkIcon()}
            attachmentsPerIntent={unlimitedIcon()}
            staticFeeTables={unlimitedIcon()}
            dynamicFeeTables={unlimitedIcon()}
            numberOfIntents={unlimitedIcon()}
        />
    );
    return (
        <Paper style={{ borderColor: "gold", borderWidth: "3px" }} className={classnames(cls.paperPro, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};
