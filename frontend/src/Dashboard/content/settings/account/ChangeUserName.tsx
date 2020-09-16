import { ApiClient } from "@api-client/Client";
import React, { useCallback, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";


export const ChangeUserName = () => {
    var client = new ApiClient();

    const [, setLoaded] = React.useState<boolean>(false);
    const [UserName, setUserName] = React.useState<string>("");

    const [alertState, setAlert] = React.useState<boolean>(false);

    const loadUserName = useCallback(async () => {
        var res = await client.Settings.Account.getUserName();
        setUserName(res.data);
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