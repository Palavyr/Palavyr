import * as React from "react";
import { TextField, makeStyles, fade } from "@material-ui/core";
import { useEffect, useState } from "react";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { LocaleMap, UserDetails } from "src/types";
import NumberFormat from "react-number-format";
import { checkUserName, checkUserEmail, checkUserPhone, INVALID_NAME, INVALID_EMAIL, INVALID_PHONE } from "./UserDetailsCheck";
import { useLocation } from "react-router-dom";
import CreateClient from "src/client/Client";

export interface IFormDialogContent {
    status: string;
    setStatus: any;
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    checkUserDetailsAreSet(useDetails: UserDetails): boolean;
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

export const UserDetailsDialogContent = ({ status, setStatus, setDetailsSet, userDetails, setUserDetails, checkUserDetailsAreSet }: IFormDialogContent) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = CreateClient(secretKey);
    // const [locales, setLocales] = useState<LocaleMap>();

    const [, setLocaleID] = useState<string | undefined>();
    const [localeName, setLocaleName] = useState<string | undefined>();
    const [localeMap, setLocaleMap] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    // const [regionSwitch, setRegionSwitch] = useState<boolean>(region === "AU" || region === undefined ? true : false);

    useEffect(() => {
        (async () => {
            const { data: locale } = await client.Widget.Access.getLocale();

            setLocaleID(locale.localeId);
            setLocaleName(locale.localeCountry);
            setphonePattern(locale.localePhonePattern);
            setLocaleMap(locale.localeMap);
        })();
    }, []);

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
                onError={() => setStatus(INVALID_NAME)}
                onBlur={() => {
                    const result = checkUserName(userDetails.userName, setStatus);
                    if (result === false) setUserDetails({ ...userDetails, userName: "" });
                    setDetailsSet(checkUserDetailsAreSet(userDetails));
                    if (status === INVALID_NAME) {
                        setStatus(null);
                    }
                }}
                onChange={event => {
                    setUserDetails({ ...userDetails, userName: event.target.value });
                }}
                helperText={status === INVALID_NAME && "Name is not set"}
                FormHelperTextProps={{ error: true }}
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
                // onError={() => setStatus(INVALID_EMAIL)}
                onBlur={() => {
                    const result = checkUserEmail(userDetails.userEmail, setStatus);
                    if (result === false) setUserDetails({ ...userDetails, userEmail: "" });
                    setDetailsSet(checkUserDetailsAreSet(userDetails));
                }}
                onChange={e => {
                    setUserDetails({ ...userDetails, userEmail: e.target.value });
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                }}
                helperText={status === INVALID_EMAIL && "Email is not formatted."}
                FormHelperTextProps={{ error: true }}
            />
            <NumberFormat
                helpertext={status === INVALID_PHONE ? "funky number!" : ""}
                placeholder="Phone number (optional)"
                className={cls.phone}
                format={phonePattern} //"+61 (##) ####-####" //{regionSwitch ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                mask="_"
                type="tel"
                onError={() => setStatus(INVALID_PHONE)}
                onBlur={() => {
                    const result = checkUserPhone(userDetails.userPhone, setStatus);
                    if (result === false) setUserDetails({ ...userDetails, userPhone: "" });
                    setDetailsSet(checkUserDetailsAreSet(userDetails));
                }}
                onValueChange={values => {
                    setUserDetails({ ...userDetails, userPhone: values.formattedValue });
                    if (status === INVALID_PHONE) {
                        setStatus(null);
                    }
                }}
                value={userDetails.userPhone}
            />
        </>
    );
};
