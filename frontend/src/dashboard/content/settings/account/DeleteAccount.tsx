import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import React, { useContext } from "react";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import Auth from "auth/Auth";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const DeleteAccount = () => {
    const { repository } = useContext(DashboardContext);
    const [alertState, setAlert] = useState<boolean>(false);
    const [] = useState<boolean>(false);
    const cls = useStyles();

    const history = useHistory();

    const handleAccountDelete = async () => {
        Auth.ClearAuthentication();
        alert("We're sorry to see you go!");
        await repository.Settings.Account.DeleteAccount();
        history.push("/");
    };
    const alertMessage = {
        title: "",
        message: "User Name successfully updated.",
    };
    return (
        <SettingsWrapper>
            <AreaConfigurationHeader title="Delete your account" subtitle="Caution - account deletion is permanent." />
            <Divider />
            <SettingsGridRowText
                onClick={handleAccountDelete}
                clearVal={true}
                buttonText="Delete Account"
                alertNode={
                    <Alert severity="error">
                        <AlertTitle className={cls.titleText}>
                            <Typography>Delete your account</Typography>
                        </AlertTitle>
                        <p>Cancel your subscription and permanently delete your account.</p>
                    </Alert>
                }
            />
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alertMessage} />}
        </SettingsWrapper>
    );
};
