import React, { useState, useCallback } from "react";
import { makeStyles, Dialog, DialogContent, Typography, TextField, DialogActions, Button } from "@material-ui/core";
import { ButtonCircularProgress } from "./ButtonCircularProgress";
import { LoginClient } from "@api-client/LoginClient";
import { VERIFICATION_EMAIL_SEND } from "@constants";
import { useEffect } from "react";

export interface IChangePasswordDialog {
    onClose: any;
    setLoginStatus: any;
}

const useStyles = makeStyles((theme) => ({
    dialogContent: {
        paddingTop: theme.spacing(2),
    },
    dialogActions: {
        paddingTop: theme.spacing(2),
        paddingBottom: theme.spacing(2),
        paddingRight: theme.spacing(2),
    },
}));

export const ChangePasswordDialog = ({ setLoginStatus, onClose }: IChangePasswordDialog) => {
    const classes = useStyles();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [emailAddress, setEmailAddress] = useState("start");
    const [errorMessage, setErrorMessage] = useState<string>("");
    const [requestSent, setRequestSent] = useState<boolean>(false);
    console.log("A render");

    const sendPasswordEmail = useCallback(async () => {
        const client = new LoginClient();
        console.log("Emaill Addy at start of send: " + emailAddress);
        setIsLoading(true);

        const { data: resetEmailResponse } = await client.Reset.resetPasswordRequest(emailAddress);
        setTimeout(() => {
            if (resetEmailResponse.status) {
                setLoginStatus(VERIFICATION_EMAIL_SEND);
                onClose(); // opens the login dialog
                setIsLoading(false);
            } else {
                setErrorMessage(resetEmailResponse.message);
                setIsLoading(false);
            }
            setRequestSent(true);
        }, 1500);
    }, [setIsLoading, setLoginStatus, onClose, emailAddress]);

    useEffect(() => {
        return () => {
            setRequestSent(false);
        };
    }, []);

    const RenderSubmitButtons = () => {
        return (
            <>
                <Button onClick={onClose} disabled={isLoading || requestSent}>
                    Cancel
                </Button>
                <Button
                    onClick={sendPasswordEmail}
                    // type="submit"
                    variant="contained"
                    color="secondary"
                    disabled={isLoading || requestSent}
                >
                    Reset password
                    {isLoading && <ButtonCircularProgress />}
                </Button>
            </>
        );
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        console.log("setting state: " + event.target.value);
        setEmailAddress(event.target.value);
        console.log("Currentstate: " + emailAddress);
    };

    const RenderCloseButton = () => {
        return <Button onClick={onClose}>Close</Button>;
    };

    return (
        <Dialog open hideBackdrop onClose={onClose} disableBackdropClick={isLoading} disableEscapeKeyDown={isLoading} maxWidth="xs">
            {/* <form
                onSubmit={(e) => {
                    e.preventDefault();
                    sendPasswordEmail();
                }}
            > */}
            <DialogContent className={classes.dialogContent}>
                <Typography paragraph>Enter your email address below and we will send you instructions on how to reset your password.</Typography>
                <TextField disabled={isLoading || requestSent} variant="outlined" margin="dense" required fullWidth label="Email Address" autoFocus type="email" autoComplete="off" helperText={errorMessage} onChange={handleChange} />
            </DialogContent>
            <DialogActions className={classes.dialogActions}>{requestSent ? <RenderCloseButton /> : <RenderSubmitButtons />}</DialogActions>
            {/* </form> */}
        </Dialog>
    );
};
