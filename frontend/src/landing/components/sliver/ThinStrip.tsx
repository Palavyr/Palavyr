import { makeStyles } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    sliverDiv: {
        backgroundColor: theme.palette.success.light,
        textAlign: "center",
        height: "15px",
        width: "100%",
    },
}));

export const GreenStrip = () => {
    const classes = useStyles();

    return <div className={classes.sliverDiv} />;
};
