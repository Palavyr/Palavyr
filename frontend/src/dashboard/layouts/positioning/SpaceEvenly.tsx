import React from "react";
import { makeStyles } from "@material-ui/core";

type StyleProps = {
    vertical: boolean;
    center: boolean;
}

const useStyles = makeStyles((theme) => ({
    align: (props: StyleProps) => ({
        display: "flex",
        justifyItems: props.center ? "center" : "none",
        flexDirection: props.vertical ? "column" : "row",
        // justifyContent: "space-evenly",
    }),
}));

export interface SpaceEvenlyProps {
    children: React.ReactNode;
    center?: boolean;
    vertical?: boolean;
}

export const SpaceEvenly = ({ children, vertical, center }: SpaceEvenlyProps) => {
    const cls = useStyles({vertical, center});
    return <div className={cls.align}>{children}</div>;
};
