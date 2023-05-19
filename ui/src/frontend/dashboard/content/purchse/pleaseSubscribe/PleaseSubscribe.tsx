import React from "react";
import { useHistory } from "react-router-dom";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles<{}>(() => ({
    center: {
        display: "flex",
        textAlign: "center",
        justifyContent: "center",
        flexDirection: "column",
    },
    card: {
        margin: "2rem",
        padding: "1rem",
    },
    outer: {
        margin: "3rem",
        padding: "3rem",
    },
}));

export const PleaseSubscribe = () => {
    const cls = useStyles();
    const history = useHistory();

    return (
        <div className={cls.outer}>
            <h1>Thank you so much for your interest in Palavyr!</h1>
            <p>The feature you're attempting to use is a paid feature. If you'd like to use it, please consider ordering a subscription.</p>
            <SinglePurposeButton
                variant="outlined"
                color="primary"
                buttonText="Return to Dashboard"
                disabled={false}
                onClick={() => {
                    history.push("/dashboard");
                }}
            />
        </div>
    );
};
