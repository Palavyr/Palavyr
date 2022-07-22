import { makeStyles, TextField } from "@material-ui/core";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import React, { useContext } from "react";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserEmail, checkUserName, INVALID_EMAIL, INVALID_PHONE } from "../UserDetailsCheck";

export interface EmailFormProps extends BaseFormProps {}

const useStyles = makeStyles(theme => ({
    helperTextRoot: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,
        border: "none",
        "&.Mui-error": {
            color: props.chatFontColor,
            border: "none",
        },
    }),
    formHelperTextProps: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,
    }),
    classesRoot: (props: WidgetPreferencesResource) => ({
        border: "none",
    }),
    inputProps: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,
    }),
    InputLabelProps: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,
    }),
    InputLabelPropsRoot: (props: WidgetPreferencesResource) => ({
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
    InputPropsClassName: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,
    }),
    textField: (props: WidgetPreferencesResource) => ({
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

export const EmailForm = ({ status, setStatus }: EmailFormProps) => {
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
