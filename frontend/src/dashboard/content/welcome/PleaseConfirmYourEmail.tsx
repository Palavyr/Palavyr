import * as React from "react";
import { ApiClient } from "@api-client/Client";
import { LocalStorage } from "localStorage/localStorage";
import classNames from "classnames";
import { Card, Typography, FormControl, InputLabel, OutlinedInput, makeStyles } from "@material-ui/core";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import auth from "auth/Auth";

const useStyles = makeStyles((theme) => ({
    contentRoot: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: "transparent",
        margin: "0px",
    },
    card: {
        backgroundColor: "#C7ECEE",
        width: "50%",
        padding: "3rem",
        margin: "3rem",
    },
    centerText: {
        textAlign: "center",
    },
    margin: {
        margin: theme.spacing(1),
    },
    inputLabel: { background: "white", borderRadius: "5px", boxShadow: "0px 0px 12px 5px white" },
    outlinedInput: { background: "white" },
}));

export const PleaseConfirmYourEmail = () => {
    const client = new ApiClient();
    const [authToken, setAuthToken] = useState<string>("");
    const [authStatus, setAuthStatus] = useState<string | null>(null);
    const emailAddress = LocalStorage.getEmailAddress();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const cls = useStyles();

    const confirmAccount = async () => {
        setIsLoading(true);
        if (authToken === "") return false;
        const { data: emailConfirmed } = await client.Settings.Account.confirmEmailAddress(authToken);
        if (emailConfirmed === true) {
            setIsLoading(false);
            auth.SetIsActive();
            window.location.reload();
        } else {
            setIsLoading(false);
            setAuthStatus("CodeNotVerified");
            alert("Authorization code incorrect.");
        }
    };

    const outlinedInputOnChange = (event: { target: { value: React.SetStateAction<string> } }) => {
        setAuthToken(event.target.value);
    };

    return (
        <div className={classNames(cls.contentRoot, cls.centerText)}>
            <Card className={cls.card}>
                <Typography variant="body2">
                    <strong>Please provide the access token emailed to: {emailAddress}</strong>
                </Typography>
                <br />
                <br />
                <FormControl fullWidth className={cls.margin} variant="outlined">
                    <InputLabel className={cls.inputLabel} htmlFor="outlined-adornment-confirmation-code">
                        Confirmation Code
                    </InputLabel>
                    <OutlinedInput className={cls.outlinedInput} placeholder="Paste your confirmation code here" value={authToken} id="outlined-adornment-confirmation-code" onChange={outlinedInputOnChange} labelWidth={140} />
                    <br />
                    <ColoredButton type="submit" variant="contained" color="primary" disabled={isLoading} onClick={confirmAccount}>
                        Submit
                        {isLoading && <ButtonCircularProgress />}
                    </ColoredButton>
                </FormControl>
            </Card>
        </div>
    );
};
