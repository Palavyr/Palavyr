import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Grid, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { Alert, AlertTitle } from "@material-ui/lab";

const useStyles = makeStyles((theme) => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const ChangeCompanyName = () => {
    const client = new ApiClient();
    const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [companyName, setCompanyName] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    const loadCompanyName = useCallback(async () => {
        const name = (await client.Settings.Account.getCompanyName()).data as string;
        setCompanyName(name);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        loadCompanyName();
        setLoaded(true);

        return () => {
            setLoaded(false);
        };
    }, [setCompanyName, loadCompanyName]);

    const handleCompanyNameChange = async (newCompanyName: string) => {
        await client.Settings.Account.updateCompanyName(newCompanyName);
        setCompanyName(newCompanyName);
    };

    const alert = {
        title: "",
        message: "Company Name successfully updated.",
    };

    return (
        <div style={{ width: "50%" }}>
            <SettingsGridRowText
                fullWidth
                placeholder={"New Company Name"}
                onClick={handleCompanyNameChange}
                clearVal={true}
                currentValue={companyName}
                useAlert
                alertMessage={{
                    title: "Successfully updated",
                    message: "",
                }}
                alertNode={
                    <Alert severity={companyName === "" ? "error" : "success"}>
                        <AlertTitle className={classes.titleText}>{companyName === "" ? "Please set your company name." : "Company / Business name name."}</AlertTitle>
                        Set your company or business name. This will be used in the header of each response PDF sent via the widget.
                    </Alert>
                }
            />
        </div>
    );
};
