import React, { memo, useContext, useEffect, useState } from "react";
import { ListItem, ListItemText, FormControlLabel, Typography, makeStyles, Tooltip } from "@material-ui/core";
import { IOSSwitch } from "@common/components/IOSSwitch";
import { DashboardContext } from "../DashboardContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import classNames from "classnames";

export interface WidgetStateSwitchProps {
    isActive: boolean;
    menuOpen: boolean;
}

type StyleProps = {
    widgetState: boolean | undefined;
};

const useStyles = makeStyles(theme => ({
    text: {
        color: "black",
    },
    item: (props: StyleProps) => ({
        border: `2px solid ${props.widgetState !== undefined ? (props.widgetState ? theme.palette.success.main : theme.palette.error.light) : theme.palette.grey[100]}`,
        backgroundColor: props.widgetState !== undefined ? (props.widgetState ? theme.palette.success.main : theme.palette.error.light) : theme.palette.grey[100],
        height: "64px",
    }),
}));

export const WidgetStateSwitch = memo(({ isActive, menuOpen }: WidgetStateSwitchProps) => {
    const [widgetState, setWidgetState] = useState<boolean | undefined>();
    const { repository } = useContext(DashboardContext);

    const updatewidgetState = async () => {
        const updatedWidgetState = await repository.Configuration.WidgetState.SetWidgetState(!widgetState);
        setWidgetState(updatedWidgetState);
    };

    const cls = useStyles({ widgetState });
    useEffect(() => {
        (async () => {
            const currentWidgetState = await repository.Configuration.WidgetState.GetWidgetState();
            setWidgetState(currentWidgetState);
        })();
    }, []);

    const Switch = <IOSSwitch className="widget-state-switch" disabled={!isActive || widgetState === undefined} checked={widgetState === undefined ? true : widgetState ? true : false} onChange={updatewidgetState} name="Active" />;

    return (
        <ListItem className={classNames(cls.item)} disabled={!isActive}>
            <ListItemText>
                {menuOpen && (
                    <PalavyrText className={cls.text} variant="h6">
                        Widget
                    </PalavyrText>
                )}
            </ListItemText>
            <FormControlLabel
                control={menuOpen ? Switch : <Tooltip title="Widget On / Off toggle">{Switch}</Tooltip>}
                className={cls.text}
                label={menuOpen && (widgetState === undefined ? <PalavyrText>loading...</PalavyrText> : widgetState ? <PalavyrText>Enabled</PalavyrText> : <PalavyrText>Disabled</PalavyrText>)}
            />
        </ListItem>
    );
});
