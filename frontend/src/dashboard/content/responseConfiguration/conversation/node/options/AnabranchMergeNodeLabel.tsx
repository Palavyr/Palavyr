import { Typography } from "@material-ui/core";
import React from "react";
import { NodeOptionalProps } from "../../Contracts";

export const AnabranchMergeNodeLabel = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrAnabranchEnd;
    return shouldShow ? <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography> : <></>;
};
