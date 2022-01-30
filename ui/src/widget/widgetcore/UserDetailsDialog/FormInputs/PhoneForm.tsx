import { makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import React, { useContext } from "react";
import NumberFormat from "react-number-format";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserPhone, INVALID_PHONE } from "../UserDetailsCheck";

const MASKCHAR = "_";

export interface PhoneFormProps extends BaseFormProps {
    phonePattern: string;
}

const useStyles = makeStyles(theme => ({
    phone: {
        width: "100%",
        marginTop: "2.3rem",
        position: "relative",
        backgroundColor: "transparent",
        border: "none",
        borderBottom: "1px solid gray",
        fontSize: 16,
        padding: "6px 6px 0.3px 0px",
        transition: theme.transitions.create(["border-color", "box-shadow"]),
        fontFamily: ["-apple-system", "BlinkMacSystemFont", '"Segoe UI"', "Roboto", '"Helvetica Neue"', "Arial", "sans-serif", '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"'].join(","),
        "&:focus": {
            borderBottom: "1px solid gray",
            outline: "none",
        },
    },

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

export const PhoneForm = ({ phonePattern, status, setStatus }: PhoneFormProps) => {
    const { context, preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return (
        <NumberFormat
            style={status === INVALID_PHONE ? { border: "3px solid red" } : {}}
            placeholder="Phone number (optional)"
            onError={() => setStatus(INVALID_PHONE)}
            error={status === INVALID_PHONE ? "WOW" : ""}
            className={cls.phone}
            format={phonePattern}
            mask={MASKCHAR}
            type="tel"
            value={context.phoneNumber}
            onBlur={() => {
                if (!checkUserPhone(context.phoneNumber, setStatus, MASKCHAR)) {
                    setStatus(INVALID_PHONE);
                }
            }}
            onValueChange={values => {
                context.setPhoneNumber(values.formattedValue);
                if (status === INVALID_PHONE) {
                    setStatus("");
                }
            }}
        />
    );
};
