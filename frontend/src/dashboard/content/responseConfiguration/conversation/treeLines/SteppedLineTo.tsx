import { Anchor, Selector, LineStyles } from "./LineTypes";
import { parseAnchor, getPoints, defaultBorderWidth } from "./LineUtils";
import { LineProps, Line } from "./Line";
import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { debounce } from "lodash";

export type SteppedLineToProps = {
    fromAnchor: Anchor;
    toAnchor: Anchor;
    from: Selector;
    to: Selector;
    borderColor?: string;
    borderStyle?: LineStyles;
    borderWidth?: number;
    zIndex?: number;
    orientation: "h" | "v";
};

export const SteppedLineTo: React.FC<SteppedLineToProps> = ({ fromAnchor, toAnchor, from, to, borderColor, borderStyle, borderWidth, zIndex, orientation }: SteppedLineToProps) => {
    const [sized, setSized] = useState<boolean>(false);

    const handle = () => setSized(!sized)

    useEffect(() => {
        window.addEventListener("resize", debounce(handle, 10));
        return () => window.removeEventListener("resize", debounce(handle, 10));
    }, [sized]);

    const parsedFromAnchor = parseAnchor(fromAnchor);
    const parsedToAnchor = parseAnchor(toAnchor);

    const points = getPoints(from, to, parsedFromAnchor, parsedToAnchor);
    if (!points) return null;
    if (orientation === "h") {
        return renderHorizontal({ ...{ ...points, ...{ zIndex, borderColor, borderStyle, borderWidth } } });
    }
    return renderVertical({ ...{ ...points, ...{ zIndex, borderColor, borderStyle, borderWidth } } });
};

const renderVertical = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth }: LineProps) => {
    const dx = x1 - x0;
    if (dx === 0) {
        return <Line {...{ zIndex, borderColor, borderStyle, borderWidth }} x0={x0} y0={y0} x1={x1} y1={y1} />;
    }

    const bWidth = borderWidth || defaultBorderWidth;
    const y2 = (y0 + y1) / 2;

    const xOffset = dx > 0 ? bWidth : 0;
    const minX = Math.min(x0, x1) - xOffset;
    const maxX = Math.max(x0, x1);

    return (
        <div className="react-steppedlineto">
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={x0} y0={y0} x1={x0} y1={y2} />
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={x1} y0={y1} x1={x1} y1={y2} />
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={minX + 1} y0={y2} x1={maxX + 1} y1={y2} />
        </div>
    );
};

const renderHorizontal = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth }: LineProps) => {
    const dy = y1 - y0;
    if (dy === 0) {
        return <Line {...{ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth }} />;
    }

    const bWidth = borderWidth || defaultBorderWidth;
    const x2 = (x0 + x1) / 2;

    const yOffset = dy < 0 ? bWidth : 0;
    const minY = Math.min(y0, y1) - yOffset;
    const maxY = Math.max(y0, y1);

    return (
        <div className="react-steppedlineto">
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={x0} y0={y0} x1={x2} y1={y0} />
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={x1} y0={y1} x1={x2} y1={y1} />
            <Line {...{ zIndex, borderColor, borderStyle, bWidth }} x0={x2} y0={minY} x1={x2} y1={maxY} />
        </div>
    );
};
