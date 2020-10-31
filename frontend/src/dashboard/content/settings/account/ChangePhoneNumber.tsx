import { ApiClient } from "@api-client/Client";
import React, { useCallback, useState, useEffect } from "react";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Alert, AlertTitle } from "@material-ui/lab";
import { makeStyles } from "@material-ui/core";
import NumberFormat from 'react-number-format';

const useStyles = makeStyles(theme => ({
    titleText: {
        fontWeight: "bold"
    }
}));


export const ChangePhoneNumber = () => {
    var client = new ApiClient();
    const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [phoneNumber, setPhoneNumber] = useState<string>("");
    const [locale, setLocale] = useState<string>("");

    const loadPhoneNumber = useCallback(async () => {

        var data = (await client.Settings.Account.getPhoneNumber()).data;
        setPhoneNumber(data.phoneNumber);
        setLocale(data.locale)
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
        setPhoneNumber(newPhoneNumber);
        return true;
    }


    return (
        <div style={{ width: "50%" }}>
            <SettingsGridRowText
                fullWidth
                inputType="phone"
                placeholder={"New Phone Number"}
                onClick={handlePhoneNumberChange}
                clearVal={true}
                currentValue={phoneNumber}
                useAlert
                alertMessage={{
                    title: "Phone Number successfully updated.",
                    message: ""
                }}
                alertNode={
                    <Alert severity={phoneNumber ? "success" : "error"}>
                        <AlertTitle className={classes.titleText}>
                            {
                                phoneNumber
                                    ? "Phone Number"
                                    : "Please set your phone number."
                            }
                        </AlertTitle>
                            Set your company or business contact phone number. This will be used in the header of each response PDF sent via the widget.
                        </Alert>
                }
                locale={locale}
            />
        </div>
    )
}