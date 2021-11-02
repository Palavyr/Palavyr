import { LoginRepository } from "@common/client/LoginRepository";
import { RESET_PASSWORD_FORM } from "@constants";
import { Typography } from "@material-ui/core";
import React, { useEffect } from "react";
import { useCallback } from "react";
import { useHistory, useLocation } from "react-router-dom";

export const ConfirmYourResetLink = () => {
    const location = useLocation();
    const history = useHistory();

    const searchParams = new URLSearchParams(location.search);
    const secretId = searchParams.get("id") as string; // from url from email

    const sendVerificationRequest = useCallback(async () => {
        const client = new LoginRepository();
        const response = await client.Reset.verifyResetIdentity(secretId);

        if (response.status) {
            setTimeout(() => {
                history.push(`${RESET_PASSWORD_FORM}?key=${response.apiKey}`);
            }, 2000);
        } else {
        }
    }, []);

    useEffect(() => {
        sendVerificationRequest();
    }, []);

    return (
        <div style={{ textAlign: "center", paddingTop: "2rem" }}>
            <Typography variant="h2">Palavyr.com password reset</Typography>
            <h3>Confirming your identity...</h3>
        </div>
    );
};
