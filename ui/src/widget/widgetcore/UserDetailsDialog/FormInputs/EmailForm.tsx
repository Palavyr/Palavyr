import { TextInput } from "@widgetcore/BotResponse/number/TextInput";
import React, { Dispatch, SetStateAction } from "react";
import { useAppContext } from "widget/hook";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, checkUserName, INVALID_EMAIL, INVALID_PHONE } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

export const EmailForm = ({ status, setStatus, setDetailsSet, disabled }: EmailFormProps) => {

    const { name, emailAddress, setEmailAddress, phoneNumber, setPhoneNumber } = useAppContext();
    const checkUserDetailsAreSet = () => {
        const userNameResult = checkUserName(name, setStatus);
        const userEmailResult = checkUserEmail(emailAddress, setStatus);

        if (status === INVALID_PHONE) {
            setPhoneNumber("");
        }

        if (!userNameResult || !userEmailResult) {
            return false;
        }
        return true;
    };

    return (
        <TextInput
            disabled={disabled}
            margin="normal"
            error={status === INVALID_EMAIL}
            required
            fullWidth
            value={emailAddress}
            label="Email Address"
            autoComplete="off"
            type="email"
            onBlur={() => {
                setDetailsSet(checkUserDetailsAreSet());
            }}
            onChange={e => {
                setEmailAddress(e.target.value);
                if (status === INVALID_EMAIL) {
                    setStatus("");
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            FormHelperTextProps={{ error: true }}
        />
    );
};
