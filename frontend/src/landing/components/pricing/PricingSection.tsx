import React from "react";
import { makeStyles } from "@material-ui/core";
import { Free, Lyte, Premium, Pro } from "./Cards";

const useStyles = makeStyles((theme) => ({
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        background: theme.palette.background.default,
        borderRadius: "4px",
        paddingLeft: "3rem",
        paddingRight: "3rem",
        paddingBottom: "2rem",
    },
    width: {
        width: "28%",
    },
}));

export const PricingSection = () => {
    const cls = useStyles();

    return (
        <section className={cls.body}>
            <div data-aos="fade-down" data-aos-delay="100" className={cls.width}>
                <Free />
            </div>
            <div data-aos="fade-down" data-aos-delay="300" className={cls.width}>
                <Lyte />
            </div>
            <div data-aos="fade-down" data-aos-delay="500" className={cls.width}>
                <Premium />
            </div>
            <div data-aos="fade-down" data-aos-delay="700" className={cls.width}>
                <Pro />
            </div>
        </section>
    );
};
