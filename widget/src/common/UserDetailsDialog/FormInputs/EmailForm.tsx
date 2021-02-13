import { TextField } from "@material-ui/core";
import React, { Dispatch, SetStateAction } from "react";
import { getEmailAddressContext, getNameContext, setEmailAddressContext, setPhoneContext } from "src/widgetCore/store/dispatcher";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, checkUserName, INVALID_EMAIL, INVALID_PHONE } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
}

export const EmailForm = ({ status, setStatus, setDetailsSet }: EmailFormProps) => {
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

    return (
        <TextField
            margin="normal"
            error={status === INVALID_EMAIL}
            required
            fullWidth
            label="Email Address"
            autoComplete="off"
            type="email"
            onBlur={() => {
                setDetailsSet(checkUserDetailsAreSet());
            }}
            onChange={e => {
                setEmailAddressContext(e.target.value);
                if (status === INVALID_EMAIL) {
                    setStatus(null);
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            FormHelperTextProps={{ error: true }}
        />
    );
};
