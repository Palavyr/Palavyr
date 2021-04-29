import React, { useEffect, useState } from "react";
import { ListItem, ListItemText, FormControlLabel, Typography, makeStyles } from "@material-ui/core";
import { IOSSwitch } from "@common/components/IOSSwitch";
import { green, red } from "theme";
import { ApiClient } from "@api-client/Client";

export interface WidgetStateSwitchProps {
    isActive: boolean;
}

const useStyles = makeStyles((theme) => ({
    text: {
        color: "black",
    },
}));

export const WidgetStateSwitch = ({ isActive }: WidgetStateSwitchProps) => {
    const [widgetState, setWidgetState] = useState<boolean | undefined>();
    const cls = useStyles();

    const updatewidgetState = async () => {
        const client = new ApiClient();
        const { data: updatedWidgetState } = await client.Configuration.WidgetState.SetWidgetState(!widgetState);
        setWidgetState(updatedWidgetState);
    };

    useEffect(() => {
        const client = new ApiClient();
        (async () => {
            const { data: currentWidgetState } = await client.Configuration.WidgetState.GetWidgetState();
            setWidgetState(currentWidgetState);
        })();
    }, []);

    const Switch = <IOSSwitch disabled={!isActive || widgetState === undefined} checked={widgetState === undefined ? true : widgetState ? true : false} onChange={updatewidgetState} name="Active" />;

    return (
        <ListItem style={{ backgroundColor: widgetState === undefined ? "gray" : widgetState ? green : red }} disabled={!isActive}>
            <ListItemText>
                <Typography className={cls.text} variant="h6">
                    Widget
                </Typography>
            </ListItemText>
            <FormControlLabel control={Switch} className={cls.text} label={widgetState === undefined ? "loading..." : widgetState ? "Enabled" : "Disabled"} />
        </ListItem>
    );
};