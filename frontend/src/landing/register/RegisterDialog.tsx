import React, { useState, useCallback, useRef } from "react";
import { makeStyles, useTheme, TextField, FormControlLabel, Checkbox, Typography, FormHelperText, Button } from "@material-ui/core";
import { FormDialog } from "@common/components/borrowed/FormDialog";
import { VisibilityPasswordTextField } from "@common/components/borrowed/VisibilityPasswordTextField";
import { HighlightedInformation } from "@common/components/borrowed/HighlightedInformation";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import Auth from "auth/Auth";
import { useHistory } from "react-router-dom";
import GoogleLogin, { GoogleLoginProps, GoogleLoginResponse } from "react-google-login";
import { DividerWithText } from "@common/components/DividerWithText";
import { googleOAuthClientId } from "@api-client/clientUtils";


const useStyles = makeStyles((theme) => ({
    link: {
        transition: theme.transitions.create(["background-color"], {
            duration: theme.transitions.duration.complex,
            easing: theme.transitions.easing.easeInOut,
        }),
        cursor: "pointer",
        color: theme.palette.primary.main,
        "&:enabled:hover": {
            color: theme.palette.primary.dark,
        },
        "&:enabled:focus": {
            color: theme.palette.primary.dark,
        }
    },
    registerbutton: {
        color: "white",
        backgroundColor: "#3e5f82",
    },
    googlebutton: {
        textAlign: "center",
        width: "100%",
        marginBottom: "1rem"
    },

}));



export type RegisterFormStatusTypes = "passwordsDontMatch" | "passwordTooShort" | "invalidEmail" | null;

export interface IRegisterDialog {
    setStatus: (status: RegisterFormStatusTypes) => void;
    onClose: any;
    openTermsDialog: any;
    status: string | null;
}

