import * as React from "react";
import { TextField, makeStyles, fade } from "@material-ui/core";
import { useState } from "react";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { UserDetails } from "src/types";
import NumberFormat from "react-number-format";

export interface IFormDialogContent {
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
}

const useStyles = makeStyles(theme => ({
    formControlLabel: {
        marginRight: 0,
    },
    centeredItems: {
        textAlign: "center",
        width: "100%",
        marginBottom: "1rem",
    },
    errorText: {
        color: "red",
        fontSize: "11pt",
    },
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

const INVALID_EMAIL = "invalid_email";
const INVALID_NAME = "invalid_name";

const checkIfEmpty = (val: any) => {
    if (val === "" || val === null || val === undefined){
        return false;
    }
    return true;
}

const checkUserName = (name: string) => {
    const userName = name.trim();
    return checkIfEmpty(userName);
}

const checkUserEmail = (email: string) => {
    const userEmail = email.trim();
    return checkIfEmpty(userEmail);
}

const checkUserPhone = (phone: string) => {
    const userPhone = phone.trim();
    return checkIfEmpty(userPhone);
}

export const UserDetailsDialogContent = ({ setDetailsSet, userDetails, setUserDetails }: IFormDialogContent) => {
    const [status, setStatus] = useState<string>("valid");
    // const [regionSwitch, setRegionSwitch] = useState<boolean>(region === "AU" || region === undefined ? true : false);

    const userDetailsAreSet = (userDetails: UserDetails) => {
        const userNameResult = checkUserName(userDetails.userName);
        const userEmailResult = checkUserEmail(userDetails.userEmail);
        const userPhoneResult = checkUserPhone(userDetails.userPhone);

        if (!userNameResult || !userEmailResult || !userPhoneResult){
            return false
        }
        return true;
    }

    const cls = useStyles();

    return (
        <>
            <TextField
                variant="outlined"
                margin="normal"
                error={status == INVALID_NAME}
                required
                fullWidth
                label="Name"
                value={userDetails.userName}
                autoFocus
                autoComplete="off"
                type="text"
                onChange={event => {
                    setUserDetails({ ...userDetails, userName: event.target.value });
                    setDetailsSet(userDetailsAreSet(userDetails));
                }}
                helperText={status === INVALID_NAME && "Name is not set"}
                FormHelperTextProps={{ error: true}}
            />
            <TextField
                variant="outlined"
                margin="normal"
                error={status === INVALID_EMAIL}
                required
                fullWidth
                label="Email Address"
                value={userDetails.userEmail}

                autoComplete="off"
                type="email"
                onChange={e => {
                    setUserDetails({ ...userDetails, userEmail: e.target.value });
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                    setDetailsSet(userDetailsAreSet(userDetails));


                }}
                helperText={status === INVALID_EMAIL && "Email is not formatted."}
                FormHelperTextProps={{ error: true }}
            />
            <NumberFormat

                placeholder="Phone number (optional)"
                className={cls.phone}
                // format={regionSwitch ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                mask="_"
                type="tel"
                onValueChange={values => {
                    setUserDetails({ ...userDetails, userPhone: values.formattedValue });
                    setDetailsSet(userDetailsAreSet(userDetails));
                }}
            />
        </>
    );
};
