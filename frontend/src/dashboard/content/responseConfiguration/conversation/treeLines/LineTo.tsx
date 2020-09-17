import { Anchor, Selector, LineStyles } from "./LineTypes";
import { parseAnchor, getPoints } from "./LineUtils";
import { Line } from "./Line";
import React from "react";


export type LineToProps = {
    fromAnchor: Anchor;
    toAnchor: Anchor;
    from: Selector;
    to: Selector;
    borderColor?: string;
    borderStyle?: LineStyles;
    borderWidth?: number;
    className?: string;
    zIndex?: number;
};

export const LineTo: React.FC<LineToProps> = ({ fromAnchor, toAnchor, from, to, borderColor, borderStyle, borderWidth, className, zIndex }: LineToProps) => {
    const parsedFromAnchor = parseAnchor(fromAnchor);
    const parsedToAnchor = parseAnchor(toAnchor);

    const points = getPoints(from, to, parsedFromAnchor, parsedToAnchor);
    return points ? <Line {...{ ...points, ...{ zIndex, borderColor, borderStyle, borderWidth, className } }} /> : null;
};
