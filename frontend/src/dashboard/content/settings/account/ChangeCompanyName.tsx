import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";


export const ChangeCompanyName = () => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [companyName, setCompanyName] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    const loadCompanyName = useCallback(async () => {
        var res = await client.Settings.Account.getCompanyName();
        setCompanyName(res.data);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadCompanyName();
        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [setCompanyName, loadCompanyName])


    const handleCompanyNameChange = async (newCompanyName: string) => {

        await client.Settings.Account.updateCompanyName(newCompanyName);
        setAlert(true);
        setCompanyName(newCompanyName);
    }

    const alert = {
        title: "",
        message: "Company Name successfully updated."
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    fullWidth
                    name={"Update company name"}
                    details={" Update the company name used when sending response."}
                    placeholder={"New Company Name"}
                    onClick={handleCompanyNameChange}
                    clearVal={true}
                    currentValue={companyName}
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alert} />}
        </>
    )
}