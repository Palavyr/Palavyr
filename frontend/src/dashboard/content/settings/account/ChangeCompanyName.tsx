import { PalavyrRepository } from "@api-client/PalavyrRepository";
import React, { useState, useCallback, useEffect } from "react";
import { Divider, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Alert, AlertTitle } from "@material-ui/lab";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { SettingsWrapper } from "../SettingsWrapper";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const ChangeCompanyName = () => {
    const repository = new PalavyrRepository();
    const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [companyName, setCompanyName] = useState<string>("");

    const [] = useState<boolean>(false);

    const loadCompanyName = useCallback(async () => {
        const name = await repository.Settings.Account.getCompanyName();
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
        await repository.Settings.Account.updateCompanyName(newCompanyName);
        setCompanyName(newCompanyName);
    };

    return (
        <SettingsWrapper>
            <AreaConfigurationHeader title="Change your company name" subtitle="Update your company name. This is used in the response email and pdf sent to customers." />
            <Divider />
            <SettingsGridRowText
                fullWidth
                placeholder="New Company Name"
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
                        <AlertTitle className={classes.titleText}>{companyName === "" ? "Please set your company name." : "Company / Business name."}</AlertTitle>
                        Set your company or business name. This will be used in the header of each response PDF sent via the widget.
                    </Alert>
                }
            />
        </SettingsWrapper>
    );
};
