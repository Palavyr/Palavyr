import { Typography } from "@material-ui/core";
import React from "react";
import { NodeOptionalProps } from "../Contracts";

export const SplitMergeAnchorLabel = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrSplitmergeMergePoint;
    return shouldShow ? <Typography>This is the primary sibling. Branches will merge to this node.</Typography> : <></>;
};
