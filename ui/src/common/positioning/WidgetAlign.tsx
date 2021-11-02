import { makeStyles } from "@material-ui/core";
import React from "react";

type Directions = "flex-start" | "flex-end" | "center";

export interface IAlign {
    children: React.ReactNode;
    direction?: Directions;
    float?: "left" | "right";
    verticalCenter?: boolean;
}

export type StyleProps = {
    direction?: Directions;
    float?: "left" | "right";
    verticalCenter?: boolean;
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

        if (props.verticalCenter) {
            styles = { ...styles, alignItems: "center" };
        }
        return styles;
    },
}));

export const Align = ({ direction, float, children, verticalCenter }: IAlign) => {
    const cls = useStyles({ direction, float, verticalCenter });

    return <div className={cls.align}>{children}</div>;
};
