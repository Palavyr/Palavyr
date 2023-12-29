import React from "react";
import { makeStyles } from "@material-ui/core";
import classnames from "classnames";

type StyleProps = {
    vertical?: boolean;
    center?: boolean;
};


const useStyles = makeStyles<{}>((theme: any) => ({
    align: (props: StyleProps) => ({
        display: "flex",
        flexDirection: props.vertical ? "column" : "row",
        justifyContent: props.center ? "center" : "space-evenly",
    }),
}));

export interface SpaceEvenlyProps {
    children: React.ReactNode;
    center?: boolean;
    vertical?: boolean;
    classes?: string;
}

export const SpaceEvenly = ({ children, vertical, center, classes }: SpaceEvenlyProps) => {
    const cls = useStyles({ vertical, center });
    return <div className={classnames(cls.align, classes)}>{children}</div>;
};
