import { Selector, ParsedAnchor } from "./LineTypes";

export const defaultAnchor = { x: 0.5, y: 0.5 };
export const defaultBorderColor = "#f00";
export const defaultBorderStyle = "solid";
export const defaultBorderWidth = 1;

export const parseDelay = (value: string | undefined) => {
    if (typeof value === "undefined") {
        return value;
    } else if (typeof value === "boolean" && value) {
        return 0;
    }
    const delay = parseInt(value, 10);
    if (isNaN(delay) || !isFinite(delay)) {
        throw new Error(`LinkTo could not parse delay attribute "${value}"`);
    }
    return delay;
};

export const parseAnchorPercent = (value: string) => {
    const percent = parseFloat(value) / 100;
    if (isNaN(percent) || !isFinite(percent)) {
        throw new Error(`LinkTo could not parse percent value "${value}"`);
    }
    return percent;
};

export const parseAnchorText = (value: string) => {
    // Try to infer the relevant axis.
    switch (value) {
        case "top":
            return { y: 0 };
        case "left":
            return { x: 0 };
        case "middle":
            return { y: 0.5 };
        case "center":
            return { x: 0.5 };
        case "bottom":
            return { y: 1 };
        case "right":
            return { x: 1 };
    }
    return null;
};

export const parseAnchor = (value: string) => {
    if (!value) {
        return defaultAnchor;
    }
    const parts = value.split(" ");
    if (parts.length > 2) {
        throw new Error('LinkTo anchor format is "<x> <y>"');
    }
    const [x, y] = parts;
    return Object.assign({}, defaultAnchor, x ? parseAnchorText(x) || { x: parseAnchorPercent(x) } : {}, y ? parseAnchorText(y) || { y: parseAnchorPercent(y) } : {});
};

export const findElementById = (id: string) => {
    return document.getElementById(id);
};

export const findElementByClassName = (className: string) => {
    return document.getElementsByClassName(className)[0];
};

export const getPoints = (from: Selector, to: Selector, parsedFromAnchor: ParsedAnchor, parsedToAnchor: ParsedAnchor) => {
    const a = findElementByClassName(from);
    const b = findElementByClassName(to);

    if (!a || !b) {
        return false;
    }

    const box0 = a.getBoundingClientRect();
    const box1 = b.getBoundingClientRect();

    let offsetX = window.pageXOffset;
    let offsetY = window.pageYOffset;

    const p = findElementByClassName("tree-item");
    const boxp = p.getBoundingClientRect();

    offsetX -= boxp.left + (window.pageXOffset || document.documentElement.scrollLeft) - p.scrollLeft;
    offsetY -= boxp.top + (window.pageYOffset || document.documentElement.scrollTop) - p.scrollTop;

    const x0 = box0.left + box0.width * parsedFromAnchor.x + offsetX;
    const x1 = box1.left + box1.width * parsedToAnchor.x + offsetX;
    const y0 = box0.top + box0.height * parsedFromAnchor.y + offsetY;
    const y1 = box1.top + box1.height * parsedToAnchor.y + offsetY;
    return { x0, y0, x1, y1 };
};
