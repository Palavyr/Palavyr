import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

type Directions = "flex-start" | "flex-end" | "center";

export interface IAlign {
    children: React.ReactNode;
    direction?: Directions;
    float?: "left" | "right";
    verticalCenter?: boolean;
    extraClassNames?: string;
    orientation?: "row" | "column";
}

export type StyleProps = {
    direction?: Directions;
    float?: "left" | "right";
    verticalCenter?: boolean;
    orientation?: "row" | "column";
};

const useStyles = makeStyles((theme) => ({
    align: (props: StyleProps) => {
        let styles = {};
        styles = {
            display: "flex",
            flexDirection: props.orientation,
            justifyContent: props.direction ?? "center",
        };
        if (props.float) {
            const float = props.float;
            styles = { ...styles, float: float };
        }

        if (props.verticalCenter) {
            styles = { ...styles, alignItems: "center" };
        }
        return styles;
    },
}));

export const Align = ({ direction, float, orientation = "row", children, verticalCenter, extraClassNames }: IAlign) => {
    const cls = useStyles({ direction, float, verticalCenter, orientation});

    return <div className={classNames(cls.align, extraClassNames)}>{children}</div>;
};
