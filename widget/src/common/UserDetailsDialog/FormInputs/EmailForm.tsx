import { TextField } from "@material-ui/core";
import React, { Dispatch, SetStateAction } from "react";
import { UserDetails } from "src/types";
import { BaseFormProps } from "../CollectDetailsForm";
import { INVALID_EMAIL } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    checkUserDetailsAreSet(userDetails: UserDetails, setStatus: Dispatch<SetStateAction<string>>): boolean;
    setDetailsSet: Dispatch<SetStateAction<boolean>>
}``

export const EmailForm = ({userDetails, setUserDetails, status, setStatus, setDetailsSet, checkUserDetailsAreSet}: EmailFormProps) => {
    return (
        <TextField
            margin="normal"
            error={status === INVALID_EMAIL}
            required
            fullWidth
            label="Email Address"
            value={userDetails.userEmail}
            autoComplete="off"
            type="email"
            onBlur={() => {
                // const result = checkUserEmail(userDetails.userEmail, setStatus);
                // if (!result) setStatus(INVALID_EMAIL);
                setDetailsSet(checkUserDetailsAreSet(userDetails, setStatus));

            }}
            onChange={e => {
                setUserDetails({ ...userDetails, userEmail: e.target.value });
                if (status === INVALID_EMAIL) {
                    setStatus(null);
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            FormHelperTextProps={{ error: true }}
        />
    );
};
