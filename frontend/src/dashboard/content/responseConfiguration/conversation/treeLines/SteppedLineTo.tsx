import React from "react";
import { Anchor, Selector, LineStyles, ParsedAnchor } from "./LineTypes";
import { LineProps, Line } from "./Line";
import { useEffect } from "react";
import { useState } from "react";
import { debounce } from "lodash";

const defaultBorderWidth = 1;

export interface SteppedLineToProps {
    from: Selector;
    to: Selector;
    treeLinkClassName: string;
    fromAnchor?: Anchor;
    toAnchor?: Anchor;
    borderColor?: string;
    borderStyle?: LineStyles;
    zIndex?: number;
    orientation?: "h" | "v";
};

export const SteppedLineTo: React.FC<SteppedLineToProps> = ({
    from,
    to,
    treeLinkClassName,
    fromAnchor = "top",
    toAnchor = "bottom",
    borderColor = "#54585A",
    borderStyle = "double",
    zIndex = 0,
    orientation = "v",
}: SteppedLineToProps) => {
    const [sized, setSized] = useState<boolean>(false);
    const handle = () => setSized(!sized);
    useEffect(() => {
        window.addEventListener("resize", debounce(handle, 10));
        return () => window.removeEventListener("resize", debounce(handle, 10));
    }, [sized]);

    const parsedFromAnchor = parseAnchor(fromAnchor);
    const parsedToAnchor = parseAnchor(toAnchor);

    const points = getPoints(from, to, treeLinkClassName, parsedFromAnchor, parsedToAnchor);
    if (!points) return null;
    if (orientation === "h") {
        return renderHorizontal({ ...{ ...points, ...{ zIndex, borderColor, borderStyle } } });
    }
    return renderVertical({ ...{ ...points, ...{ zIndex, borderColor, borderStyle } } });
};

const renderVertical = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle }: LineProps) => {
    const dx = x1 - x0;
    if (dx === 0) {
        return <Line {...{ zIndex, borderColor, borderStyle, borderWidth: defaultBorderWidth }} x0={x0} y0={y0} x1={x1} y1={y1} />;
    }

    const y2 = (y0 + y1) / 2;

    const xOffset = dx > 0 ? defaultBorderWidth : 0;
    const minX = Math.min(x0, x1) - xOffset;
    const maxX = Math.max(x0, x1);

    return (
        <div>
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={x0} y0={y0} x1={x0} y1={y2} />
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={x1} y0={y1} x1={x1} y1={y2} />
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={minX + 1} y0={y2} x1={maxX + 1} y1={y2} />
        </div>
    );
};

const renderHorizontal = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle }: LineProps) => {
    const dy = y1 - y0;
    if (dy === 0) {
        return <Line {...{ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth: defaultBorderWidth }} />;
    }

    const x2 = (x0 + x1) / 2;

    const yOffset = dy < 0 ? defaultBorderWidth : 0;
    const minY = Math.min(y0, y1) - yOffset;
    const maxY = Math.max(y0, y1);

    return (
        <div>
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={x0} y0={y0} x1={x2} y1={y0} />
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={x1} y0={y1} x1={x2} y1={y1} />
            <Line {...{ zIndex, borderColor, borderStyle, defaultBorderWidth }} x0={x2} y0={minY} x1={x2} y1={maxY} />
        </div>
    );
};

export const parseAnchor = (value: string) => {
    switch (value) {
        case "top":
            return { x: 0.5, y: 0 };
        case "left":
            return { x: 0, y: 0.5 };
        case "middle":
            return { x: 0.5, y: 0.5 };
        case "center":
            return { x: 0.5, y: 0.5 };
        case "bottom":
            return { x: 0.5, y: 1 };
        case "right":
            return { x: 1, y: 0.5 };
        default:
            throw new Error("Must provide compatible anchor position.");
    }
};

export const findElementById = (id: string): HTMLElement => {
    const element = document.getElementById(id);
    if (!element) throw new Error(`Could not find the element by id: ${id}`);
    return element;
};

export const findElementByClassName = (className: string) => {
    const elements = document.getElementsByClassName(className); // TODO: would be better if we could deliberately grab a specific element by id and use it instead
    return elements[0];
};

export const getPoints = (from: Selector, to: Selector, treeLinkClassName: string, parsedFromAnchor: ParsedAnchor, parsedToAnchor: ParsedAnchor) => {
    const fromElement = findElementById(from);
    const toElement = findElementById(to);

    if (!fromElement || !toElement) {
        throw new Error("Could not find either 'from' element or 'to' Element");
    }

    const fromElementBox = fromElement.getBoundingClientRect();
    const toElementBox = toElement.getBoundingClientRect();

    let offsetX = window.pageXOffset;
    let offsetY = window.pageYOffset;

    const treeItem = findElementByClassName(treeLinkClassName);
    const treeItemBox = treeItem.getBoundingClientRect();

    offsetX -= treeItemBox.left + (window.pageXOffset || document.documentElement.scrollLeft) - treeItem.scrollLeft;
    offsetY -= treeItemBox.top + (window.pageYOffset || document.documentElement.scrollTop) - treeItem.scrollTop;

    const x0 = fromElementBox.left + fromElementBox.width * parsedFromAnchor.x + offsetX;
    const x1 = toElementBox.left + toElementBox.width * parsedToAnchor.x + offsetX;
    const y0 = fromElementBox.top + fromElementBox.height * parsedFromAnchor.y + offsetY;
    const y1 = toElementBox.top + toElementBox.height * parsedToAnchor.y + offsetY;
    return { x0, y0, x1, y1 };
};
