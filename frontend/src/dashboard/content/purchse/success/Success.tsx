import React from "react";
import { ApiClient } from "@api-client/Client";
import { useEffect } from "react";
import { useCallback } from "react";
import { useHistory, useLocation } from "react-router-dom";
import { useState } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Card, makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    center: {
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
    },
    card: {
        margin: "2rem",
        padding: "1rem"
    }
}));

export const Success = () => {
    const client = new ApiClient();
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const successSessionId = searchParams.get("session_id") as string;
    const cls = useStyles();
    const history = useHistory();

    return (
        <div className={cls.center}>
            <Card className={cls.card}>
                <h1>Success!</h1>
                <p>Now you can send a post with the success session ID back to the server and use it to update the database.</p>
                <p>SessionID: {successSessionId}</p>
                <SinglePurposeButton
                    variant="outlined"
                    color="primary"
                    buttonText="Return to Dashboard"
                    disabled={false}
                    onClick={() => {
                        history.push("/dashboard/subscribe");
                    }}
                />
            </Card>
        </div>
    );
};