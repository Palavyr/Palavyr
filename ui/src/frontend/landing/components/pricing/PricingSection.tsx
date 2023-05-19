import React from "react";
import { Hidden, makeStyles } from "@material-ui/core";
import { Free, Lyte, Premium, Pro } from "./Cards";

const useStyles = makeStyles<{}>((theme: any) => ({
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
    bodySmall: {
        display: "felx",
        flexDirection: "column",
        margin: "0.5rem",
    },
}));

export const PricingSection = () => {
    const cls = useStyles();

    return (
        <>
            <Hidden smDown>
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
            </Hidden>
            <Hidden mdUp>
                <section className={cls.bodySmall}>
                    <Free />
                    <Lyte />
                    <Premium />
                    <Pro />
                </section>
            </Hidden>
        </>
    );
};
