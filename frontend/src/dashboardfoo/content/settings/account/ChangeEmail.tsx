import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";


export const ChangeEmail = () => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [Email, setEmail] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    const loadEmail = useCallback(async () => {

        var res = await client.Settings.Account.getEmail();
        setEmail(res.data);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadEmail();
        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [setEmail, loadEmail])


    const handleEmailChange = async (newEmail: string) => {

        await client.Settings.Account.updateEmail(newEmail);
        setAlert(true);
        setEmail(newEmail);
    }

    const alert = {
        title: "",
        message: "Email successfully updated."
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    name={"Update Email"}
                    details={" Update the Email used when sending response."}
                    placeholder={"New Email"}
                    onClick={handleEmailChange}
                    clearVal={true}
                    currentValue={Email}
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alert} />}
        </>
    )
}