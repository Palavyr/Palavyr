import React from "react";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    align: {
        display: "flex",
        justifyContent: "space-evenly",
    },
}));

export interface SpaceEvenlyProps {
    children: React.ReactNode;
}

export const SpaceEvenly = ({ children }: SpaceEvenlyProps) => {
    const cls = useStyles();
    return <div className={cls.align}>{children}</div>;
};
