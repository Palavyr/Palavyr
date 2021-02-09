import { ApiClient } from "@api-client/Client";
import React, { useCallback, useState, useEffect } from "react";
import { Divider, makeStyles, MenuItem } from "@material-ui/core";
import { SettingsGridRowList } from "@common/components/SettingsGridRowList";
import { Alert, AlertTitle } from "@material-ui/lab";
import { LocaleMapItem, LocaleMap } from "@Palavyr-Types";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
    paper: {
        backgroundColor: "#C7ECEE",
        padding: "2rem",
        margin: "1rem",
        width: "100%",
        display: "inline-block",
        justifyContent: "center",
        textAlign: "center",
        position: "relative",
    },
}));

const LocaleId = "LocaleId";
const CountryName = "CountryName";

export const ChangeLocale = () => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [, setLocaleID] = useState<string | undefined>();
    const [localeName, setLocaleName] = useState<string | undefined>();
    const [localeMap, setLocaleMap] = useState<LocaleMap>([]);
    const [currencySymbol, setCurrencySymbol] = useState<string>("");
    const [, setAlert] = useState<boolean>(false);

    const classes = useStyles();

    const loadLocale = useCallback(async () => {
        const { data: locale } = await client.Settings.Account.GetLocale();

        setLocaleID(locale.localeId);
        setLocaleName(locale.localeCountry);
        setCurrencySymbol(locale.localeCurrencySymbol);
        setLocaleMap(locale.localeMap);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        loadLocale();
        setLoaded(true);

        return () => {
            setLoaded(false);
        };
    }, [loadLocale]);

    const handleLocaleChange = async (event) => {
        const newLocaleId = event.target.value;

        const { data: updatedLocale } = await client.Settings.Account.updateLocale(newLocaleId);

        setLocaleName(updatedLocale.localeCountry);
        setLocaleID(updatedLocale.localeId);
        setCurrencySymbol(updatedLocale.localeCurrencySymbol);
        setAlert(true);
    };

    return (
        <>
            <div style={{ width: "50%" }}>
                <AreaConfigurationHeader title="Change your locale" subtitle="The locale affects the currency symbol used." />
                <Divider />
                <SettingsGridRowList
                    onChange={handleLocaleChange}
                    currentValue={localeName + " - " + currencySymbol}
                    menuName="Select your locale"
                    menu={localeMap.map((localeItem: LocaleMapItem, index: number) => (
                        <MenuItem key={localeItem.localeId + index.toString()} value={localeItem.localeId}>
                            {localeItem.countryName}
                        </MenuItem>
                    ))}
                    useModal
                    modalMessage={{
                        title: "",
                        message: "Locale successfully updated.",
                    }}
                    alertNode={
                        <Alert>
                            <AlertTitle className={classes.titleText}>Set your Locale</AlertTitle>
                            Set the locale of your company. This will be used to determine the following properties of your estimates:
                            <ul>
                                <li>Currency Symbol</li>
                            </ul>
                        </Alert>
                    }
                />
            </div>
        </>
    );
};
