import React from "react";
import { LineStyles } from "./LineTypes";
import { defaultBorderColor, defaultBorderStyle, defaultBorderWidth } from "./LineUtils";
import { uuid } from "uuidv4";


export type LineProps = {
    x0: number;
    x1: number;
    y0: number;
    y1: number;
    borderColor?: string;
    borderStyle?: LineStyles;
    borderWidth?: number;
    zIndex?: number;
    className?: string;
};

type positionStyles = {
    position: "absolute";
    top: string;
    left: string;
    width: string;
    zIndex: number;
    transform: string;
    transformOrigin: string;
};

export const Line = ({ x0, x1, y0, y1, borderColor, borderStyle, borderWidth, zIndex, className }: LineProps): React.ReactElement | null => {

    const dy = y1 - y0;
    const dx = x1 - x0;

    const angle = (Math.atan2(dy, dx) * 180) / Math.PI;
    const length = Math.sqrt(dx * dx + dy * dy);

    const z = zIndex ? Number(zIndex) : 1;

    const positionStyle: positionStyles = {
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
        <div key={uuid()} className="react-lineto-placeholder">
            <div
                key={uuid()}
                className={className ?? ""}
                style={{ ...defaultStyle, ...positionStyle }}
            ></div>
        </div>
    )
};
