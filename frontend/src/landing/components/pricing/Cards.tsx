import React from "react";
import { makeStyles, Paper } from "@material-ui/core";
import { PricingCard } from "./PricingCard";
import { PricingCardTableRow } from "./PricingCardTableRow";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import NotInterestedIcon from "@material-ui/icons/NotInterested";
import { green, red } from "theme";
import classnames from "classnames";
import { PricingCardHeader } from "./PricingCardHeader";
import AccountBalanceIcon from "@material-ui/icons/AccountBalance";
import FreeBreakfastIcon from "@material-ui/icons/FreeBreakfast";
import CardMembershipIcon from "@material-ui/icons/CardMembership";
import { PricingCardBody } from "./PricingCardBody";

const useStyles = makeStyles((theme) => ({
    yes: {
        color: green,
    },
    no: {
        color: red,
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

const checkIcon = (yes: string) => <CheckCircleIcon className={yes} />;
const crossIcon = (no: string) => <NotInterestedIcon className={no} />;

export const Free = () => {
    const cls = useStyles();

    const header = <PricingCardHeader icon={<FreeBreakfastIcon className={cls.icon} />} title="Lyte" currency="Free" amount="" />;
    const body = <PricingCardBody editor={crossIcon(cls.no)} response={checkIcon(cls.yes)} email={checkIcon(cls.yes)} area="2" enquiries={crossIcon(cls.no)} attachments={crossIcon(cls.no)} table="1" />;
    return (
        <Paper className={classnames(cls.paperFree, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};

export const Premium = () => {
    const cls = useStyles();
    const header = <PricingCardHeader icon={<CardMembershipIcon className={cls.icon} />} title="Premium" currency="$" amount="50" per />;
    const body = <PricingCardBody editor={checkIcon(cls.yes)} response={checkIcon(cls.yes)} email={checkIcon(cls.yes)} area="6" enquiries={checkIcon(cls.yes)} attachments={checkIcon(cls.yes)} table="2" />;

    return (
        <Paper className={classnames(cls.paperPremium, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};

export const Pro = () => {
    const cls = useStyles();

    const header = <PricingCardHeader icon={<AccountBalanceIcon className={cls.icon} />} title="Pro" currency="$" amount="75" per />;
    const body = <PricingCardBody editor={checkIcon(cls.yes)} response={checkIcon(cls.yes)} email={checkIcon(cls.yes)} area="Unlimited" enquiries={checkIcon(cls.yes)} attachments={checkIcon(cls.yes)} table="Unlimited" />;

    return (
        <Paper className={classnames(cls.paperPro, cls.paperCommon)}>
            <PricingCard header={header} body={body} />
        </Paper>
    );
};
