import { TextField } from "@material-ui/core";
import React, { Dispatch, SetStateAction } from "react";
import { setEmailAddressContext } from "src/widgetCore/store/actions";
import { getContextProperties, getEmailAddressContext } from "src/widgetCore/store/dispatcher";
import { ContextProperties } from "src/widgetCore/store/types";
import { BaseFormProps } from "../CollectDetailsForm";
import { INVALID_EMAIL } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    checkUserDetailsAreSet(contextProperties: ContextProperties, setStatus: Dispatch<SetStateAction<string>>): boolean;
    setDetailsSet: Dispatch<SetStateAction<boolean>>
}

export const EmailForm = ({ status, setStatus, setDetailsSet, checkUserDetailsAreSet}: EmailFormProps) => {
    // const [emailState, setEmailState] = useState<string>("");
    return (
        <TextField
            margin="normal"
            error={status === INVALID_EMAIL}
            required
            fullWidth
            label="Email Address"
            value={getEmailAddressContext()}
            autoComplete="off"
            type="email"
            onBlur={() => {
                setDetailsSet(checkUserDetailsAreSet(getContextProperties(), setStatus));

            }}
            onChange={e => {
                setEmailAddressContext(e.target.value);
                // setContextProperties({ ...contextProperties, emailAddress: e.target.value });
                if (status === INVALID_EMAIL) {
                    setStatus(null);
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            FormHelperTextProps={{ error: true }}
        />
    );
};
