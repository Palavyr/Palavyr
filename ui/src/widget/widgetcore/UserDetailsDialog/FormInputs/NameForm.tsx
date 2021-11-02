import React, { useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { getNameContext, setNameContext } from "@store-dispatcher";
import { TextInput } from "@widgetcore/BotResponse/number/TextInput";

export interface NameFormProps extends BaseFormProps {
    disabled: boolean;
}

const useStyles = makeStyles(theme => ({
    input: {
        color: theme.palette.common.black,
        borderBottom: "1px solid gray"
    },
    label: {
        color: theme.palette.common.black,
    },
}));

export const NameForm = ({ status, setStatus, disabled }: NameFormProps) => {
    const [nameState, setNameState] = useState<string>("");
    const cls = useStyles();

    useEffect(() => {
        setNameState(getNameContext());
    }, []);

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
            value={nameState}
            autoFocus
            autoComplete="off"
            type="text"
            onBlur={() => {
                const reduxName = getNameContext();
                if (!checkUserName(reduxName, setStatus)) setStatus(INVALID_NAME);
            }}
            onChange={event => {
                setNameContext(event.target.value);
                setNameState(event.target.value);
                if (status === INVALID_NAME) {
                    setStatus("");
                }
            }}
            helperText={status === INVALID_NAME && "Name is not set"}
            FormHelperTextProps={{ error: true }}
        />
    );
};
