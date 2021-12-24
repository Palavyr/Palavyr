import React from "react";
import { makeStyles } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { TextInput } from "@widgetcore/BotResponse/number/TextInput";
import { useAppContext } from "widget/hook";

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
    const cls = useStyles();
    const { name, setName } = useAppContext();

    return (
        <TextInput
            disabled={disabled}
            inputPropsClassName={cls.input}
            inputLabelPropsClassName={cls.label}
            margin="normal"
            error={status === INVALID_NAME}
            required
            fullWidth
            label="Name"
            value={name}
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                if (!checkUserName(name, setStatus)) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setName(event.target.value);
                if (status === INVALID_NAME) {
                    setStatus("");
                }
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
