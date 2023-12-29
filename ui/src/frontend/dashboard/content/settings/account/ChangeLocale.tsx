import React, { useCallback, useState, useEffect, useContext } from "react";
import { Divider, makeStyles, MenuItem, Typography } from "@material-ui/core";
import { SettingsGridRowList } from "@common/components/SettingsGridRowList";
import { Alert, AlertTitle } from "@material-ui/lab";
import { LocaleMap, LocaleResource } from "@Palavyr-Types";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";


const useStyles = makeStyles<{}>((theme: any) => ({
    titleText: {
        fontWeight: "bold",
    },
    paper: {
        backgroundColor: theme.palette.secondary.light,
        padding: "2rem",
        margin: "1rem",
        width: "100%",
        display: "inline-block",
        justifyContent: "center",
        textAlign: "center",
        position: "relative",
    },
}));

export const ChangeLocale = () => {
    const { repository } = useContext(DashboardContext);

    const [, setLoaded] = useState<boolean>(false);
    const [, setLocaleID] = useState<string | undefined>();
    const [localeName, setLocaleName] = useState<string | undefined>();
    const [localeMap, setLocaleMap] = useState<LocaleMap>([]);
    const [currencySymbol, setCurrencySymbol] = useState<string>("");
    const [, setAlert] = useState<boolean>(false);

    const classes = useStyles();

    const loadLocale = useCallback(async () => {
        const { currentLocale: locale, localeMap } = await repository.Settings.Account.GetLocale();
        setLocaleID(locale.name);
        setLocaleName(locale.displayName);
        setCurrencySymbol(locale.currencySymbol);
        setLocaleMap(localeMap);
    }, []);

    useEffect(() => {
        loadLocale();
        setLoaded(true);

        return () => {
            setLoaded(false);
        };
    }, [loadLocale]);

    const handleLocaleChange = async event => {
        const newLocaleId = event.target.value;
        const { currentLocale: updatedLocale, localeMap } = await repository.Settings.Account.UpdateLocale(newLocaleId);
        setLocaleName(updatedLocale.displayName);
        setLocaleID(updatedLocale.name);
        setCurrencySymbol(updatedLocale.currencySymbol);
        setAlert(true);
    };

    return (
        <>
            {localeMap && (
                <SettingsWrapper>
                    <HeaderStrip title="Change your locale" subtitle="The locale affects the currency symbol used." />
                    <Divider />
                    <SettingsGridRowList
                        loading={localeName === undefined}
                        onChange={handleLocaleChange}
                        currentValue={localeName + " - " + currencySymbol}
                        menuName="Select your locale"
                        menu={localeMap.map((localeItem: LocaleResource, index: number) => (
                            <MenuItem key={localeItem.name + index.toString()} value={localeItem.name}>
                                {localeItem.displayName}
                            </MenuItem>
                        ))}
                        useModal
                        modalMessage="Locale successfully updated."
                        alertNode={
                            <Alert>
                                <AlertTitle className={classes.titleText}>Set your Locale</AlertTitle>
                                <Typography>This will be used to determine the following properties of your estimates:</Typography>
                                <ul>
                                    <li>Currency Symbol</li>
                                </ul>
                            </Alert>
                        }
                    />
                </SettingsWrapper>
            )}
        </>
    );
};
