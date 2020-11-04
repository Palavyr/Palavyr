import * as React from "react";
import { HelpTypes } from "dashboard/layouts/DashboardLayout";
import { FreeCard } from "@landing/components/pricing/FreeCard";
import { PremiumCard } from "@landing/components/pricing/PremiumCard";
import { ProCard } from "@landing/components/pricing/ProCard";
import { makeStyles, Paper, Slide } from "@material-ui/core";
import classNames from "classnames";
import { pricingContainerStyles } from "@landing/components/pricing/cardStyles";
import { useHistory } from "react-router-dom";


const useStyles = makeStyles((theme) => ({
    root: {
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
        paddingRight: "14%",
        paddingLeft: "14%",
        borderRadius: "4px",
    },
    card: {
        marginTop: "5rem",
        "&:hover": {
            transition: "margin-top 3s",
            background: "navy",
            border: "3px solid black",
            marginTop: "1rem"
        }
    }
}));

type Loc = "free" | "premium" | "pro";

interface ISubscribe {
    setHelpType(helpType: HelpTypes): void;
}
export const Subscribe = ({ setHelpType }: ISubscribe) => {
    setHelpType("subscribe");
    const cls = useStyles();
    const containerCls = pricingContainerStyles();

    const history = useHistory();

    const purchase = (loc: Loc) => {
        history.push(`/dashboard/purchase?${loc}`)
    }


    return (
        <div className={classNames(cls.root, cls.body)}>
            <Paper onClick={() => alert("OK")} data-aos="fade-down" data-aos-delay="100" className={classNames(cls.card, containerCls.paperCommon, containerCls.paperFree)} variant="outlined">
                <FreeCard />
            </Paper>

            <Paper onClick={() => alert("OK")} data-aos="fade-down" data-aos-delay="100" className={classNames(cls.card, containerCls.paperCommon, containerCls.paperPremium)} variant="outlined">
                <PremiumCard />
            </Paper>

            <Paper onClick={() => alert("OK")} data-aos="fade-down" data-aos-delay="100" className={classNames(cls.card, containerCls.paperCommon, containerCls.paperPro)} variant="outlined">
                <ProCard />
            </Paper>
        </div>
    );
};
