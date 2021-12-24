import { makeStyles } from "@material-ui/core";
import React from "react";
import NumberFormat from "react-number-format";
import { useAppContext } from "widget/hook";
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
        // borderRadius: 4,
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
}));

export const PhoneForm = ({ phonePattern, status, setStatus }: PhoneFormProps) => {
    const cls = useStyles();
    const { phoneNumber, setPhoneNumber } = useAppContext();

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
            value={phoneNumber}
            onBlur={() => {
                if (!checkUserPhone(phoneNumber, setStatus, MASKCHAR)) {
                    setStatus(INVALID_PHONE);
                }
            }}
            onValueChange={values => {
                setPhoneNumber(values.formattedValue);
                if (status === INVALID_PHONE) {
                    setStatus("");
                }
            }}
        />
    );
};
