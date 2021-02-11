import React from "react";
import { TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";

export interface NameFormProps extends BaseFormProps {}

export const NameForm = ({ userDetails, status, setStatus, setUserDetails }: NameFormProps) => {
    return (
        <TextField
            margin="normal"
            error={status == INVALID_NAME}
            required
            fullWidth
            label="Name"
            value={userDetails.userName}
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                const result = checkUserName(userDetails.userName, setStatus);
                if (!result) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setUserDetails({ ...userDetails, userName: event.target.value });
                if (status === INVALID_NAME) {
                    setStatus(null);
                }
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
