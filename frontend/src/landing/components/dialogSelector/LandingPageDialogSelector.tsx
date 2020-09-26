import React, { useState, useCallback, Fragment } from "react";

import { RegisterDialog } from "@landing/register/RegisterDialog";
import { DialogTypes } from "./dialogTypes";
import { FormStatusTypes } from "@common/components/borrowed/FormDialogContent";
import { TermsOfServiceDialog } from "legal/TermsOfService";
import { LoginDialog } from "@landing/login/LoginDialog";
import { ChangePasswordDialog } from "@common/components/borrowed/ChangePasswordDialog";
import { ModalBackdrop } from "@common/components/borrowed/ModalBackdrop";

export interface ILandingPageDialogSelector {
    dialogOpen: DialogTypes;
    openTermsDialog: any;
    openRegisterDialog: any;
    openLoginDialog: any;
    openChangePasswordDialog: any;
    onClose: any;
}

export const LandingPageDialogSelector = ({ dialogOpen, openTermsDialog, openRegisterDialog, openLoginDialog, openChangePasswordDialog, onClose }: ILandingPageDialogSelector) => {

    const [loginStatus, setLoginStatus] = useState<FormStatusTypes>(null);
    const [registerStatus, setRegisterStatus] = useState<string | null>(null);

    const _onClose = useCallback(() => {
        setLoginStatus(null);
        setRegisterStatus(null);
        onClose();
    }, [onClose, setLoginStatus, setRegisterStatus]);

    const handleResetPassword = () => {
        console.log("TODO: Reset Password Email")
    }

    const printDialog = useCallback(() => {
        switch (dialogOpen) {
            case "register":
                return (
                    <RegisterDialog
                        onClose={_onClose}
                        openTermsDialog={openTermsDialog}
                        status={registerStatus}
                        setStatus={setRegisterStatus}
                    />
                );
            case "termsOfService":
                return <TermsOfServiceDialog onClose={openRegisterDialog} />;
            case "login":
                return (
                    <LoginDialog
                        onClose={_onClose}
                        status={loginStatus}
                        setStatus={setLoginStatus}
                        openChangePasswordDialog={openChangePasswordDialog}
                    />
                );
            case "changePassword":
                return (
                    <ChangePasswordDialog
                        setLoginStatus={setLoginStatus}
                        onClose={openLoginDialog}
                        handleResetPassword={handleResetPassword}
                    />
                );
            default:
                return null;
        }
    }, [
        dialogOpen,
        openChangePasswordDialog,
        openLoginDialog,
        openRegisterDialog,
        openTermsDialog,
        _onClose,
        loginStatus,
        registerStatus,
        setLoginStatus,
        setRegisterStatus,
    ]);

    return (
        <Fragment>
            {dialogOpen && <ModalBackdrop open />}
            {printDialog()}
        </Fragment>
    );
}