import { TextField } from "@material-ui/core";
import React from "react";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, INVALID_EMAIL } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {}

export const EmailForm = ({userDetails, status, setStatus, setUserDetails}: EmailFormProps) => {
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
                const result = checkUserEmail(userDetails.userEmail, setStatus);
                if (!result) setStatus(INVALID_EMAIL);
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
