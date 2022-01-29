import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { SUPPORTED_FONTS } from "@constants";
import { SetState, WidgetPreferences } from "@Palavyr-Types";
import { CustomSelect } from "frontend/dashboard/content/responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { Align } from "@common/positioning/Align";
import React, { useEffect, useState } from "react";
import { googleFontApikey } from "@api-client/clientUtils";
import FontPicker from "@common/fonts/FontPicker";
import { cloneDeep } from "lodash";
import { uuid } from "uuidv4";
import { DEFAULT_FONT, FontManager, Options, OPTIONS_DEFAULTS, Variant } from "@common/fonts/fontManager";

export interface FontSelector {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}

export const FontSelector = ({ widgetPreferences, setWidgetPreferences }: FontSelector) => {
    const [font, setFont] = useState<string>("");
    const [options, setOptions] = useState<Options>(OPTIONS_DEFAULTS);
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            const fontManager = new FontManager(googleFontApikey, DEFAULT_FONT, OPTIONS_DEFAULTS);
            fontManager.init();
            fontManager.setActiveFont(widgetPreferences.fontFamily);
            const activeFont = fontManager.getActiveFont();
            setOptions(options);
            if (widgetPreferences.fontFamily === "Architects Daughter") {
                setFont(fontManager.getFonts()[0]);
            } else {
                setFont(activeFont.family);
            }
            setLoaded(true);
        })();
    }, []);

    return (
        <div>
            <Align>
                <PalavyrText align="center" gutterBottom variant="h4">
                    Widget Font
                </PalavyrText>
            </Align>
            <Align>
                {widgetPreferences && loaded && (
                    <FontPicker
                        apiKey={googleFontApikey}
                        activeFontFamily={font}
                        onChange={nextFont => {
                            widgetPreferences.fontFamily = nextFont.family;
                            setWidgetPreferences(cloneDeep(widgetPreferences));
                            setFont(nextFont.family);
                        }}
                        {...options}
                    />
                )}
            </Align>
        </div>
    );
};
