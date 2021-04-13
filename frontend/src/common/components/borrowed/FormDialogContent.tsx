import * as React from "react";
import { TextField, FormControlLabel, Checkbox, Typography, makeStyles } from "@material-ui/core";
import { VisibilityPasswordTextField } from "./VisibilityPasswordTextField";
import { HighlightedInformation } from "./HighlightedInformation";
import { DividerWithText } from "../DividerWithText";
import { GoogleLogin } from "auth/googlebutton/GoogleLogin";
import { FormStatusTypes, GoogleAuthResponse } from "@Palavyr-Types";
import { COULD_NOT_FIND_SERVER, GOOGLE_ACCOUNT_NOT_FOUND, INVALID_EMAIL, INVALID_GOOGLE_TOKEN, INVALID_PASSWORD, NOT_A_DEFAULT_ACCOUNT, NOT_A_GOOGLE_ACCOUNT } from "@constants";
import { useEffect } from "react";
import { SessionStorage } from "localStorage/sessionStorage";

export interface IFormDialogContent {
    loginEmail: string;
    loginPassword: string;
    setLoginEmail: any;
    setLoginPassword: any;
    setStatus: any;
    isPasswordVisible: boolean;
    setIsPasswordVisible: any;
    status: FormStatusTypes;
    responseGoogleSuccess(res: GoogleAuthResponse): void;
    responseGoogleFailure(res: GoogleAuthResponse): void;
    rememberMe: boolean;
    setRememberMe(rememberMe: boolean): void;
}

const useStyles = makeStyles({
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

export const FormDialogContent = ({ rememberMe, setRememberMe, loginEmail, setLoginEmail, loginPassword, setLoginPassword, isPasswordVisible, setIsPasswordVisible, responseGoogleSuccess, responseGoogleFailure, setStatus, status }: IFormDialogContent) => {
    const cls = useStyles();

    useEffect(() => {
        const isRemembered = SessionStorage.checkIsRemembered();
        setRememberMe(isRemembered);
    }, [])

    return (
        <>
            <div className={cls.centeredItems}>
                {status === COULD_NOT_FIND_SERVER && <span className={cls.errorText}>Could not find server.</span>}
                <GoogleLogin onSuccess={responseGoogleSuccess} onFailure={responseGoogleFailure} />
                <br></br>
                {status === INVALID_GOOGLE_TOKEN && <span className={cls.errorText}>Session Expired. Please login again.</span>}
                {status === NOT_A_GOOGLE_ACCOUNT && <span className={cls.errorText}>Email found, but should be used with standard login form (below).</span>}
                {status === GOOGLE_ACCOUNT_NOT_FOUND && <span className={cls.errorText}>No account with this email address was found.</span>}
            </div>
            <br></br>
            <DividerWithText text="OR" />
            <br></br>
            <div className={cls.centeredItems}>{status === NOT_A_DEFAULT_ACCOUNT && <span className={cls.errorText}>Email found, but is associated with google login (above).</span>}</div>
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
                onChange={(e) => {
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
                onChange={(e) => {
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
            <FormControlLabel className={cls.formControlLabel} control={<Checkbox checked={rememberMe} onClick={() => setRememberMe(!rememberMe)} color="primary" />} label={<Typography variant="body1">Remember me</Typography>} />
            {status === "verificationEmailSend" ? <HighlightedInformation>We have sent instructions on how to reset your password to your email address</HighlightedInformation> : null}
        </>
    );
};
