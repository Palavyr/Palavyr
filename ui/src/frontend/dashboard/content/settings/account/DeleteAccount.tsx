import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Button, Dialog, Divider, makeStyles, Typography } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import React, { useContext } from "react";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import Auth from "@auth/Auth";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import Cookies from "js-cookie";
import { ALL_COOKIE_NAMES } from "@constants";
import ErrorIcon from "@material-ui/icons/Error";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";


const useStyles = makeStyles<{}>((theme: any) => ({
    titleText: {
        fontWeight: "bold",
    },
    buttonHover: {
        "&:hover": {
            backgroundColor: theme.palette.error.main,
            color: theme.palette.warning.light,
        },
    },
}));

export const DeleteAccount = () => {
    const { repository } = useContext(DashboardContext);
    const [alertState, setAlert] = useState<boolean>(false);
    const [] = useState<boolean>(false);
    const cls = useStyles();

    const history = useHistory();

    const handleAccountDelete = async () => {
        alert("We're sorry to see you go!");
        await repository.Settings.Account.DeleteAccount();
        Auth.ClearAuthentication();
        ALL_COOKIE_NAMES.forEach((x: string) => Cookies.remove(x));
        history.push("/");
    };
    const alertMessage = {
        title: "",
        message: "User Name successfully updated.",
    };

    const [dialogOpen, setDialogOpen] = useState<boolean>(false);

    return (
        <SettingsWrapper>
            <HeaderStrip
                title="Delete your account"
                subtitle={
                    <PalavyrText>
                        Caution - Account Deletion is{" "}
                        <i>
                            <b>permanent</b>
                        </i>
                    </PalavyrText>
                }
            />
            <Divider />
            <SettingsGridRowText
                onClick={async () => await Promise.resolve(setDialogOpen(true))}
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
            <Dialog PaperProps={{ style: { textAlign: "center", margin: "2rem", padding: "2rem" } }} style={{ margin: "2rem", padding: "2rem" }} open={dialogOpen} onClose={() => setDialogOpen(false)}>
                <Typography variant="h4">Are you sure you want to delete your account?</Typography>
                <Typography>This is permanent and cannot be undone. If you have a subscription, it will be automatically cancelled. This cannot be recovered. </Typography>
                <div style={{ marginTop: "1rem", display: "flex", flexDirection: "row", justifyContent: "center", width: "100%" }}>
                    <ErrorIcon style={{ float: "left", fontSize: "3rem", color: "red", marginBottom: ".1rem" }} />
                    <ErrorIcon style={{ float: "right", fontSize: "2.5rem", color: "red", marginTop: ".2rem" }} />
                </div>
                <div style={{ marginBottom: "1rem", display: "flex", flexDirection: "row", justifyContent: "center", width: "100%" }}>
                    <ErrorIcon style={{ fontSize: "1.6rem", color: "red" }} />
                </div>

                <Button className={cls.buttonHover} onClick={handleAccountDelete}>
                    PERMANENTLY DELETE
                </Button>
            </Dialog>
        </SettingsWrapper>
    );
};
