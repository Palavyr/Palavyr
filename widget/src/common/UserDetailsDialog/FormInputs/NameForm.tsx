import React from "react";
import { TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";

export interface NameFormProps extends BaseFormProps {}

export const NameForm = ({ userDetails, setUserDetails, status, setStatus }: NameFormProps) => {
    return (
        <TextField
            margin="normal"
            error={status == INVALID_NAME}
            required
            fullWidth
            label="Name"
            value={userDetails.name}
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                const result = checkUserName(userDetails.name, setStatus);
                if (!result) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setUserDetails({ ...userDetails, name: event.target.value });
                if (status === INVALID_NAME) {
                    setStatus(null);
                }
                console.log(userDetails.name + " From NameForm")
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
