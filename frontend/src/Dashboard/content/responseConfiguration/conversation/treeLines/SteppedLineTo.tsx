import { Anchor, Selector, LineStyles } from "./LineTypes";
import { parseAnchor, getPoints, defaultBorderWidth } from "./LineUtils";
import { LineProps, Line } from "./Line";
import React from "react";
import { uuid } from "uuidv4";


export type SteppedLineToProps = {
    fromAnchor: Anchor;
    toAnchor: Anchor;
    from: Selector;
    to: Selector;
    borderColor?: string;
    borderStyle?: LineStyles;
    borderWidth?: number;
    className?: string;
    zIndex?: number;
    orientation: "h" | "v";
};

export const SteppedLineTo: React.FC<SteppedLineToProps> = ({ fromAnchor, toAnchor, from, to, borderColor, borderStyle, borderWidth, className, zIndex, orientation }: SteppedLineToProps) => {
    const parsedFromAnchor = parseAnchor(fromAnchor);
    const parsedToAnchor = parseAnchor(toAnchor);

    const points = getPoints(from, to, parsedFromAnchor, parsedToAnchor);
    if (!points) return null;
    if (orientation === "h") {
        return renderHorizontal({ ...{ ...points, ...{ zIndex, borderColor, borderStyle, borderWidth, className } } });
    }
    return renderVertical({ ...{ ...points, ...{ zIndex, borderColor, borderStyle, borderWidth, className } } });
};

const renderVertical: React.FC<LineProps> = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth, className }: LineProps) => {
    const dx = x1 - x0;
    if (dx === 0) {
        return <Line {...{ zIndex, borderColor, borderStyle, borderWidth, className }} x0={x0} y0={y0} x1={x1} y1={y1} />;
    }

    const bWidth = borderWidth || defaultBorderWidth;
    const y2 = (y0 + y1) / 2;

    const xOffset = dx > 0 ? bWidth : 0;
    const minX = Math.min(x0, x1) - xOffset;
    const maxX = Math.max(x0, x1);

    return (
        <div className="react-steppedlineto">
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={x0} y0={y0} x1={x0} y1={y2} />
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={x1} y0={y1} x1={x1} y1={y2} />
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={minX + 1} y0={y2} x1={maxX + 1} y1={y2} />
        </div>
    );
};

const renderHorizontal: React.FC<LineProps> = ({ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth, className }: LineProps) => {
    const dy = y1 - y0;
    if (dy === 0) {
        return <Line {...{ x0, y0, x1, y1, zIndex, borderColor, borderStyle, borderWidth, className }} />;
    }

    const bWidth = borderWidth || defaultBorderWidth;
    const x2 = (x0 + x1) / 2;

    const yOffset = dy < 0 ? bWidth : 0;
    const minY = Math.min(y0, y1) - yOffset;
    const maxY = Math.max(y0, y1);

    return (
        <div className="react-steppedlineto">
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={x0} y0={y0} x1={x2} y1={y0} />
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={x1} y0={y1} x1={x2} y1={y1} />
            <Line key={uuid()} {...{ zIndex, borderColor, borderStyle, bWidth, className }} x0={x2} y0={minY} x1={x2} y1={maxY} />
        </div>
    );
};
