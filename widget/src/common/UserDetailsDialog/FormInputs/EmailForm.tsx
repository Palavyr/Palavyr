import { TextField } from "@material-ui/core";
import { getNameContext, getEmailAddressContext, setPhoneContext, setEmailAddressContext } from "@store-dispatcher";
import { TextInput } from "common/number/TextInput";
import React, { Dispatch, SetStateAction } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, checkUserName, INVALID_EMAIL, INVALID_PHONE } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

export const EmailForm = ({ status, setStatus, setDetailsSet, disabled }: EmailFormProps) => {
    const checkUserDetailsAreSet = () => {
        const name = getNameContext();
        const emailAddress = getEmailAddressContext();

        const userNameResult = checkUserName(name, setStatus);
        const userEmailResult = checkUserEmail(emailAddress, setStatus);

        if (status === INVALID_PHONE) {
            setPhoneContext("");
        }

        if (!userNameResult || !userEmailResult) {
            return false;
        }
        return true;
    };

    const [emailState, setEmailState] = useState<string>("");
    useEffect(() => {
        setEmailState(getEmailAddressContext());
    }, []);

    return (
        <TextInput
            disabled={disabled}
            margin="normal"
            error={status === INVALID_EMAIL}
            required
            fullWidth
            value={emailState}
            label="Email Address"
            autoComplete="off"
            type="email"
            onBlur={() => {
                setDetailsSet(checkUserDetailsAreSet());
            }}
            onChange={e => {
                setEmailAddressContext(e.target.value);
                setEmailState(e.target.value);
                if (status === INVALID_EMAIL) {
                    setStatus("");
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            FormHelperTextProps={{ error: true }}
        />
    );
};
