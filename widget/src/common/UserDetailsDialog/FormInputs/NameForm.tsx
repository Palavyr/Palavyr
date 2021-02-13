import React, { useState } from "react";
import { TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { setNameContext } from "src/widgetCore/store/actions";
import { getNameContext } from "src/widgetCore/store/dispatcher";

export interface NameFormProps extends BaseFormProps {}

export const NameForm = ({ status, setStatus }: NameFormProps) => {
    const [name, setName] = useState("");
    return (
        <TextField
            margin="normal"
            error={status == INVALID_NAME}
            required
            fullWidth
            label="Name"
            value={name}
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                const result = checkUserName(name, setStatus);
                if (!result) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setName(event.target.value);
                setNameContext(event.target.value);
                if (status === INVALID_NAME) {
                    setStatus(null);
                }
                console.log("local State: " + name);
                console.log("redux State: " + getNameContext());
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
