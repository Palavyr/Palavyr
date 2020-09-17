import { ApiClient } from "@api-client/Client";
import React, { useCallback, useState, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";


export const ChangePhoneNumber = () => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [PhoneNumber, setPhoneNumber] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    const loadPhoneNumber = useCallback(async () => {

        var res = await client.Settings.Account.getPhoneNumber();
        setPhoneNumber(res.data);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadPhoneNumber();
        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [setPhoneNumber, loadPhoneNumber])


    const handlePhoneNumberChange = async (newPhoneNumber: string) => {

        await client.Settings.Account.updatePhoneNumber(newPhoneNumber);
        setAlert(true);
        setPhoneNumber(newPhoneNumber);
    }

    const alert = {
        title: "",
        message: "Phone Number successfully updated."
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    name={"Update Phone Number"}
                    details={" Update the Phone Number used when sending response."}
                    placeholder={"New Phone Number"}
                    onClick={handlePhoneNumberChange}
                    clearVal={true}
                    currentValue={PhoneNumber}
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alert} />}
        </>
    )
}