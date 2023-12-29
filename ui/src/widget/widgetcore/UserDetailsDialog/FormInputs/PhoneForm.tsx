import { TextInput } from "@common/components/TextField/TextInput";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { makeStyles } from "@material-ui/core";
import { TextInputProps } from "@widgetcore/BotResponse/number/TextInput";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import React, { useContext } from "react";
import { NumericFormat, PatternFormat } from "react-number-format";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserPhone, INVALID_PHONE } from "../UserDetailsCheck";

const MASKCHAR = "#";

export interface PhoneFormProps extends BaseFormProps {
    phonePattern: string;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    phone: (props: WidgetPreferencesResource) => ({
        width: "100%",
        marginTop: "1.3rem",
        color: props.chatFontColor,
        position: "relative",
        backgroundColor: "transparent",
        border: "none",
        fontSize: 16,
        padding: "6px 6px 0.3px 0px",
        transition: theme.transitions.create(["border-color", "box-shadow"]),
        fontFamily: ["-apple-system", "BlinkMacSystemFont", '"Segoe UI"', "Roboto", '"Helvetica Neue"', "Arial", "sans-serif", '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"'].join(","),
        "&:focus": {
            borderBottom: `1px solid ${props.chatFontColor}`,
            outline: "none",
        },
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
    placeholder: (props: WidgetPreferencesResource) => ({
        color: props.chatFontColor,

        "&::placeholder": {
            color: props.chatFontColor,
            fontStyle: "italic",
        },
    }),
}));

const CustomInput = (props: TextInputProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return <TextInput {...props} placeholder="Phone number (optional)" InputProps={{ classes: { input: cls.placeholder } }} />;
};

export const PhoneForm = ({ phonePattern, status, setStatus }: PhoneFormProps) => {
    const { context, preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return (
        <PatternFormat
            customInput={CustomInput}
            style={status === INVALID_PHONE ? { border: "3px solid red" } : {}}
            onError={() => setStatus(INVALID_PHONE)}
            error={status === INVALID_PHONE}
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
