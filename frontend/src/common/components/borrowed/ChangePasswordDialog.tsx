import React, { useState, useCallback } from "react";
import { makeStyles, Dialog, DialogContent, Typography, TextField, DialogActions, Button } from "@material-ui/core";
import { ButtonCircularProgress } from "./ButtonCircularProgress";

export interface IChangePasswordDialog {
    onClose: any;
    setLoginStatus: any;
    handleResetPassword: any;
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

export const ChangePasswordDialog = ({ setLoginStatus, onClose, handleResetPassword }: IChangePasswordDialog) => {

    const classes = useStyles();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [emailAddress, setEmailAddress] = useState<string>("")


    const sendPasswordEmail = useCallback(() => {
        setIsLoading(true);
        setTimeout(() => {
            setLoginStatus("verificationEmailSend");
            setIsLoading(false);
            onClose();
        }, 1500);
    }, [setIsLoading, setLoginStatus, onClose]);

    return (
        <Dialog
            open
            hideBackdrop
            onClose={onClose}
            disableBackdropClick={isLoading}
            disableEscapeKeyDown={isLoading}
            maxWidth="xs"
        >
            <form
                onSubmit={(e) => {
                    e.preventDefault();
                    sendPasswordEmail();
                }}
            >
                <DialogContent className={classes.dialogContent}>
                    <Typography paragraph>
                        Enter your email address below and we will send you instructions on
                        how to reset your password.
                    </Typography>
                    <TextField
                        variant="outlined"
                        margin="dense"
                        required
                        fullWidth
                        label="Email Address"
                        autoFocus
                        type="email"
                        autoComplete="off"
                        onChange={
                            (e) => setEmailAddress(e.target.value)
                        }
                    />
                </DialogContent>
                <DialogActions className={classes.dialogActions}>
                    <Button
                        onClick={onClose}
                        disabled={isLoading}>
                        Cancel
                    </Button>
                    <Button
                        type="submit"
                        variant="contained"
                        color="secondary"
                        disabled={isLoading}
                    >
                        Reset password
                        {isLoading && <ButtonCircularProgress />}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
}
