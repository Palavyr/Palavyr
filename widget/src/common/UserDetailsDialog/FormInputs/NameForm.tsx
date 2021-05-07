import React, { useEffect, useState } from "react";
import { makeStyles, TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { getNameContext, setNameContext } from "@store-dispatcher";

export interface NameFormProps extends BaseFormProps {}

const useStyles = makeStyles(theme => ({
    input: {
        color: theme.palette.common.white,
    },
    label: {
        color: theme.palette.common.white,
    },
}));

export const NameForm = ({ status, setStatus }: NameFormProps) => {
    const [nameState, setNameState] = useState<string>("");
    const cls = useStyles();

    useEffect(() => {
        setNameState(getNameContext());
    }, []);

    return (
        <TextField
            InputProps={{ className: cls.input }}
            InputLabelProps={{ className: cls.label }}
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
