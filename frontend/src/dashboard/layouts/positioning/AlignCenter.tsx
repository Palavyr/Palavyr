import { makeStyles } from "@material-ui/core";
import React from "react";

type Directions = "left" | "right" | "center";

export interface IAlign {
    children: React.ReactNode;
    direction?: Directions;
    float?: "left" | "right";
}

export type StyleProps = {
    direction: Directions;
    float: "left" | "right";
};

const useStyles = makeStyles((theme) => ({
    align: (props: StyleProps) => {
        let styles = {};
        styles = {
            display: "flex",
            justifyContent: props.direction ?? "center",
        };
        if (props.float) {
            const float = props.float;
            styles = { ...styles, float: float };
        }
        return styles;
    },
}));

export const Align = ({ direction, float, children }: IAlign) => {
    const cls = useStyles({ direction, float });

    return <div className={cls.align}>{children}</div>;
};
