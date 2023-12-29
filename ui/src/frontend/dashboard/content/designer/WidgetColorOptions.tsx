import React from "react";
import { Hidden, makeStyles } from "@material-ui/core";
import { SetState, WidgetPreferencesResource } from "@Palavyr-Types";
import { SpaceEvenly } from "@common/positioning/SpaceEvenly";
import { SketchPicker } from "react-color";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { LineSpacer } from "@common/components/typography/LineSpacer";

export type ColorPickerType = {
    method: SetState<string>;
    name?: string;
    variable?: string;
    disable?: boolean;
};

export interface WidgetColorOptionsProps {
    widgetPreferences: WidgetPreferencesResource;
    setWidgetPreferences: SetState<WidgetPreferencesResource>;
}

export const widgetStyles = makeStyles<{}>(theme => ({
    gridList: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        paddingBottom: "4rem",
        paddingLeft: "8%",
        paddingRight: "8%",
    },

    pickerTitle: {
        marginBottom: "0.3rem",
    },
}));

export const WidgetColorOptions = ({ widgetPreferences, setWidgetPreferences }: WidgetColorOptionsProps) => {
    const cls = widgetStyles();

    const rowOne = (widgetPreferences: WidgetPreferencesResource): ColorPickerType[] => {
        return [
            { name: "Header Color", variable: widgetPreferences.headerColor, method: (headerColor: string) => setWidgetPreferences({ ...widgetPreferences, headerColor }), disable: true },
            { name: "Header Font Color", variable: widgetPreferences.headerFontColor, method: (headerFontColor: string) => setWidgetPreferences({ ...widgetPreferences, headerFontColor }), disable: true },
            { name: "Options List Color", variable: widgetPreferences.selectListColor, method: (selectListColor: string) => setWidgetPreferences({ ...widgetPreferences, selectListColor }), disable: true },
            { name: "Options List Font Color", variable: widgetPreferences.listFontColor, method: (listFontColor: string) => setWidgetPreferences({ ...widgetPreferences, listFontColor }), disable: true },
        ];
    };

    const rowTwo = (widgetPreferences: WidgetPreferencesResource): ColorPickerType[] => {
        return [
            { name: "Background Color", variable: widgetPreferences.chatBubbleColor, method: (chatBubbleColor: string) => setWidgetPreferences({ ...widgetPreferences, chatBubbleColor }), disable: true },
            { name: "Text Color", variable: widgetPreferences.chatFontColor, method: (chatFontColor: string) => setWidgetPreferences({ ...widgetPreferences, chatFontColor }), disable: true },
            { name: "Button Color", variable: widgetPreferences.buttonColor, method: (buttonColor: string) => setWidgetPreferences({ ...widgetPreferences, buttonColor }), disable: true },
            { name: "Button Font Color", variable: widgetPreferences.buttonFontColor, method: (buttonFontColor: string) => setWidgetPreferences({ ...widgetPreferences, buttonFontColor }), disable: true },
        ];
    };

    const colors = ["#000000", "#264653", "#2A9D8F", "#E9C46A", "#F4A261", "#E9C46B", "#118AB2", "#83C5BE", "#FFE8D6", "#FFDDD2", "#E29578", "#006D77", "#FFFFFF", "#F1FAEE", "#EDF6F9", "#FDFFB6"];
    return (
        <>
            <div className={cls.gridList}>
                {widgetPreferences &&
                    rowOne(widgetPreferences).map((picker: ColorPickerType, index: number) => {
                        return (
                            <div key={`${picker.variable}-${index}`}>
                                <PalavyrText align="center" variant="body1" className={cls.pickerTitle} gutterBottom>
                                    {picker.name}
                                </PalavyrText>
                                <LineSpacer />
                                <SpaceEvenly>
                                    {picker.variable && (
                                        <>
                                            <Hidden lgUp>
                                                <SketchPicker disableAlpha presetColors={colors} color={picker.variable} width={"130px"} onChangeComplete={({ hex }) => picker.method(hex)} />
                                            </Hidden>
                                            <Hidden mdDown>
                                                <SketchPicker disableAlpha presetColors={colors} color={picker.variable} width={"170px"} onChangeComplete={({ hex }) => picker.method(hex)} />
                                            </Hidden>
                                        </>
                                    )}
                                </SpaceEvenly>
                            </div>
                        );
                    })}
            </div>
            <div className={cls.gridList}>
                {widgetPreferences &&
                    rowTwo(widgetPreferences).map((picker: ColorPickerType, index: number) => {
                        return (
                            <div key={`${index}-${picker.variable}`}>
                                <PalavyrText align="center" variant="body1" className={cls.pickerTitle} gutterBottom>
                                    {picker.name}
                                </PalavyrText>
                                <LineSpacer />
                                <SpaceEvenly>
                                    {picker.variable && (
                                        <>
                                            <Hidden lgUp>
                                                <SketchPicker disableAlpha presetColors={colors} color={picker.variable} width={"130px"} onChangeComplete={({ hex }) => picker.method(hex)} />
                                            </Hidden>
                                            <Hidden mdDown>
                                                <SketchPicker disableAlpha presetColors={colors} color={picker.variable} width={"170px"} onChangeComplete={({ hex }) => picker.method(hex)} />
                                            </Hidden>
                                        </>
                                    )}
                                </SpaceEvenly>
                            </div>
                        );
                    })}
            </div>
        </>
    );
};
