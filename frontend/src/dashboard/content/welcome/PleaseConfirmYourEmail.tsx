import * as React from 'react';
import { ApiClient } from '@api-client/Client';
import { LocalStorage } from 'localStorage/localStorage';
import classNames from 'classnames';
import { Card, Typography, FormControl, InputLabel, OutlinedInput, makeStyles } from '@material-ui/core';
import { ColoredButton } from '@common/components/borrowed/ColoredButton';
import { ButtonCircularProgress } from '@common/components/borrowed/ButtonCircularProgress';
import { useState } from 'react';


const useStyles = makeStyles(theme => ({
    contentRoot: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
        margin: "0px",
      },
    card: {
        padding: "3rem",
        margin: "3rem"
    },
    centerText: {
        textAlign: "center"
    },
    margin: {
        margin: theme.spacing(1),
      },
}))


export const PleaseConfirmYourEmail = () => {

    const client = new ApiClient();
    const [authToken, setAuthToken] = useState<string>("");
    const [authStatus, setAuthStatus] = useState<string | null>(null);
    const emailAddress = LocalStorage.getEmailAddress();
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const classes = useStyles()

    const confirmAccount = async () => {
        setIsLoading(true);
        if (authToken === "") return false;
        var res = await client.Settings.Account.confirmEmailAddress(authToken)
        if (res.data === true) {
            setIsLoading(false)
            window.location.reload()
        } else {
            setIsLoading(false);
            setAuthStatus("CodeNotVerified");
        }
    }

    return (
        <div className={classNames(classes.contentRoot, classes.centerText)}>
            <Card className={classes.card}>
                <Typography variant="body2">
                    <strong>Please provide the access token emailed to: {emailAddress}</strong>
                </Typography>
                <br /><br />

                <FormControl fullWidth className={classes.margin} variant="outlined">
                    <InputLabel htmlFor="outlined-adornment-confirmation-code">Confirmation Code</InputLabel>
                    <OutlinedInput
                        placeholder="Paste your confirmation code here"
                        value={authToken}
                        id="outlined-adornment-confirmation-code"
                        onChange={(e) => setAuthToken(e.target.value)}
                        labelWidth={140}
                    />
                    <br />
                    <ColoredButton
                        type="submit"
                        variant="contained"
                        color="primary"
                        disabled={isLoading}
                        onClick={confirmAccount}
                    >
                        Submit
                        {isLoading && <ButtonCircularProgress />}
                    </ColoredButton>
                </FormControl>
            </Card>
        </div>
    )
}