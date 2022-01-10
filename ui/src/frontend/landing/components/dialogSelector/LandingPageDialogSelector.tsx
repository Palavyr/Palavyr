import React, { useState, useCallback } from "react";

import { RegisterDialog } from "@landing/register/RegisterDialog";
import { DialogTypes } from "./dialogTypes";
import { LoginDialog } from "@landing/login/LoginDialog";
import { ChangePasswordDialog } from "@common/components/borrowed/ChangePasswordDialog";
import { FormStatusTypes } from "@Palavyr-Types";
import { CHANGE_PASSWORD, LOGIN, PRIVACY_POLICY, REGISTER, TERMS_OF_SERVICE } from "@constants";
import { TermsOfServiceDialog } from "@common/legal/terms-of-use/TermsOfServiceDialog";
import { PrivacyPolicyDialog } from "@common/legal/privacy-policy/PrivacyPolicyDialog";

export interface ILandingPageDialogSelector {
    dialogOpen: DialogTypes;
    openPrivacyDialog(): void;
    openTermsDialog(): void;
    openRegisterDialog(): void;
    openLoginDialog(): void;
    openChangePasswordDialog(): void;
    onClose(): void;
}

export const LandingPageDialogSelector = ({ dialogOpen, openPrivacyDialog, openTermsDialog, openRegisterDialog, openLoginDialog, openChangePasswordDialog, onClose }: ILandingPageDialogSelector) => {
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
                return <RegisterDialog openPrivacyDialog={openPrivacyDialog} openTermsDialog={openTermsDialog} status={registerStatus} setStatus={setRegisterStatus} />;
            case TERMS_OF_SERVICE:
                return <TermsOfServiceDialog onClose={openRegisterDialog} />;
            case LOGIN:
                return <LoginDialog status={loginStatus} setStatus={setLoginStatus} openChangePasswordDialog={openChangePasswordDialog} />;
            case CHANGE_PASSWORD:
                return <ChangePasswordDialog setLoginStatus={setLoginStatus} onClose={openLoginDialog} />;
            case PRIVACY_POLICY:
                return <PrivacyPolicyDialog onClose={openRegisterDialog} />;
            default:
                return null;
        }
    }, [dialogOpen, openChangePasswordDialog, openLoginDialog, openRegisterDialog, openTermsDialog, _onClose, loginStatus, registerStatus, setLoginStatus, setRegisterStatus]);

    return <>{printDialog()}</>;
};
