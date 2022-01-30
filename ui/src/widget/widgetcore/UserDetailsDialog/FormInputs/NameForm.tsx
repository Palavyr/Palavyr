import React, { useContext } from "react";
import { makeStyles, TextField } from "@material-ui/core";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserName, INVALID_NAME } from "../UserDetailsCheck";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetPreferences } from "@Palavyr-Types";
import classNames from "classnames";

export interface NameFormProps extends BaseFormProps {
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
    asterisk: (props: WidgetPreferences) => ({
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

export const NameForm = ({ status, setStatus, disabled }: NameFormProps) => {
    const {
        preferences,
        context: { name, setName },
    } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    return (
        <TextField
            disabled={false}
            margin="normal"
            error={status === INVALID_NAME}
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
            variant="standard"
        />
    );
};
