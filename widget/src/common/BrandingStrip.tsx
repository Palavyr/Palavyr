import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles(theme => ({
    leadingText: {},
    wrapper: {
        fontFamily: "Poppins",
        display: "static",
        bottom: "0px",
        alignText: "center",
    },
    brand: {
        "&:hover": {
            cursor: "pointer"
        }
    }
}));
export const BrandingStrip = () => {
    const cls = useStyles();
    return (
        <Typography className={cls.wrapper} variant="caption" align="center" display="inline">
            Crafted with <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")} >Palavyr</strong>
        </Typography>
    );
};
