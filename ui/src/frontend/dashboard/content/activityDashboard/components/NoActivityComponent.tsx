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
                No data... yet! You'll see plots here once there is some usage data to report.
            </Typography>
        </span>
    );
};
