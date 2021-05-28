import React from "react";
import { makeStyles, Typography } from "@material-ui/core";
import { SetState, WidgetPreferences } from "@Palavyr-Types";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import { ChromePicker } from "react-color";

export type ColorPickerType = {
    method: SetState<string>;
    name: string;
    variable: string;
    disable: boolean;
};

export interface WidgetColorOptionsProps {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}

export const useStyles = makeStyles((theme) => ({
    gridList: {
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        paddingBottom: "4rem",
        paddingLeft: "10%",
        paddingRight: "10%",
    },

    pickerTitle: {
        marginBottom: "0.3rem",
    },
}));

export const WidgetColorOptions = ({ widgetPreferences, setWidgetPreferences }: WidgetColorOptionsProps) => {
    const cls = useStyles();

    const rowOne = (widgetPreferences: WidgetPreferences): ColorPickerType[] => {
        return [
            { name: "Header Color", variable: widgetPreferences.headerColor, method: (headerColor: string) => setWidgetPreferences({ ...widgetPreferences, headerColor }), disable: true },
            { name: "Header Font Color", variable: widgetPreferences.headerFontColor, method: (headerFontColor: string) => setWidgetPreferences({ ...widgetPreferences, headerFontColor }), disable: true },
            { name: "Options List Color", variable: widgetPreferences.selectListColor, method: (selectListColor: string) => setWidgetPreferences({ ...widgetPreferences, selectListColor }), disable: true },
            { name: "Options List Font Color", variable: widgetPreferences.listFontColor, method: (listFontColor: string) => setWidgetPreferences({ ...widgetPreferences, listFontColor }), disable: true },
        ];
    };

    const rowTwo = (widgetPreferences: WidgetPreferences): ColorPickerType[] => {
        return [
            { name: "Chat Bubble Color", variable: widgetPreferences.chatBubbleColor, method: (chatBubbleColor: string) => setWidgetPreferences({ ...widgetPreferences, chatBubbleColor }), disable: true },
            { name: "Chat Bubble Font Color", variable: widgetPreferences.chatFontColor, method: (chatFontColor: string) => setWidgetPreferences({ ...widgetPreferences, chatFontColor }), disable: true },
            { name: "Chat Button Color", variable: widgetPreferences.buttonColor, method: (buttonColor: string) => setWidgetPreferences({ ...widgetPreferences, buttonColor }), disable: true },
            { name: "Chat Button Font Color", variable: widgetPreferences.buttonFontColor, method: (buttonFontColor: string) => setWidgetPreferences({ ...widgetPreferences, buttonFontColor }), disable: true },
        ];
    };

    return (
        <>
            <div className={cls.gridList}>
                {widgetPreferences &&
                    rowOne(widgetPreferences).map((picker: ColorPickerType, index: number) => {
                        return (
                            <div>
                                <Typography align="center" variant="body1" className={cls.pickerTitle} gutterBottom>
                                    {picker.name}
                                </Typography>
                                <SpaceEvenly>{picker.variable && <ChromePicker disableAlpha color={picker.variable} onChangeComplete={(color: { hex: React.SetStateAction<string> }) => picker.method(color.hex)} />}</SpaceEvenly>
                            </div>
                        );
                    })}
            </div>
            <div className={cls.gridList}>
                {widgetPreferences &&
                    rowTwo(widgetPreferences).map((picker: ColorPickerType, index: number) => {
                        return (
                            <div>
                                <Typography align="center" variant="body1" className={cls.pickerTitle} gutterBottom>
                                    {picker.name}
                                </Typography>
                                <SpaceEvenly>{picker.variable && <ChromePicker disableAlpha color={picker.variable} onChangeComplete={(color: { hex: React.SetStateAction<string> }) => picker.method(color.hex)} />}</SpaceEvenly>
                            </div>
                        );
                    })}
            </div>
        </>
    );
};
