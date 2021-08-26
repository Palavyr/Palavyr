import React, { useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { TextInput } from "../../BotResponse/number/TextInput";

export interface NameFormProps extends BaseFormProps {
    disabled: boolean;
}

const useStyles = makeStyles(theme => ({
    input: {
        color: theme.palette.common.black,
        borderBottom: "1px solid gray",
    },
    label: {
        color: theme.palette.common.black,
    },
}));

export const NameForm = ({ status, setStatus, disabled }: NameFormProps) => {
    const [nameState, setNameState] = useState<string>("");
    const cls = useStyles();

    useEffect(() => {
        setNameState("James Dean");
    }, []);

    return (
        <TextInput
            disabled={disabled}
            inputPropsClassName={cls.input}
            inputLabelPropsClassName={cls.label}
            margin="normal"
            required
            fullWidth
            label="Name"
            value={nameState}
            autoFocus
            autoComplete="off"
            type="text"
            onChange={event => {
                setNameState(event.target.value);
            }}
            FormHelperTextProps={{ error: true }}
        />
    );
};
