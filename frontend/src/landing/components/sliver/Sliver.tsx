import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    sliverDiv: {
        color: "lighgray",
        textAlign: "center",
        height: "15px",
        width: "100%",
        backgroundColor: theme.palette.success.dark,
    },
    sliver: {
        fontSize: "16pt",
    },
}));

export const Sliver = () => {
    const classes = useStyles();

    return <div className={classes.sliverDiv}>{/* <Typography variant="h5" data-aos="fade-right">
                Questions? Get in touch: info.palavyr@gmail.com
            </Typography> */}</div>;
};
