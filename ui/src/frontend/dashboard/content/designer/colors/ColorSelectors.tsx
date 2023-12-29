import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { makeStyles } from "@material-ui/core";
import { SetState, WidgetPreferencesResource } from "@Palavyr-Types";
import { Align } from "@common/positioning/Align";
import React from "react";
import { WidgetColorOptions } from "../WidgetColorOptions";

const useStyles = makeStyles<{}>((theme: any) => ({
    colorstext: {
        paddingTop: "1.2rem",
        paddingBottom: "1.2rem",
    },
}));

export interface ColorPickerProps {
    widgetPreferences: WidgetPreferencesResource;
    setWidgetPreferences: SetState<WidgetPreferencesResource>;
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
