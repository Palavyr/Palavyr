import React, { useState, useCallback, useRef } from "react";
import { makeStyles, useTheme, TextField, FormControlLabel, Checkbox, Typography, FormHelperText, Button } from "@material-ui/core";
import { FormDialog } from "@common/components/borrowed/FormDialog";
import { VisibilityPasswordTextField } from "@common/components/borrowed/VisibilityPasswordTextField";
import { HighlightedInformation } from "@common/components/borrowed/HighlightedInformation";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { TermsOfServiceDialog } from "legal/TermsOfService";
import { ApiClient } from "@api-client/Client";
import Auth from "auth/Auth";
import { useHistory } from "react-router-dom";
import { LocalStorage } from "localStorage/localStorage";



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
        },
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

    const success = () => {
        setTimeout(() => {
            history.push("/dashboard/editor");
        }, 150);
    }

    const error = (response) => {
        if (response.message === "Password does not match.") {
            console.log(response.message)
        } else if (response.message === "Could not find user.") {
            console.log(response.message)
        }
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
                var res = await Auth.register(registerEmail.value, registerPassword.value, success, error);
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
            // hasCloseIcon
            content={
                <>
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
                    {status === "accountCreated" ? (
                        <HighlightedInformation>
                            We have created your account. Please click on the link in the
                            email we have sent to you before logging in.
                        </HighlightedInformation>
                    ) : (
                            <HighlightedInformation>
                                Registration is currently in Demo Mode. <br /> <strong>Accounts are subject to arbitrary deletion.</strong>
                            </HighlightedInformation>
                        )}
                </>
            }
            actions={
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    size="large"
                    color="secondary"
                    disabled={isLoading}
                >
                    Register
                    {isLoading && <ButtonCircularProgress />}
                </Button>
            }
        />
    );
}
