import React from "react";
import Auth from "@auth/Auth";
import { FormCard } from "@common/components/borrowed/FormDialog";
import { LoginActions } from "@landing/login/LoginActions";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import { makeStyles } from "@material-ui/core";
import { Credentials, FormStatusTypes } from "@Palavyr-Types";
import { SessionStorage } from "@localStorage/sessionStorage";
import {
    COULD_NOT_FIND_ACCOUNT,
    COULD_NOT_FIND_SERVER,
    DASHBOARD_HOME,
    DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE,
    INVALID_EMAIL,
    INVALID_PASSWORD,
    NOT_A_DEFAULT_ACCOUNT,
    PASSWORD_DOES_NOT_MATCH,
    REMEMBER_ME_EMAIL_COOKIE_NAME,
    REMEMBER_ME_PASSWORD_COOKIE_NAME,
} from "@constants";
import { FormDialogContent } from "@common/components/borrowed/FormDialogContent";
import { useEffect } from "react";
import Cookies from "js-cookie";

interface ILoginDialog {
    status: FormStatusTypes;
    setStatus: any;
    openChangePasswordDialog: () => void;
}

const useStyles = makeStyles(theme => ({
    background: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
}));

export const LoginDialog = ({ status, setStatus, openChangePasswordDialog }: ILoginDialog) => {
    const [isLoading, setIsLoading] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [loginEmail, setLoginEmail] = useState<string>("");
    const [loginPassword, setLoginPassword] = useState<string>("");
    const [rememberMe, setRememberMe] = useState<boolean>(false);

    const history = useHistory();

    useEffect(() => {
        const email = Cookies.get(REMEMBER_ME_EMAIL_COOKIE_NAME);
        const password = Cookies.get(REMEMBER_ME_PASSWORD_COOKIE_NAME);

        if (email && password) {
            setLoginEmail(email);
            setLoginPassword(password);
        }
    }, []);

    const successRedirectToDashboard = () => {
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

    const login = async () => {
        setIsLoading(true);
        setStatus(null);

        if (loginEmail && loginPassword) {
            const successfulResponse = await Auth.login(loginEmail, loginPassword, successRedirectToDashboard, error);
            SessionStorage.setDefaultLoginType();
            setIsLoading(false);
            if (successfulResponse === null) {
                setStatus(COULD_NOT_FIND_SERVER);
            }
        }
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();

        if (rememberMe && loginEmail && loginPassword) {
            Cookies.set(REMEMBER_ME_EMAIL_COOKIE_NAME, loginEmail);
            Cookies.set(REMEMBER_ME_PASSWORD_COOKIE_NAME, loginPassword);
        } else {
            Cookies.remove(REMEMBER_ME_EMAIL_COOKIE_NAME);
            Cookies.remove(REMEMBER_ME_PASSWORD_COOKIE_NAME);
        }

        await login();
    };

    return (
        <FormCard
            onFormSubmit={onFormSubmit}
            content={
                <FormDialogContent
                    rememberMe={rememberMe}
                    setRememberMe={setRememberMe}
                    status={status}
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
