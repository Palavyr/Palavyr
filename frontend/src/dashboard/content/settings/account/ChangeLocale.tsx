import { ApiClient } from "@api-client/Client";
import React, { useCallback, useState, useEffect } from "react";
import { Grid, makeStyles, MenuItem, Select } from "@material-ui/core";
import { SettingsGridRowList } from "@common/components/SettingsGridRowList";
import { Alert, AlertTitle } from "@material-ui/lab";

const useStyles = makeStyles(theme => ({
    titleText: {
        fontWeight: "bold"
    }
}))

export const ChangeLocale = () => {
    var client = new ApiClient();

    const supportedLocales = {
        "en-AU": "Australia",
        "en-US": "United States"
    }

    const [, setLoaded] = useState<boolean>(false);
    const [localeId, setLocaleID] = useState<string | undefined>();
    const [localeName, setLocaleName] = useState<string | undefined>();

    const [alertState, setAlert] = useState<boolean>(false);

    const classes = useStyles();

    const loadLocale = useCallback(async () => {

        var res = (await client.Settings.Account.getLocale()).data;

        setLocaleID(res)
        setLocaleName(supportedLocales[res]);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadLocale();

        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [loadLocale])


    const handleLocaleChange = async (event) => {
        const newLocaleId = event.target.value;

        await client.Settings.Account.updateLocale(newLocaleId);
        setAlert(true);

        setLocaleName(supportedLocales[newLocaleId])
        setLocaleID(newLocaleId);
    }

    return (
        <div style={{ width: "50%" }}>
            <Grid container spacing={3}>
                <SettingsGridRowList
                    onChange={handleLocaleChange}
                    currentValue={localeName}
                    menuName={"Locale"}
                    menu={Object.keys(supportedLocales).map(locKey => <MenuItem key={locKey} value={locKey}>{supportedLocales[locKey]}</MenuItem>)}
                    useModal
                    modalMessage={
                        {
                            title: "",
                            message: "Locale successfully updated."
                        }
                    }
                    alertNode={
                        <Alert>
                            <AlertTitle className={classes.titleText}>
                                Set your Locale
                            </AlertTitle>
                            Set the locale of your company. This will be used to determine the following properties of your estimates:
                            <ul><li>Currency Symbol</li></ul>
                        </Alert>
                    }
                />
            </Grid>
        </div>
    )
}