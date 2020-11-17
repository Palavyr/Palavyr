import { ApiClient } from "@api-client/Client";
import React, { useCallback, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { useState } from "react";


export const ChangeUserName = () => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [UserName, setUserName] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    const loadUserName = useCallback(async () => {
        var {data: username} = await client.Settings.Account.getUserName();
        setUserName(username);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadUserName();
        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [setUserName, loadUserName])


    const handleUserNameChange = async (newUserName: string) => {

        await client.Settings.Account.updateUserName(newUserName);
        setAlert(true);
        setUserName(newUserName);
    }

    const alert = {
        title: "",
        message: "User Name successfully updated."
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    name={"Update User name"}
                    details={" Update the User name used when sending response."}
                    placeholder={"New User Name"}
                    onClick={handleUserNameChange}
                    clearVal={true}
                    currentValue={UserName}
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alert} />}
        </>
    )
}