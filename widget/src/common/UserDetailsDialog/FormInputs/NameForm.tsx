import React from "react";
import { TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { getNameContext, setNameContext } from "src/widgetCore/store/dispatcher";

export interface NameFormProps extends BaseFormProps {}

export const NameForm = ({ status, setStatus }: NameFormProps) => {
    return (
        <TextField
            margin="normal"
            error={status == INVALID_NAME}
            required
            fullWidth
            label="Name"
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                const reduxName = getNameContext();
                const result = checkUserName(reduxName, setStatus);
                if (!result) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setNameContext(event.target.value);
                if (status === INVALID_NAME) {
                    setStatus(null);
                }
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
