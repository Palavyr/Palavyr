import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles(theme => ({
    brandName: {
        // color: "",
        fontFamily: "Poppins",
    },
    leadingText: {
        fontFamily: "Poppins",
    },
}));
export const BrandingStrip = () => {
    const cls = useStyles();
    return (
        <div>
            <Typography display="inline" className={cls.leadingText}>
                Powered By
            </Typography>
            <Typography display="inline" className={cls.brandName}>
                 Palavyr
            </Typography>
        </div>
    );
};
