import React, { Dispatch, SetStateAction } from "react";
import { useState } from "react";
import { TextInput } from "../../BotResponse/number/TextInput";
import { BaseFormProps } from "../CollectDetailsForm";

export interface EmailFormProps extends BaseFormProps {
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

export const EmailForm = ({ status, setStatus, setDetailsSet, disabled }: EmailFormProps) => {
    const [emailState, setEmailState] = useState<string>("");
    return (
        <TextInput
            disabled={disabled}
            margin="normal"
            required
            fullWidth
            value={emailState}
            label="Email Address"
            autoComplete="off"
            type="email"
            onBlur={() => {}}
            onChange={e => {}}
            FormHelperTextProps={{ error: true }}
        />
    );
};