export const RegisterDialog = ({ onClose, openTermsDialog, status, setStatus }: IRegisterDialog) => {

    const theme = useTheme();
    const classes = useStyles();
    const history = useHistory();

    const [isLoading, setIsLoading] = useState(false);
    const [hasTermsOfServiceError, setHasTermsOfServiceError] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);

    const registerTermsCheckboxRef = useRef<HTMLInputElement | null>(null);
    const registerPasswordRef = useRef<HTMLInputElement | null>(null);
    const registerPasswordRepeatRef = useRef<HTMLInputElement | null>(null);
    const registerEmailRef = useRef<HTMLInputElement | null>(null);

    const defaultSuccess = () => {
        setTimeout(() => {
            history.push("/dashboard/editor");
        }, 150);
    }

    const defaultError = (response) => {
        console.log(response);
        alert("Error registering: " + response)
    }


    const register = useCallback(() => {

        const termsCheckBoxRef = registerTermsCheckboxRef.current;
        const registerPassword = registerPasswordRef.current;
        const registerEmail = registerEmailRef.current;

        if (termsCheckBoxRef && !termsCheckBoxRef.checked) {
            setHasTermsOfServiceError(true);
            return;
        }
        if (registerPassword && registerPassword.value !== registerPassword.value) {
            setStatus("passwordsDontMatch");
            return;
        }

        if (registerEmail && (registerEmail.value === "" || registerEmail.value === null)) {
            setStatus("invalidEmail")
            return;
        }

        setStatus(null);
        setIsLoading(true);
        setTimeout(async () => {
            setIsLoading(false);
            if (registerEmail !== null && registerPassword !== null) {
                var res = await Auth.register(registerEmail.value, registerPassword.value, defaultSuccess, defaultError);

            }
        }, 1500);
    }, [
        setIsLoading,
        setStatus,
        setHasTermsOfServiceError,
        registerPasswordRef,
        registerPasswordRepeatRef,
        registerTermsCheckboxRef,
    ]);

    const passwordHelperText = () => {
        if (status === "passwordTooShort") {
            return "Create a password at least 6 characters long.";
        }
        if (status === "passwordsDontMatch") {
            return "Your passwords dont match.";
        }
        return null;
    }

    const TermsOfServiceLink = () => {
        return (
            <Typography variant="body1">
                I agree to the
                <span className={classes.link} onClick={isLoading ? null : openTermsDialog} tabIndex={0} role="button"
                    onKeyDown={(event) => {
                        // For screenreaders listen to space and enter events
                        if (
                            (!isLoading && event.keyCode === 13) ||
                            event.keyCode === 32
                        ) {
                            openTermsDialog();
                        }
                    }}
                >
                    {" "}
                terms of service
                </span>
            </Typography>
        )
    }

    const passwordOnChange = () => {
        if (status === "passwordTooShort" || status === "passwordsDontMatch") {
            setStatus(null);
        }
    }

    const registerGoogleSuccess = () => {
        defaultSuccess();
    }

    const registerGoogleError = (response) => {
        defaultError(response.message);
    }

    const responseGoogleSuccess = useCallback(async (response: GoogleLoginResponse) => {
        const termsCheckBoxRef = registerTermsCheckboxRef.current;
        if (termsCheckBoxRef && !termsCheckBoxRef.checked) {
            setHasTermsOfServiceError(true);
            return;
        }
        if (response) {
            var oneTimeCode = response.tokenId;
            var accessToken = response.accessToken;
            var other = response.googleId;
            var res = await Auth.registerWithGoogle(oneTimeCode, accessToken, other, registerGoogleSuccess, registerGoogleError);
        } else {
            alert("Account not recognized")
        }
    }, [])

    const responseGoogleFailure = () => {
        alert("Google rego failed.")
    }


    return (
        <FormDialog
            loading={isLoading}
            onClose={onClose}
            open
            headline="Register"
            onFormSubmit={(e) => {
                e.preventDefault();
                register();
            }}
            hideBackdrop
            content={
                <>
                    <div className={classes.googlebutton} >
                        <GoogleLogin
                            theme="dark"
                            clientId={googleOAuthClientId}
                            buttonText="Sign up with Google"
                            onSuccess={responseGoogleSuccess}
                            onFailure={responseGoogleFailure}
                        />
                    </div>
                    <br></br>
                    <DividerWithText text={"OR"} />
                    <br></br>
                    <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        error={status === "invalidEmail"}
                        label="Email Address"
                        inputRef={registerEmailRef}
                        autoFocus
                        autoComplete="off"
                        type="email"
                        onChange={() => {
                            if (status === "invalidEmail") {
                                setStatus(null);
                            }
                        }}
                        FormHelperTextProps={{ error: true }}
                    />
                    <VisibilityPasswordTextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        error={
                            status === "passwordTooShort" || status === "passwordsDontMatch"
                        }
                        label="Password"
                        inputRef={registerPasswordRef}
                        autoComplete="off"
                        onChange={passwordOnChange}
                        FormHelperTextProps={{ error: true }}
                        isVisible={isPasswordVisible}
                        onVisibilityChange={setIsPasswordVisible}
                    />
                    <VisibilityPasswordTextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        error={
                            status === "passwordTooShort" || status === "passwordsDontMatch"
                        }
                        label="Repeat Password"
                        inputRef={registerPasswordRepeatRef}
                        autoComplete="off"
                        onChange={passwordOnChange}
                        helperText={passwordHelperText()}
                        FormHelperTextProps={{ error: true }}
                        isVisible={isPasswordVisible}
                        onVisibilityChange={setIsPasswordVisible}
                    />
                    <FormControlLabel
                        style={{ marginRight: 0 }}
                        control={
                            <Checkbox
                                color="primary"
                                inputRef={registerTermsCheckboxRef}
                                onChange={() => {
                                    setHasTermsOfServiceError(false);
                                }}
                            />
                        }
                        label={<TermsOfServiceLink />}
                    />
                    {hasTermsOfServiceError && (
                        <FormHelperText error style={{ display: "block", marginTop: theme.spacing(-1) }}>
                            In order to create an account, you have to accept our terms of
                            service.
                        </FormHelperText>
                    )}
                    {/* {status === "accountCreated" ? (
                        <HighlightedInformation>
                            We have created your account. Please click on the link in the
                            email we have sent to you before logging in.
                        </HighlightedInformation>
                    ) : (
                            <HighlightedInformation>
                                Registration is currently in Demo Mode. <br /> <strong>Accounts are subject to arbitrary deletion.</strong>
                            </HighlightedInformation>
                        )} */}
                </>
            }
            actions={
                <Button
                    className={classes.registerbutton}
                    type="submit"
                    fullWidth
                    variant="contained"
                    size="large"
                    // color="secondary"
                    disabled={isLoading}
                >
                    Register
                    {isLoading && <ButtonCircularProgress />}
                </Button>
            }
        />
    );
}
