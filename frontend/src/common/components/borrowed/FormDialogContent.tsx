import * as React from 'react';
import { TextField, FormControlLabel, Checkbox, Typography, makeStyles } from '@material-ui/core';
import { VisibilityPasswordTextField } from './VisibilityPasswordTextField';
import { HighlightedInformation } from './HighlightedInformation';

export type FormStatusTypes = "invalidEmail" | "invalidPassword" | "verificationEmailSend" | null;



export interface IFormDialogContent {
    loginEmail: string;
    loginPassword: string;
    setLoginEmail: any;
    setLoginPassword: any;
    setStatus: any;
    isPasswordVisible: boolean;
    setIsPasswordVisible: any
    status: FormStatusTypes;
}

const useStyles = makeStyles(({
    formControlLabel: {
        marginRight: 0,
    },
}))

export const FormDialogContent = ({ loginEmail, setLoginEmail, loginPassword, setLoginPassword, isPasswordVisible, setIsPasswordVisible, setStatus, status }: IFormDialogContent) => {

    const classes = useStyles();

    return (
        <>
            <TextField
                variant="outlined"
                margin="normal"
                error={status === "invalidEmail"}
                required
                fullWidth
                label="Email Address"
                value={loginEmail}
                autoFocus
                autoComplete="off"
                type="email"
                onChange={(e) => {
                    setLoginEmail(e.target.value)
                    if (status === "invalidEmail") {
                        setStatus(null);
                    }
                }}
                helperText={
                    status === "invalidEmail" &&
                    "This email address isn't associated with an account."
                }
                FormHelperTextProps={{ error: true }}
            />
            <VisibilityPasswordTextField
                variant="outlined"
                margin="normal"
                required
                fullWidth
                error={status === "invalidPassword"}
                label="Password"
                value={loginPassword}
                autoComplete="off"
                onChange={(e) => {
                    setLoginPassword(e.target.value)
                    if (status === "invalidPassword") {
                        setStatus(null);
                    }
                }}
                helperText={
                    status === "invalidPassword" ? (
                        <span>
                            Incorrect password. Try again, or click on{" "}
                            <b>&quot;Forgot Password?&quot;</b> to reset it.
                        </span>
                    ) : (
                            ""
                        )
                }
                FormHelperTextProps={{ error: true }}
                onVisibilityChange={setIsPasswordVisible}
                isVisible={isPasswordVisible}
            />
            <FormControlLabel
                className={classes.formControlLabel}
                control={<Checkbox color="primary" />}
                label={<Typography variant="body1">Remember me</Typography>}
            />
            {status === "verificationEmailSend" ? (
                <HighlightedInformation>
                    We have send instructions on how to reset your password to your
                    email address
                </HighlightedInformation>
            ) : (
                    <HighlightedInformation>
                        <b>Access is restricted until we go live.</b>
                        <br />
                    </HighlightedInformation>
                )}
        </>
    )
}