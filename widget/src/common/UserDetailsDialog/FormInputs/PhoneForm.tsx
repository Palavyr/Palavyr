import { makeStyles } from "@material-ui/core";
import React from "react";
import NumberFormat from "react-number-format";
import { BaseFormProps } from "../CollectDetailsForm";
import { checkUserPhone, INVALID_PHONE } from "../UserDetailsCheck";

export interface PhoneFormProps extends BaseFormProps {
    phonePattern: string;
}

const useStyles = makeStyles(theme => ({
    phone: {
        width: "100%",
        marginTop: "2.3rem",
        borderRadius: 4,
        position: "relative",
        backgroundColor: "transparent",
        border: "none",
        borderBottom: "1px solid gray",
        fontSize: 16,
        padding: "0px 6px 0px 0px",
        transition: theme.transitions.create(["border-color", "box-shadow"]),
        fontFamily: ["-apple-system", "BlinkMacSystemFont", '"Segoe UI"', "Roboto", '"Helvetica Neue"', "Arial", "sans-serif", '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"'].join(","),
        "&:focus": {
            borderBottom: "1px solid gray",
            outline: "none"

        },
    },
}));

export const PhoneForm = ({ phonePattern, contextProperties, setContextProperties, status, setStatus }: PhoneFormProps) => {
    const cls = useStyles();

    return (
        <NumberFormat
            // helpertext={status === INVALID_PHONE ? "funky number!" : ""}
            placeholder="Phone number (optional)"
            onError={() => setStatus(INVALID_PHONE)}
            error={status === INVALID_PHONE ? "WOW" : ""}
            className={cls.phone}
            format={phonePattern}
            mask="_"
            type="tel"
            onBlur={() => {
                const result = checkUserPhone(contextProperties.phoneNumber, setStatus);
                if (!result) setStatus(INVALID_PHONE);
                if (!result) setContextProperties({...contextProperties, phoneNumber: ""})
            }}
            onValueChange={values => {
                setContextProperties({ ...contextProperties, phoneNumber: values.formattedValue });
                if (status === INVALID_PHONE) {
                    setStatus(null);
                }
            }}
            value={contextProperties.phoneNumber}
        />
    );
};
