import { makeStyles } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles(theme => ({
    outer: {
        margin: "3rem",
        padding: "3rem"
    }
}))
export const Cancel = () => {
    const cls = useStyles();
    return (
        <div className={cls.outer}>
            <h1>Don't worry - you've cancelled the transaction :D</h1>
            <p>You haven't paid anything to use. If you change your mind, head back to the Subscribe page.</p>
        </div>
    );
};
