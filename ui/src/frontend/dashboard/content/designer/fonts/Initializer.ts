import { googleFontApikey } from "@api-client/clientUtils";
import { FontManager, OPTIONS_DEFAULTS } from "@common/fonts/fontManager";
import { WidgetPreferences } from "@Palavyr-Types";

export const InitializeFonts = (widgetPreferences: WidgetPreferences) => {
    const fontManager = getFontManager(widgetPreferences);
    fontManager.setActiveFont(widgetPreferences.fontFamily);
};

export const getFontManager = (widgetPreferences: WidgetPreferences) => {
    const fontManager = new FontManager(googleFontApikey, widgetPreferences.fontFamily, OPTIONS_DEFAULTS);
    fontManager.init();
    return fontManager;
};
