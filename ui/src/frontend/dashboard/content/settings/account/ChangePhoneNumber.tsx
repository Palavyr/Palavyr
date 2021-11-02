import React, { useCallback, useState, useEffect, useContext } from "react";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Alert, AlertTitle } from "@material-ui/lab";
import { Divider, makeStyles } from "@material-ui/core";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const ChangePhoneNumber = () => {
    const { repository } = useContext(DashboardContext);
    const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [phoneNumber, setPhoneNumber] = useState<string>("");
    const [locale, setLocale] = useState<string>("");

    const loadPhoneNumber = useCallback(async () => {
        const { phoneNumber, locale } = await repository.Settings.Account.getPhoneNumber();
        setPhoneNumber(phoneNumber);
        setLocale(locale);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        loadPhoneNumber();
        setLoaded(true);

        return () => {
            setLoaded(false);
        };
    }, [setPhoneNumber, loadPhoneNumber]);

    const handlePhoneNumberChange = async (newPhoneNumber: string) => {
        await repository.Settings.Account.updatePhoneNumber(newPhoneNumber);
        setPhoneNumber(newPhoneNumber);
        return true;
    };

    return (
        <SettingsWrapper>
            <AreaConfigurationHeader
                title="Change your Primary Phone Number"
                subtitle="Update your primary phone number. This is the primary contact phone number provided in the response email and pdf sent to customers."
            />
            <Divider />
            <SettingsGridRowText
                fullWidth
                inputType="phone"
                placeholder="New Phone Number"
                onClick={handlePhoneNumberChange}
                clearVal={true}
                currentValue={phoneNumber}
                successText="Successfully updated Phone Number"
                alertNode={
                    <Alert severity={phoneNumber ? "success" : "error"}>
                        <AlertTitle className={classes.titleText}>{phoneNumber ? "Phone Number" : "Set your phone number."}</AlertTitle>
                        Set your company or business contact phone number. This will be used in the header of each response PDF sent via the widget.
                    </Alert>
                }
                locale={locale}
            />
        </SettingsWrapper>
    );
};
