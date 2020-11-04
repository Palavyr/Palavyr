
import React from "react";
import Auth from "auth/Auth";
import { FormDialog } from "@common/components/borrowed/FormDialog";
import { LoginActions } from "@landing/login/LoginActions";
import { useCallback, useState } from "react";
import { useHistory } from "react-router-dom";
import { FormDialogContent, FormStatusTypes } from "@common/components/borrowed/FormDialogContent";
import { makeStyles } from "@material-ui/core";
import { GoogleAuthResponse } from "@Palavyr-Types";
import { LocalStorage } from "localStorage/localStorage";
// import { GoogleLoginResponse } from "react-google-login";


export type GoogleResponse = {
    tokenId: string;
    accessToken: string;
    googleId: string;
}


interface ILoginDialog {
    status: FormStatusTypes;
    setStatus: any;
    onClose: () => void;
    openChangePasswordDialog: () => void;
}

const useStyles = makeStyles(theme => ({
    background: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",

    }
}))

export const LoginDialog = ({ status, setStatus, onClose, openChangePasswordDialog }: ILoginDialog) => {

    const [isLoading, setIsLoading] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [loginEmail, setLoginEmail] = useState<string>("");
    const [loginPassword, setLoginPassword] = useState<string>("");

    const classes = useStyles();

    const history = useHistory();

    const success = () => {
        setTimeout(() => {
            setIsLoading(false);
            history.push("/dashboard");
        }, 150);
    }

    const error = (response) => {
        if (response.message === "Password does not match.") {
            setStatus("invalidPassword");
        } else if (response.message === "Could not find user.") {
            setStatus("invalidEmail");
        }
        setIsLoading(false);
    }

    const googleError = (response) => {
        window.gapi.load('auth2', () => {
            const auth2 = window.gapi.auth2.getAuthInstance()
            if (auth2 != null) {
                auth2.then(
                    () => {
                        auth2.signOut().then(async () => {
                            auth2.disconnect().then(error(response))
                        })
                    })
            }
        })
    }


    const login = useCallback(async () => {
        setIsLoading(true);
        setStatus(null);

        if (loginEmail && loginPassword) {
            const successfulResponse = await Auth.login(
                loginEmail,
                loginPassword,
                success,
                error
            );
            LocalStorage.setDefaultLoginType();
            if (!successfulResponse) {
                setTimeout(() => {
                    setStatus("invalidEmail");
                    setIsLoading(false);
                }, 1500);
            } else {
                setIsLoading(false);
            }
        }

    }, [setIsLoading, loginEmail, loginPassword, history, setStatus]);

    const onFormSubmit = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();
        await login();
    }


    const googleLogin = useCallback(
        async (response: GoogleResponse) => {
            setIsLoading(true);
            setStatus(null);
            var successfulResponse = await Auth.loginWithGoogle(
                response.tokenId,
                response.accessToken,
                response.googleId,
                success,
                googleError
            )
            if (!successfulResponse) {
                setTimeout(() => {
                    setStatus("invalidEmail");
                    setIsLoading(false);
                }, 1500);
            }
        },
        []
    )

    const responseGoogle = async (res: GoogleAuthResponse) => {
        console.log("wow")
        if (res) {
            const response: GoogleResponse = {
                tokenId: res.wc.id_token,
                accessToken: res.wc.access_token,
                googleId: res.tt.CT,
            }
            LocalStorage.setGoogleImage(res.tt.OJ);
            LocalStorage.setGoogleLoginType();
            console.log("logging in with google")
            await googleLogin(response);

        }
    }

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
                    status={status}
                    responseGoogle={responseGoogle}
                    isPasswordVisible={isPasswordVisible}
                    setIsPasswordVisible={setIsPasswordVisible}
                    loginEmail={loginEmail}
                    loginPassword={loginPassword}
                    setLoginEmail={setLoginEmail}
                    setLoginPassword={setLoginPassword}
                    setStatus={setStatus}
                />
            }
            actions={
                <LoginActions
                    isLoading={isLoading}
                    openChangePasswordDialog={openChangePasswordDialog}
                />
            }
        />
    );
}
