import { ApiClient } from "@api-client/Client";
import React, { useCallback, useState, useEffect } from "react";
import { Grid, MenuItem, Select } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { SettingsGridRowList } from "@common/components/SettingsGridRowList";


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

    const alert = {
        title: "",
        message: "Locale successfully updated."
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowList
                    name={"Update Locale"}
                    details={"Update the Locale used when setting currency, etc."}
                    onClick={handleLocaleChange}
                    currentValue={localeName}
                    menuName={"Locale"}
                    menu={
                        localeId && <Select
                            labelId="select-list-label"
                            id="select-text-list"
                            value={localeId}
                            onChange={handleLocaleChange}
                        >
                            {
                                Object.keys(supportedLocales).map(locKey => <MenuItem key={locKey} value={locKey}>{supportedLocales[locKey]}</MenuItem>)
                            }
                       </Select>
                    }
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={alert} />}
        </>
    )
}