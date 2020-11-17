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
import { googleOAuthClientId } from "@api-client/clientUtils";
import { GoogleLoginResponseOffline } from "react-google-login";

export type GoogleResponse = {
    tokenId: string;
    accessToken: string;
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

    const classes = useStyles();

    const history = useHistory();

    const success = () => {
        setTimeout(() => {
            setIsLoading(false);
            history.push("/dashboard");
        }, 150);
    };

    const error = (response) => {
        if (response.message === "Password does not match.") {
            setStatus("invalidPassword");
        } else if (response.message === "Could not find user.") {
            setStatus("invalidEmail");
        }
        setIsLoading(false);
    };

    const googleError = (response) => {
        Auth.googleLogout(() => console.log(response));
    };

    const login = async () => {
        setIsLoading(true);
        setStatus(null);

        if (loginEmail && loginPassword) {
            const successfulResponse = await Auth.login(loginEmail, loginPassword, success, error);
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
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
        await login();
    };

    const googleLogin = async (response: GoogleResponse) => {
        Auth.ClearAuthentication();
        setIsLoading(true);
        setStatus(null);
        var successfulResponse = await Auth.loginWithGoogle(response.tokenId, response.accessToken, response.googleId, success, googleError);
        if (!successfulResponse) {
            setTimeout(() => {
                setStatus("invalidEmail");
                setIsLoading(false);
            }, 1500);
        }
    };
    const responseGoogle = async (res: GoogleAuthResponse) => {
        Auth.ClearAuthentication();
        console.log(`GoogleAuthResponse: ${res}`);
        if (res !== null) {
            let response: GoogleResponse;
            // if (res.wc === undefined) {
            //     response = {
            //         tokenId: res.xc.id_token,
            //         accessToken: res.xc.access_token,
            //         googleId: ""
            //     };
            // } else {
            response = {
                tokenId: res.wc.id_token,
                accessToken: res.wc.access_token,
                googleId: res.tt.CT,
            };
            // }

            LocalStorage.setGoogleImage(res.tt.OJ);
            LocalStorage.setGoogleLoginType();
            await googleLogin(response);
        }
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
            actions={<LoginActions isLoading={isLoading} openChangePasswordDialog={openChangePasswordDialog} />}
        />
    );
};
