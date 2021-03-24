import React from "react";
import { LineStyles } from "./LineTypes";
import { defaultBorderColor, defaultBorderStyle, defaultBorderWidth } from "./LineUtils";

export type LineProps = {
    x0: number;
    x1: number;
    y0: number;
    y1: number;
    borderColor?: string;
    borderStyle?: LineStyles;
    borderWidth?: number;
    zIndex?: number;
};

type PositionStyles = {
    position: "absolute";
    top: string;
    left: string;
    width: string;
    zIndex: number;
    transform: string;
    transformOrigin: string;
};

export const Line = ({ x0, x1, y0, y1, borderColor, borderStyle, borderWidth, zIndex }: LineProps): React.ReactElement | null => {
    const dy = y1 - y0;
    const dx = x1 - x0;

    const angle = (Math.atan2(dy, dx) * 180) / Math.PI;
    const length = Math.sqrt(dx * dx + dy * dy);

    const z = zIndex ? Number(zIndex) : 1;

    const positionStyle: PositionStyles = {
        position: "absolute",
        top: `${y0}px`,
        left: `${x0}px`,
        width: `${length}px`,
        zIndex: z,
        transform: `rotate(${angle}deg)`,
        transformOrigin: "0 0",
    };

    const defaultStyle = {
        borderTopColor: borderColor ?? defaultBorderColor,
        borderTopStyle: borderStyle ?? defaultBorderStyle,
        borderTopWidth: borderWidth ?? defaultBorderWidth,
    };
    return (
        <div style={{ ...defaultStyle, ...positionStyle }}></div>
    );
};
