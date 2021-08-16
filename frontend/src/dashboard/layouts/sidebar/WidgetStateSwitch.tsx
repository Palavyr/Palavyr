import React, { memo, useContext, useEffect, useState } from "react";
import { ListItem, ListItemText, FormControlLabel, Typography, makeStyles } from "@material-ui/core";
import { IOSSwitch } from "@common/components/IOSSwitch";
import { green, red } from "theme";
import { DashboardContext } from "../DashboardContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface WidgetStateSwitchProps {
    isActive: boolean;
    menuOpen: boolean;
}

const useStyles = makeStyles(theme => ({
    text: {
        color: "black",
    },
}));

export const WidgetStateSwitch = memo(({ isActive, menuOpen }: WidgetStateSwitchProps) => {
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
                {menuOpen && (
                    <PalavyrText className={cls.text} variant="h6">
                        Widget
                    </PalavyrText>
                )}
            </ListItemText>
            <FormControlLabel
                control={Switch}
                className={cls.text}
                label={menuOpen && (widgetState === undefined ? <PalavyrText>loading...</PalavyrText> : widgetState ? <PalavyrText>Enabled</PalavyrText> : <PalavyrText>Disabled</PalavyrText>)}
            />
        </ListItem>
    );
});
