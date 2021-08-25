import { makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { SpaceEvenly } from "./SpaceEvenly";

const useStyles = makeStyles(theme => ({
    leadingText: {},
    wrapper: {
        fontFamily: "Poppins",
        // display: "flex",
        // justifyContent: "center",
        // bottom: "0px",
        // alignText: "center",
        // height: "30px",
        justifyItems: "center"
    },
    brand: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    spacer: {
        height: "7%",
        width: "100%",
        backgroundColor: "#264B94",
        color: "white",
        justifyItems: "center"
    },
}));
export const BrandingStrip = () => {
    const cls = useStyles();
    return (
        <SpaceEvenly vertical classes={cls.spacer} center>
            <Typography className={cls.wrapper} variant="caption" align="center" display="inline">
                Crafted with{" "}
                <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")}>
                    Palavyr
                </strong>
            </Typography>
        </SpaceEvenly>
    );
};
