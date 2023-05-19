import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    interfaceElement: {
        paddingBottom: "1rem",
    },
}));

interface INodeInterfaceHeader {
    isRoot: boolean;
    optionPath: string;
    nodeId: string;
}

export const NodeHeader = ({ isRoot, optionPath, nodeId }: INodeInterfaceHeader) => {
    const cls = useStyles();
    return (
        <>
            <Typography className={cls.interfaceElement} variant={isRoot ? "h5" : "body1"} align="center">
                {isRoot ? "Begin" : optionPath === "Continue" ? optionPath : "If " + optionPath}
            </Typography>
            {/* <Typography align="center" variant="subtitle2">{`Unique Id: ${nodeId.slice(0, 3)}`}</Typography> */}
        </>
    );
};
