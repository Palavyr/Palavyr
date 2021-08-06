import React, { memo, useContext, useEffect, useState } from "react";
import { ListItem, ListItemText, FormControlLabel, Typography, makeStyles } from "@material-ui/core";
import { IOSSwitch } from "@common/components/IOSSwitch";
import { green, red } from "theme";
import { DashboardContext } from "../DashboardContext";

export interface WidgetStateSwitchProps {
    isActive: boolean;
}

const useStyles = makeStyles((theme) => ({
    text: {
        color: "black",
    },
}));

export const WidgetStateSwitch = memo(({ isActive }: WidgetStateSwitchProps) => {
    const [widgetState, setWidgetState] = useState<boolean | undefined>();
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);

    const updatewidgetState = async () => {
        const updatedWidgetState = await repository.Configuration.WidgetState.SetWidgetState(!widgetState);
        setWidgetState(updatedWidgetState);
    };

    useEffect(() => {
        (async () => {
            const currentWidgetState = await repository.Configuration.WidgetState.GetWidgetState();
            setWidgetState(currentWidgetState);
        })();
    }, []);

    const Switch = <IOSSwitch disabled={!isActive || widgetState === undefined} checked={widgetState === undefined ? true : widgetState ? true : false} onChange={updatewidgetState} name="Active" />;

    return (
        <ListItem className="widget-state-switch" style={{ backgroundColor: widgetState === undefined ? "gray" : widgetState ? green : red }} disabled={!isActive}>
            <ListItemText>
                <Typography className={cls.text} variant="h6">
                    Widget
                </Typography>
            </ListItemText>
            <FormControlLabel control={Switch} className={cls.text} label={widgetState === undefined ? "loading..." : widgetState ? "Enabled" : "Disabled"} />
        </ListItem>
    );
});
