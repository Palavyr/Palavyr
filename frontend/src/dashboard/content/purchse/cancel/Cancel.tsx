import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles } from "@material-ui/core";
import React from "react";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
    outer: {
        margin: "3rem",
        padding: "3rem",
    },
}));
export const Cancel = () => {
    const cls = useStyles();
    const history = useHistory();
    return (
        <div className={cls.outer}>
            <h1>Don't worry - you've cancelled the transaction :D</h1>
            <p>You haven't paid anything to use. If you change your mind, head back to the Subscribe page.</p>
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
