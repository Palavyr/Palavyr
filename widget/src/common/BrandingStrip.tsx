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
}));
export const BrandingStrip = () => {
    const cls = useStyles();
    return (
        <Typography className={cls.wrapper} variant="caption" align="center" display="inline">
            Crafted with <strong>Palavyr</strong>
        </Typography>
    );
};
