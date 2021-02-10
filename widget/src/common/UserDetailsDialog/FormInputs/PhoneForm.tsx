import { fade, makeStyles } from "@material-ui/core";
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
        marginTop: "1rem",
        borderRadius: 4,
        position: "relative",
        backgroundColor: theme.palette.common.white,
        border: "1px solid #ced4da",
        fontSize: 16,
        padding: "10px 12px",
        transition: theme.transitions.create(["border-color", "box-shadow"]),
        fontFamily: ["-apple-system", "BlinkMacSystemFont", '"Segoe UI"', "Roboto", '"Helvetica Neue"', "Arial", "sans-serif", '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"'].join(","),
        "&:focus": {
            boxShadow: `${fade(theme.palette.primary.main, 0.25)} 0 0 0 0.2rem`,
            borderColor: theme.palette.primary.main,
        },
    },
}));

export const PhoneForm = ({ phonePattern, userDetails, status, setStatus, setUserDetails }: PhoneFormProps) => {
    const cls = useStyles();

    return (
        <NumberFormat
            helpertext={status === INVALID_PHONE ? "funky number!" : ""}
            placeholder="Phone number (optional)"
            className={cls.phone}
            format={phonePattern}
            mask="_"
            type="tel"
            onError={() => setStatus(INVALID_PHONE)}
            onBlur={() => {
                const result = checkUserPhone(userDetails.userPhone, setStatus);
                if (!result) setStatus(INVALID_PHONE);
            }}
            onValueChange={values => {
                setUserDetails({ ...userDetails, userPhone: values.formattedValue });
                if (status === INVALID_PHONE) {
                    setStatus(null);
                }
            }}
            value={userDetails.userPhone}
        />
    );
};
