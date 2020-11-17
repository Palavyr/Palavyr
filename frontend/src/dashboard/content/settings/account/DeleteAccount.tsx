import { ApiClient } from "@api-client/Client";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Grid, makeStyles, Typography } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import { AuthContext } from "dashboard/layouts/DashboardContext";
import React from "react";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import Auth from "auth/Auth";

const useStyles = makeStyles(theme => ({
    titleText: {
        fontWeight: "bold"
    }
}));


export const DeleteAccount = () => {
    var client = new ApiClient();
    const [alertState, setAlert] = useState<boolean>(false);
    const [, setLoaded] = useState<boolean>(false);
    const cls = useStyles();

    const history = useHistory();

    const handleAccountDelete = async () => {
        const { data: result } = await client.Settings.Account.DeleteAccount();
        Auth.ClearAuthentication();
        alert("We're sorry to see you go!")
        history.push("/")
    };
    const alertMessage = {
        title: "",
        message: "User Name successfully updated.",
    };
    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    name="Delete your account."
                    details="Cancel your subscription and permanently delete your account."
                    onClick={handleAccountDelete}
                    clearVal={true}
                    buttonText="Delete Account"
                    alertNode={
                        <Alert severity="error">
                            <AlertTitle className={cls.titleText}>
                                <Typography>Delete your account</Typography>
                            </AlertTitle>
                            <p>Cancel your subscription and permanently delete your account.</p>{" "}
                        </Alert>
                    }
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alertMessage} />}
        </>
    );
};
