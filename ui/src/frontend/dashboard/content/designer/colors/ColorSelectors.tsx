import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { makeStyles } from "@material-ui/core";
import { SetState, WidgetPreferences } from "@Palavyr-Types";
import { Align } from "frontend/dashboard/layouts/positioning/Align";
import React from "react";
import { WidgetColorOptions } from "../ColorOptions";

const useStyles = makeStyles(theme => ({
    colorstext: {
        paddingTop: "1.2rem",
        paddingBottom: "1.2rem",
    },
}));

export interface ColorPickerProps {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}
export const ColorSelectors = ({ widgetPreferences, setWidgetPreferences }: ColorPickerProps) => {
    const cls = useStyles();
    return (
        <>
            <Align>
                <PalavyrText className={cls.colorstext} variant="h4">
                    Select your widget colors
                </PalavyrText>
            </Align>
            <WidgetColorOptions widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
        </>
    );
};
