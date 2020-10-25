import * as React from 'react';
import { TextField, FormControlLabel, Checkbox, Typography, makeStyles, Divider } from '@material-ui/core';
import { VisibilityPasswordTextField } from './VisibilityPasswordTextField';
import { HighlightedInformation } from './HighlightedInformation';
import { GoogleLogin, GoogleLoginResponse } from 'react-google-login';
import { DividerWithText } from '../DividerWithText';
import { googleOAuthClientId } from '@api-client/clientUtils';

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
    responseGoogle(response: GoogleLoginResponse): void;
}

const useStyles = makeStyles(({
    formControlLabel: {
        marginRight: 0,
    },
    googlebutton: {
        textAlign: "center",
        width: "100%",
        marginBottom: "1rem"
    },

}))


export const FormDialogContent = ({ loginEmail, setLoginEmail, loginPassword, setLoginPassword, isPasswordVisible, setIsPasswordVisible, responseGoogle, setStatus, status }: IFormDialogContent) => {

    const classes = useStyles();
    return (
        <>
            <div className={classes.googlebutton} >
                <GoogleLogin
                    theme="dark"
                    clientId={googleOAuthClientId}
                    buttonText="Sign in with Google"
                    onSuccess={responseGoogle}
                    onFailure={responseGoogle}
                />
            </div>
            <br></br>
            <DividerWithText text={"OR"} />
            <br></br>
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
                    We have sent instructions on how to reset your password to your
                    email address
                </HighlightedInformation>
            ) : null
            }
        </>
    )
}