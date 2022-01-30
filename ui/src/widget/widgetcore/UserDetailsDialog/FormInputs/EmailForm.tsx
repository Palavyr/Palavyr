import { makeStyles, TextField } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import React, { Dispatch, SetStateAction, useContext } from "react";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, checkUserName, INVALID_EMAIL, INVALID_PHONE } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

const useStyles = makeStyles(theme => ({
    input: (props: WidgetPreferences) => ({
        border: "none",
        // borderBottom: `1px solid ${props.chatFontColor}`,
        color: props.chatFontColor,
        "&:hover": {
            border: "none",
        },
    }),
    label: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
        border: "none",
        "&:hover": {
            border: "none",
        },
    }),
    helperText: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
        border: "none",
        "&:hover": {
            border: "none",
        },
    }),
    error: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
        border: "none",
        "&:hover": {
            border: "none",
        },
    }),
}));

export const EmailForm = ({ status, setStatus, setDetailsSet, disabled }: EmailFormProps) => {
    const {
        preferences,
        context: { name, emailAddress, setEmailAddress, phoneNumber, setPhoneNumber },
    } = useContext(WidgetContext);
    const checkUserDetailsAreSet = () => {
        const userNameResult = checkUserName(name, setStatus);
        const userEmailResult = checkUserEmail(emailAddress, setStatus);

        if (status === INVALID_PHONE) {
            setPhoneNumber("");
        }

        if (!userNameResult || !userEmailResult) {
            return false;
        }
        return true;
    };

    const cls = useStyles(preferences);

    return (
        <TextField
            disabled={false}
            margin="normal"
            error={status === INVALID_EMAIL}
            inputProps={{ className: cls.input }}
            InputLabelProps={{ className: cls.label }}
            InputProps={{ className: cls.input }}
            FormHelperTextProps={{ error: true, className: cls.helperText }}
            classes={{ root: cls.input }}
            required
            fullWidth
            value={emailAddress}
            label="Email Address"
            autoComplete="off"
            type="email"
            onBlur={() => {
                setDetailsSet(checkUserDetailsAreSet());
            }}
            onChange={e => {
                setEmailAddress(e.target.value);
                if (status === INVALID_EMAIL) {
                    setStatus("");
                }
            }}
            helperText={status === INVALID_EMAIL && "Email is not formatted."}
            variant="standard"
        />
    );
};
