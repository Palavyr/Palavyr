export const takeNCharacters = (text?: string, n: number = 12): string => {
    if (!text) return "";

    if (text.length <= n) {
        return text;
    }
    return text.slice(0, n) + "...";
};
