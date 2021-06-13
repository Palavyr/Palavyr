import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

interface INodeInterfaceHeader {
    isRoot: boolean;
    optionPath: string;
}

const useStyles = makeStyles((theme) => ({
    interfaceElement: {
        paddingBottom: "1rem",
    },
}));

export const NodeHeader = ({ isRoot, optionPath }: INodeInterfaceHeader) => {
    const cls = useStyles();
    return (
        <Typography className={cls.interfaceElement} variant={isRoot ? "h5" : "body1"} align="center">
            {isRoot ? "Begin" : optionPath === "Continue" ? optionPath : "If " + optionPath}
        </Typography>
    );
};
