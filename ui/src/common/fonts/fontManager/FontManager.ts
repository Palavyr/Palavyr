import "./picker-styles/styles.scss";

import getFontList from "./google-fonts/fontList";
import { loadActiveFont, loadFontPreviews } from "./loadFonts";
import { Font, DEFAULT_FONT_FAMILY, FontList, Options, OPTIONS_DEFAULTS } from "./types";
import { getFontId, validatePickerId } from "./utils/ids";
import { Session } from "inspector";
import { SessionStorage } from "@frontend/localStorage/sessionStorage";

/**
 * Class for managing the list of fonts for the font picker, keeping track of the active font and
 * downloading/activating Google Fonts
 */
export default class FontManager {
    // Parameters

    private readonly apiKey: string;

    private readonly options: Options;

    private onChange: (event: any, font: Font) => void;

    // Other class variables

    // Name of currently applied font
    private activeFontFamily: string;

    // Map from font families to font objects
    private fonts: FontList = new Map<string, Font>();

    // Suffix appended to CSS selectors which would have name clashes if multiple font pickers are
    // used on the same site (e.g. "-test" if the picker has pickerId "test" or "" if the picker
    // doesn't have an ID)
    public selectorSuffix: string;

    /**
     * Save relevant options, download the default font, add it to the font list and apply it
     */
    constructor(
        apiKey: string,
        defaultFamily: string = DEFAULT_FONT_FAMILY,
        {
            pickerId = OPTIONS_DEFAULTS.pickerId,
            families = OPTIONS_DEFAULTS.families,
            categories = OPTIONS_DEFAULTS.categories,
            scripts = OPTIONS_DEFAULTS.scripts,
            variants = OPTIONS_DEFAULTS.variants,
            filter = OPTIONS_DEFAULTS.filter,
            limit = OPTIONS_DEFAULTS.limit,
            sort = OPTIONS_DEFAULTS.sort,
        }: Options,
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        onChange: (event: any, font: Font) => void = (event, font) => {}
    ) {
        // Validate pickerId parameter
        validatePickerId(pickerId);
        this.selectorSuffix = pickerId ? `-${pickerId}` : "";

        // Save parameters as class variables
        this.apiKey = apiKey;
        this.options = {
            pickerId,
            families,
            categories,
            scripts,
            variants,
            filter,
            limit,
            sort,
        };
        this.onChange = onChange;

        // Download default font and add it to the empty font list
        this.addFont(defaultFamily, false);
        this.setActiveFont(defaultFamily, false);
    }

    /**
     * Fetch list of all fonts from Google Fonts API, filter it according to the class parameters and
     * save them to the font map
     */
    public async init(): Promise<FontList> {
        // Get list of all fonts

        let fonts = SessionStorage.getFonts();
        if (!fonts) {
            fonts = await getFontList(this.apiKey);
            SessionStorage.setFonts(fonts);
        }

        // Save desired fonts in the font map
        for (let i = 0; i < fonts.length; i += 1) {
            const font = fonts[i];
            // Exit once specified limit of number of fonts is reached
            if (this.fonts.size >= this.options.limit) {
                break;
            }

            let canInclude = true;
            if (this.fonts.has(font.family)) {
                canInclude = false;
            }

            if (this.options.families.length > 0) {
                if (!this.options.families.includes(font.family)) {
                    canInclude = false;
                }
            }

            if (this.options.categories.length > 0) {
                if (!this.options.categories.includes(font.category)) {
                    canInclude = false;
                }
            }

            if (!this.options.scripts.every((script): boolean => font.scripts.includes(script))) {
                canInclude = false;
            }

            if (!this.options.variants.every((variant): boolean => font.variants.includes(variant))) {
                canInclude = false;
            }

            if (this.options.filter(font) === false) {
                canInclude = false;
            }

            if (canInclude) {
                this.fonts.set(font.family, font);
            } else {
                // console.log("Skipping: " + font.family);
            }
        }
        // Download previews for all fonts in list except for default font (its full font has already
        // been downloaded)
        const fontsToLoad = new Map(this.fonts);
        fontsToLoad.delete(this.activeFontFamily);
        loadFontPreviews(fontsToLoad, this.options.scripts, this.options.variants, this.selectorSuffix);

        return this.fonts;
    }

    /**
     * Return font map
     */
    public getFonts(): FontList {
        return this.fonts;
    }

    /**
     * Add a new font to the font map and download its preview characters
     */
    public addFont(fontFamily: string, downloadPreview = true): void {
        // @ts-ignore: Custom font does not need `categories`, `scripts` and `variants` attributes
        const font: Font = {
            family: fontFamily,
            id: getFontId(fontFamily),
        };
        this.fonts.set(fontFamily, font);

        // Download font preview unless specified not to
        if (downloadPreview) {
            const fontMap: FontList = new Map<string, Font>();
            fontMap.set(fontFamily, font);
            loadFontPreviews(fontMap, this.options.scripts, this.options.variants, this.selectorSuffix);
        }
    }

    /**
     * Remove the specified font from the font map
     */
    public removeFont(fontFamily: string): void {
        this.fonts.delete(fontFamily);
    }

    /**
     * Return the font object of the currently active font
     */
    public getActiveFont(): Font {
        const activeFont = this.fonts.get(this.activeFontFamily);
        if (!activeFont) {
            throw Error(`Cannot get active font: "${this.activeFontFamily}" is not in the font list`);
        } else {
            return activeFont;
        }
    }

    /**
     * Set the specified font as the active font and download it
     */
    public setActiveFont(fontFamily: string, runOnChange = true): void {
        const previousFontFamily = this.activeFontFamily;
        const activeFont = this.fonts.get(fontFamily);
        if (!activeFont) {
            // Font is not in fontList: Keep current activeFont and log error
            throw Error(`Darn! Cannot update active font: "${fontFamily}" is not in the font list`);
        }

        this.activeFontFamily = fontFamily;
        loadActiveFont(activeFont, previousFontFamily, this.options.scripts, this.options.variants, this.selectorSuffix).then((): void => {
            if (runOnChange) {
                this.onChange("", activeFont);
            }
        });
    }

    /**
     * Update the onChange function (executed when changing the active font)
     */
    public setOnChange(onChange: (event: any, font: Font) => void): void {
        this.onChange = onChange;
    }
}
