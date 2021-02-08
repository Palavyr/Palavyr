import * as React from "react";
import { TextField, makeStyles } from "@material-ui/core";
import { useState } from "react";
import { UserDetails } from "src/widgetCore/store/types";
import { Dispatch } from "react";
import { SetStateAction } from "react";

export interface IFormDialogContent {
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
}

const useStyles = makeStyles({
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
});

const INVALID_EMAIL = "invalid_email";

export const UserDetailsDialogContent = ({userDetails, setUserDetails}: IFormDialogContent) => {

    const [status, setStatus] = useState<string>("valid");


    return (
        <>
            <TextField
                variant="outlined"
                margin="normal"
                error={status === INVALID_EMAIL}
                required
                fullWidth
                label="Email Address"
                value={userDetails.userEmail}
                autoFocus
                autoComplete="off"
                type="email"
                onChange={e => {
                    setUserDetails({...userDetails, userEmail: e.target.value});
                    if (status === INVALID_EMAIL) {
                        setStatus(null);
                    }
                }}
                helperText={status === INVALID_EMAIL && "Email is not formatted."}
                FormHelperTextProps={{ error: true }}
            />
        </>
    );
};
