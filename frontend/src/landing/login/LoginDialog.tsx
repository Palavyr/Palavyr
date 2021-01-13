import React from "react";
import Auth from "auth/Auth";
import { FormDialog } from "@common/components/borrowed/FormDialog";
import { LoginActions } from "@landing/login/LoginActions";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import { makeStyles } from "@material-ui/core";
import { Credentials, FormStatusTypes, GoogleAuthResponse } from "@Palavyr-Types";
import { LocalStorage } from "localStorage/localStorage";
import { SessionStorage } from "localStorage/sessionStorage";
import {
    COULD_NOT_FIND_ACCOUNT,
    COULD_NOT_FIND_ACCOUNT_WITH_GOOGLE,
    COULD_NOT_FIND_SERVER,
    COULD_NOT_VALIDATE_GOOGLE_TOKEN,
    DASHBOARD_HOME,
    DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE,
    GOOGLE_ACCOUNT_NOT_FOUND,
    GOOGLE_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE,
    INVALID_EMAIL,
    INVALID_GOOGLE_TOKEN,
    INVALID_PASSWORD,
    NOT_A_DEFAULT_ACCOUNT,
    NOT_A_GOOGLE_ACCOUNT,
    PASSWORD_DOES_NOT_MATCH,
} from "@constants";
import { noop } from "lodash";
import { FormDialogContent } from "@common/components/borrowed/FormDialogContent";
import { useEffect } from "react";

export type GoogleResponse = {
    tokenId: string;
    googleId: string;
};

interface ILoginDialog {
    status: FormStatusTypes;
    setStatus: any;
    onClose: () => void;
    openChangePasswordDialog: () => void;
}

const useStyles = makeStyles((theme) => ({
    background: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
}));

export const LoginDialog = ({ status, setStatus, onClose, openChangePasswordDialog }: ILoginDialog) => {
    const [isLoading, setIsLoading] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [loginEmail, setLoginEmail] = useState<string>("");
    const [loginPassword, setLoginPassword] = useState<string>("");
    const [rememberMe, setRememberMe] = useState<boolean>(false);

    const classes = useStyles();

    const history = useHistory();

    useEffect(() => {
        const remembered = SessionStorage.getRememberMe();
        if (remembered !== undefined){
            const {emailAddress: emailAddress, password: password} = remembered;
            setLoginEmail(emailAddress);
            setLoginPassword(password);
        }
    }, [])

    const success = () => {
        setTimeout(() => {
            setIsLoading(false);
            history.push(DASHBOARD_HOME);
        }, 150);
    };

    const error = (response: Credentials) => {
        if (response.message === PASSWORD_DOES_NOT_MATCH) {
            setStatus(INVALID_PASSWORD);
        } else if (response.message === COULD_NOT_FIND_ACCOUNT) {
            setStatus(INVALID_EMAIL);
        } else if (response.message === DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE) {
            setStatus(NOT_A_DEFAULT_ACCOUNT);
        }
        setIsLoading(false);
    };

    const googleError = (response: Credentials) => {
        console.log(response.message);
        if (response.message === COULD_NOT_VALIDATE_GOOGLE_TOKEN) {
            setStatus(INVALID_GOOGLE_TOKEN);
        } else if (response.message === GOOGLE_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE) {
            setStatus(NOT_A_GOOGLE_ACCOUNT);
        } else if (response.message === COULD_NOT_FIND_ACCOUNT_WITH_GOOGLE) {
            setStatus(GOOGLE_ACCOUNT_NOT_FOUND);
        }
        setIsLoading(false);
        Auth.googleLogout(noop);
    };

    const login = async () => {
        setIsLoading(true);
        setStatus(null);

        if (loginEmail && loginPassword) {
            const successfulResponse = await Auth.login(loginEmail, loginPassword, success, error);
            LocalStorage.setDefaultLoginType();
            setIsLoading(false);
            if (successfulResponse === null) {
                setStatus(COULD_NOT_FIND_SERVER);
            }
        }
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();

        if (rememberMe && loginEmail && loginPassword) {
            SessionStorage.setRememberMe(loginEmail, loginPassword);
        } else {
            SessionStorage.unsetRememberMe();
        }

        await login();
    };

    const googleLogin = async (response: GoogleResponse) => {
        setIsLoading(true);
        setStatus(null);
        var successfulResponse = await Auth.loginWithGoogle(response.tokenId, response.googleId, success, googleError);
        if (successfulResponse === null) {
            Auth.ClearAuthentication();
            Auth.googleLogout(noop);
            setStatus(COULD_NOT_FIND_SERVER);
        }
        setIsLoading(false);
    };

    const responseGoogleSuccess = async (authResponse: GoogleAuthResponse) => {
        Auth.ClearAuthentication();

        const { id_token } = authResponse.getAuthResponse();
        const imageURL = authResponse.getBasicProfile().getImageUrl();
        const googleID = authResponse.getId();

        if (authResponse !== null) {
            const response = {
                tokenId: id_token,
                googleId: googleID,
            };
            LocalStorage.setGoogleImage(imageURL);
            LocalStorage.setGoogleLoginType();
            await googleLogin(response);
        }
    };

    const responseGoogleFailure = async (authResponse: GoogleAuthResponse) => {
        Auth.ClearAuthentication();
        setIsLoading(false);
    };

    return (
        <FormDialog
            open
            onClose={onClose}
            loading={isLoading}
            onFormSubmit={onFormSubmit}
            hideBackdrop
            headline="Login"
            content={
                <FormDialogContent
                    rememberMe={rememberMe}
                    setRememberMe={setRememberMe}
                    status={status}
                    responseGoogleSuccess={responseGoogleSuccess}
                    responseGoogleFailure={responseGoogleFailure}
                    isPasswordVisible={isPasswordVisible}
                    setIsPasswordVisible={setIsPasswordVisible}
                    loginEmail={loginEmail}
                    loginPassword={loginPassword}
                    setLoginEmail={setLoginEmail}
                    setLoginPassword={setLoginPassword}
                    setStatus={setStatus}
                />
            }
            actions={<LoginActions isLoading={isLoading} openChangePasswordDialog={openChangePasswordDialog} />}
        />
    );
};
