import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { SUPPORTED_FONTS } from "@constants";
import { SetState, WidgetPreferences } from "@Palavyr-Types";
import { CustomSelect } from "frontend/dashboard/content/responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { Align } from "@common/positioning/Align";
import React from "react";

export interface FontSelector {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}

export const FontSelector = ({ widgetPreferences, setWidgetPreferences }: FontSelector) => {
    return (
        <div>
            <Align>
                <PalavyrText align="center" gutterBottom variant="h4">
                    Widget Font
                </PalavyrText>
            </Align>
            <Align>
                {widgetPreferences && (
                    <CustomSelect
                        styles={{ borderRadius: "20px" }}
                        option={widgetPreferences.fontFamily}
                        options={SUPPORTED_FONTS}
                        width="50%"
                        align="left"
                        onChange={event => {
                            const newFont = event.target.value as string;
                            setWidgetPreferences({ ...widgetPreferences, fontFamily: newFont });
                        }}
                    />
                )}
            </Align>
        </div>
    );
};
