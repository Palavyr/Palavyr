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
    helperTextRoot: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
        border: "none",
        "&.Mui-error": {
            color: props.chatFontColor,
            border: "none",
        },
    }),
    formHelperTextProps: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
    }),
    classesRoot: (props: WidgetPreferences) => ({
        border: "none",
    }),
    inputProps: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
    }),
    InputLabelProps: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
    }),
    InputLabelPropsRoot: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
        borderBottomColor: props.chatFontColor,
        "&.Mui-focused": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },
        "&.Mui-error": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },
    }),
    InputPropsClassName: (props: WidgetPreferences) => ({
        color: props.chatFontColor,
    }),
    textField: (props: WidgetPreferences) => ({
        "&.Mui-error": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },
        "&.MuiFormHelperText": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },
        "&.MuiFormHelperText-root": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },
        "&.MuiInputBase-input:invalid": {
            color: props.chatFontColor,
        },
        "&.focus": {
            color: props.chatFontColor,
            borderBottomColor: props.chatFontColor,
        },

        "& .MuiInputBase-input": {
            color: props.chatFontColor,
        },
        "& .MuiInput-underline:before": {
            borderBottomColor: props.chatFontColor, // Semi-transparent underline
        },
        "& .MuiInput-underline:hover:before": {
            borderBottomColor: props.chatFontColor, // Solid underline on hover
        },
        "& .MuiInput-underline:after": {
            borderBottomColor: props.chatFontColor, // Solid underline on focus
        },
    }),
}));

export const EmailForm = ({ status, setStatus, setDetailsSet, disabled }: EmailFormProps) => {
    const {
        preferences,
        context: { name, emailAddress, setEmailAddress, setPhoneNumber },
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
            className={cls.textField}
            inputProps={{ className: cls.inputProps }}
            InputLabelProps={{
                className: cls.InputLabelProps,
                classes: { root: cls.InputLabelPropsRoot },
            }}
            InputProps={{ className: cls.InputPropsClassName }}
            FormHelperTextProps={{ error: true, className: cls.formHelperTextProps, classes: { root: cls.helperTextRoot } }}
            classes={{ root: cls.classesRoot }}
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
