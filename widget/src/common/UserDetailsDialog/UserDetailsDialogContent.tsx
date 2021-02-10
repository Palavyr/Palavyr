import * as React from "react";
import { TextField, makeStyles, fade } from "@material-ui/core";
import { useState } from "react";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { LocaleMap, LocaleMapItem, UserDetails } from "src/types";

export interface IFormDialogContent {
    status: string;
    setStatus: any;
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
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

export const UserDetailsDialogContent = ({ status, setStatus, userDetails, setUserDetails }: IFormDialogContent) => {
    const [, setLocaleID] = useState<string | undefined>();
    const [localeMap] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");

    const cls = useStyles();

    const sortGetter = (opt: LocaleMapItem) => opt.countryName;
    const onChange = (event: any, newOption: LocaleMapItem) => {
        setLocaleID(newOption.localeId);
        setphonePattern(newOption.phonePattern);
    };
    return (
        <>
            {/* <TextField
                margin="normal"
                error={status == INVALID_NAME}
                required
                fullWidth
                label="Name"
                value={userDetails.userName}
                autoFocus
                autoComplete="off"
                type="text"
                onBlur={() => {
                    const result = checkUserName(userDetails.userName, setStatus);
                    if (!result) setStatus(INVALID_NAME);
                }}
                onChange={event => {
                    setUserDetails({ ...userDetails, userName: event.target.value });
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                }}
                helperText={status === INVALID_NAME && "Name is not set"}
                FormHelperTextProps={{ error: true }}
            /> */}
            {/* <TextField
                margin="normal"
                error={status === INVALID_EMAIL}
                required
                fullWidth
                label="Email Address"
                value={userDetails.userEmail}
                autoComplete="off"
                type="email"
                onBlur={() => {
                    const result = checkUserEmail(userDetails.userEmail, setStatus);
                    if (!result) setStatus(INVALID_EMAIL);
                }}
                onChange={e => {
                    setUserDetails({ ...userDetails, userEmail: e.target.value });
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                }}
                helperText={status === INVALID_EMAIL && "Email is not formatted."}
                FormHelperTextProps={{ error: true }}
            /> */}
            {/* <NumberFormat
                helpertext={status === INVALID_PHONE ? "funky number!" : ""}
                placeholder="Phone number (optional)"
                className={cls.phone}
                format={phonePattern} //"+61 (##) ####-####" //{regionSwitch ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                mask="_"
                type="tel"
                onError={() => setStatus(INVALID_PHONE)}
                onBlur={() => {
                    const result = checkUserPhone(userDetails.userPhone, setStatus);
                    if (!result) setStatus(INVALID_EMAIL);
                }}
                onValueChange={values => {
                    setUserDetails({ ...userDetails, userPhone: values.formattedValue });
                    if (status === INVALID_PHONE) {
                        setStatus(null);
                    }
                }}
                value={userDetails.userPhone}
            /> */}
            {/* <div>
                {localeMap && (
                    <Autocomplete
                        size="small"
                        disableClearable
                        clearOnEscape
                        onChange={onChange}
                        options={sortByPropertyAlphabetical(sortGetter, localeMap)}
                        getOptionLabel={(option: LocaleMapItem) => option.countryName}
                        renderInput={(params: AutocompleteRenderInputParams) => <TextField label="Select your locale..." ref={params.InputProps.ref} {...params} />}
                    />
                )}
            </div> */}
        </>
    );
};
