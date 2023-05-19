import * as React from "react";
import { TextField, FormControlLabel, Checkbox, Typography, makeStyles } from "@material-ui/core";
import { VisibilityPasswordTextField } from "./VisibilityPasswordTextField";
import { HighlightedInformation } from "./HighlightedInformation";
import { FormStatusTypes } from "@Palavyr-Types";
import { INVALID_EMAIL, INVALID_PASSWORD } from "@constants";
import { useEffect } from "react";
import { SessionStorage } from "@localStorage/sessionStorage";

export interface IFormDialogContent {
    loginEmail: string;
    loginPassword: string;
    setLoginEmail: any;
    setLoginPassword: any;
    setStatus: any;
    isPasswordVisible: boolean;
    setIsPasswordVisible: any;
    status: FormStatusTypes;
    rememberMe: boolean;
    setRememberMe(rememberMe: boolean): void;
}

const useStyles = makeStyles<{}>({
    formControlLabel: {
        marginRight: 0,
    },
    centeredItems: {
        textAlign: "center",
        width: "100%",
        marginBottom: "1rem",
    },
    errorText: {
        color: "red",
        fontSize: "11pt",
    },
});

export const FormDialogContent = ({ rememberMe, setRememberMe, loginEmail, setLoginEmail, loginPassword, setLoginPassword, isPasswordVisible, setIsPasswordVisible, setStatus, status }: IFormDialogContent) => {
    const cls = useStyles();

    useEffect(() => {
        const isRemembered = SessionStorage.checkIsRemembered();
        setRememberMe(isRemembered);
    }, []);

    return (
        <>
            <TextField
                variant="outlined"
                margin="normal"
                error={status === INVALID_EMAIL}
                required
                fullWidth
                label="Email Address"
                value={loginEmail}
                autoFocus
                autoComplete="off"
                type="email"
                onChange={e => {
                    setLoginEmail(e.target.value);
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                }}
                helperText={status === INVALID_EMAIL && "This email address isn't associated with an account."}
                FormHelperTextProps={{ error: true }}
            />
            <VisibilityPasswordTextField
                variant="outlined"
                margin="normal"
                required
                fullWidth
                error={status === INVALID_PASSWORD}
                label="Password"
                value={loginPassword}
                autoComplete="off"
                onChange={e => {
                    setLoginPassword(e.target.value);
                    if (status === INVALID_PASSWORD) {
                        setStatus(null);
                    }
                }}
                helperText={
                    status === INVALID_PASSWORD ? (
                        <span>
                            Incorrect password. Try again, or click on <b>&quot;Forgot Password?&quot;</b> to reset it.
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
                className={cls.formControlLabel}
                control={<Checkbox checked={rememberMe} onClick={() => setRememberMe(!rememberMe)} color="primary" />}
                label={<Typography variant="body1">Remember me</Typography>}
            />
            {status === "verificationEmailSend" ? <HighlightedInformation>We have sent instructions on how to reset your password to your email address</HighlightedInformation> : null}
        </>
    );
};
