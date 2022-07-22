import React, { useState, useCallback, useEffect, useContext } from "react";
import { Divider, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Alert, AlertTitle } from "@material-ui/lab";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const ChangeCompanyName = () => {
    const { repository } = useContext(DashboardContext);
    const classes = useStyles();

    const [, setLoaded] = useState<boolean>(false);
    const [companyName, setCompanyName] = useState<string | null>(null);

    const [] = useState<boolean>(false);

    const loadCompanyName = useCallback(async () => {
        const name = await repository.Settings.Account.GetCompanyName();
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
        await repository.Settings.Account.UpdateCompanyName(newCompanyName);
        setCompanyName(newCompanyName);
    };

    return (
        <SettingsWrapper>
            <HeaderStrip title="Change your company name" subtitle="Update your company name. This is used in the response email and pdf sent to customers." />
            <Divider />
            <SettingsGridRowText
                fullWidth
                loading={companyName === null}
                placeholder="New Company Name"
                onClick={handleCompanyNameChange}
                clearVal={true}
                currentValue={companyName ?? ""}
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
