import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { SetState, WidgetPreferencesResource } from "@Palavyr-Types";
import { Align } from "@common/positioning/Align";
import React, { useEffect, useState } from "react";
import { googleFontApikey } from "@api-client/clientUtils";
import FontPicker from "@common/fonts/FontPicker";
import { cloneDeep } from "lodash";
import { Font, Options, OPTIONS_DEFAULTS } from "@common/fonts/fontManager";
import { makeStyles } from "@material-ui/core";
import { getFontManager } from "./Initializer";

export interface FontSelector {
    widgetPreferences: WidgetPreferencesResource;
    setWidgetPreferences: SetState<WidgetPreferencesResource>;
}

const useStyles = makeStyles(theme => ({
    fontPicker: {
        width: "50ch",
    },
}));

export const FontSelector = ({ widgetPreferences, setWidgetPreferences }: FontSelector) => {
    const [font, setFont] = useState<string>("");
    const [options, setOptions] = useState<Options>(OPTIONS_DEFAULTS);
    const [loaded, setLoaded] = useState<boolean>(false);
    const cls = useStyles();

    useEffect(() => {
        (async () => {
            setOptions(options);
            if (widgetPreferences.fontFamily === "Architects Daughter") {
                setFont("Poppins");
            } else {
                setFont(widgetPreferences.fontFamily);
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
                        className={cls.fontPicker}
                        onChange={(event: any, nextFont: Font) => {
                            widgetPreferences.fontFamily = nextFont.family;
                            const fontManager = getFontManager(widgetPreferences);
                            fontManager.setActiveFont(nextFont.family);
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
