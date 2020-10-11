
import Auth from "auth/Auth";
import { FormDialog } from "@common/components/borrowed/FormDialog";
import { LoginActions } from "@landing/login/LoginActions";
import { useCallback, useState } from "react";
import { useHistory } from "react-router-dom";
import React from "react";
import { FormDialogContent, FormStatusTypes } from "@common/components/borrowed/FormDialogContent";
import { makeStyles } from "@material-ui/core";


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


    const login = useCallback(async () => {
        setIsLoading(true);
        setStatus(null);

        const successfulResponse = await Auth.login(
            loginEmail,
            loginPassword,
            success,
            error
        );

        if (!successfulResponse) {
            setTimeout(() => {
                setStatus("invalidEmail");
                setIsLoading(false);
            }, 1500);
        }

    }, [setIsLoading, loginEmail, loginPassword, history, setStatus]);

    const onFormSubmit = async (e: { preventDefault: () => void; }) => {
        e.preventDefault();
        await login();
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
