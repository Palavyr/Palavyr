import { googleFontApikey } from "@api-client/clientUtils";
import { FontManager, OPTIONS_DEFAULTS } from "@common/fonts/fontManager";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";

export const InitializeFonts = (widgetPreferences: WidgetPreferencesResource) => {
    const fontManager = getFontManager(widgetPreferences);
    fontManager.setActiveFont(widgetPreferences.fontFamily);
};

export const getFontManager = (widgetPreferences: WidgetPreferencesResource) => {
    const fontManager = new FontManager(googleFontApikey, widgetPreferences.fontFamily, OPTIONS_DEFAULTS);
    fontManager.init();
    return fontManager;
};
