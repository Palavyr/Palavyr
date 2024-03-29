import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { makeStyles } from "@material-ui/core";
import React from "react";
import { useHistory } from "react-router-dom";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
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
            <h1>You've cancelled your transaction :D</h1>
            <p>You haven't paid anything. If you change your mind, head back to the Subscribe page.</p>
            <SinglePurposeButton
                variant="outlined"
                color="primary"
                buttonText="Return to Dashboard"
                disabled={false}
                onClick={() => {
                    history.push("/dashboard/activity");
                }}
            />
        </div>
    );
};
