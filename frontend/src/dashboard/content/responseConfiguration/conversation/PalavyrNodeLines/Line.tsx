import { makeStyles } from "@material-ui/core";
import { LineStyles } from "@Palavyr-Types";
import React from "react";

const defaultBorderColor = "#f00";
const defaultBorderStyle = "solid";
const defaultBorderWidth = 1;

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
    top: string;
    left: string;
    width: string;
    zIndex: number;
    transform: string;
    transformOrigin: string;
};

const useStyles = makeStyles((theme) => ({
    line: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        position: "absolute",
    },
}));

export const Line = ({ x0, x1, y0, y1, borderColor, borderStyle, borderWidth, zIndex }: LineProps): React.ReactElement | null => {
    const cls = useStyles();

    const dy = y1 - y0;
    const dx = x1 - x0;

    const angle = (Math.atan2(dy, dx) * 180) / Math.PI;
    const length = Math.sqrt(dx * dx + dy * dy);

    const z = zIndex ? Number(zIndex) : 1;

    const positionStyle: PositionStyles = {
        top: `${y0}px`,
        left: `${x0}px`,
        width: `${length}px`,
        zIndex: z,
        transform: `rotate(${angle}deg)`,
        transformOrigin: "0px 0px",
    };

    const defaultStyle = {
        borderTopColor: borderColor ?? defaultBorderColor,
        borderTopStyle: borderStyle ?? defaultBorderStyle,
        borderTopWidth: borderWidth ?? defaultBorderWidth,
    };

    return <div className={cls.line} style={{ ...defaultStyle, ...positionStyle }}></div>;
};
