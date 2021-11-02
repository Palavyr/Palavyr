import React, { useState, useCallback, Fragment } from "react";

import { RegisterDialog } from "@landing/register/RegisterDialog";
import { DialogTypes } from "./dialogTypes";
import { LoginDialog } from "@landing/login/LoginDialog";
import { ChangePasswordDialog } from "@common/components/borrowed/ChangePasswordDialog";
import { ModalBackdrop } from "@common/components/borrowed/ModalBackdrop";
import { FormStatusTypes } from "@Palavyr-Types";
import { CHANGE_PASSWORD, LOGIN, REGISTER, TERMS_OF_SERVICE } from "@constants";
import { TermsOfServiceDialog } from "@legal/terms-of-use/TermsOfServiceDialog";

export interface ILandingPageDialogSelector {
    dialogOpen: DialogTypes;
    openTermsDialog(): void;
    openRegisterDialog(): void;
    openLoginDialog(): void;
    openChangePasswordDialog(): void;
    onClose(): void;
}

export const LandingPageDialogSelector = ({ dialogOpen, openTermsDialog, openRegisterDialog, openLoginDialog, openChangePasswordDialog, onClose }: ILandingPageDialogSelector) => {

    const [loginStatus, setLoginStatus] = useState<FormStatusTypes>(null);
    const [registerStatus, setRegisterStatus] = useState<string | null>(null);

    const _onClose = useCallback(() => {
        setLoginStatus(null);
        setRegisterStatus(null);
        onClose();
    }, [onClose, setLoginStatus, setRegisterStatus]);

    const printDialog = useCallback(() => {
        switch (dialogOpen) {
            case REGISTER:
                return (
                    <RegisterDialog
                        onClose={_onClose}
                        openTermsDialog={openTermsDialog}
                        status={registerStatus}
                        setStatus={setRegisterStatus}
                    />
                );
            case TERMS_OF_SERVICE:
                return <TermsOfServiceDialog onClose={openRegisterDialog} />;
            case LOGIN:
                return (
                    <LoginDialog
                        onClose={_onClose}
                        status={loginStatus}
                        setStatus={setLoginStatus}
                        openChangePasswordDialog={openChangePasswordDialog}
                    />
                );
            case CHANGE_PASSWORD:
                return (
                    <ChangePasswordDialog
                        setLoginStatus={setLoginStatus}
                        onClose={openLoginDialog}
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
        <>
            {dialogOpen && <ModalBackdrop open />}
            {printDialog()}
        </>
    );
}