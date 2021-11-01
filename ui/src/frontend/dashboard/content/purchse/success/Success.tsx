import React from "react";
import { useHistory } from "react-router-dom";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(() => ({
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

export const Success = () => {
    const cls = useStyles();
    const history = useHistory();

    return (
        <div className={cls.outer}>
            <h1>Thank you so much for purchasing a subscription!</h1>
            <p>Let us know right away if you encounter any problems or have any feature requests!</p>
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
