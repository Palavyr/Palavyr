import React from "react";
import { makeStyles, Typography } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    fallback: {
        margin: "2rem",
    },
}));

export const NoActivityComponent = () => {
    const cls = useStyles();
    return (
        <span className={cls.fallback}>
            <Typography align="center" gutterBottom>
                Things seems a little quite with the widget...
            </Typography>
        </span>
    );
};
