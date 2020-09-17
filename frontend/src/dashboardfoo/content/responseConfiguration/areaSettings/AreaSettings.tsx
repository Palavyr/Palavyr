import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { useHistory } from "react-router-dom";
import { Grid } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";


interface IAreaSettings {
    areaName: string;
    areaIdentifier: string;
    setViewName: any;
}

export const AreaSettings = ({ areaName, areaIdentifier, setViewName }: IAreaSettings) => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>();
    const [currentDisplayTitle, setCurrentAreaDisplayTitle] = useState<string>("");
    const history = useHistory();

    const loadSettings = useCallback(async () => {

        var res = await client.Area.GetArea(areaIdentifier);
        setCurrentAreaDisplayTitle(res.data.areaDisplayTitle);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        loadSettings();
        setLoaded(true);
        return () => {
            setLoaded(false)
        }
    }, [loadSettings])

    const handleAreaNameChange = async (newAreaName: any) => {

        setViewName(newAreaName);
        var res = await client.Area.updateArea(areaIdentifier, newAreaName, null);
        console.log(res);
        window.location.reload();
    }

    const handleAreaDisplayTitleChange = async (newAreaDisplayTitle: any) => {

        var res = await client.Area.updateArea(areaIdentifier, null, newAreaDisplayTitle);
        console.log(res);
        window.location.reload();
    }

    const handleAreaDelete = async () => {

        await client.Area.deleteArea(areaIdentifier);
        history.push("/");
        window.location.reload();
    }

    return (
        <Grid container spacing={3}>
            <SettingsGridRowText
                name={"Update Area Name"}
                details={" Update the name of this area for dashboard."}
                placeholder={"New Area Name"}
                currentValue={areaName}
                onClick={handleAreaNameChange}
                clearVal={false}
            />
            <SettingsGridRowText
                name={"Update Area Display Title"}
                details={" Update the area title used in the widget."}
                placeholder={"New Area Display Title"}
                currentValue={currentDisplayTitle}
                onClick={handleAreaDisplayTitleChange}
                clearVal={false}
            />
            <SettingsGridRowText
                name={"Delete Area"}
                details={"Permanently delete this area."}
                onClick={handleAreaDelete}
                clearVal={false}
                buttonText={"Permanently Delete"}
            />
        </Grid>
        // TODO: Force users to type in the area name in order to delete it.
    )
}