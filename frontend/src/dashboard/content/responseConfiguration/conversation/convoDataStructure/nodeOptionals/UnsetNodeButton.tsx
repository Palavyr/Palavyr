import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeTypeCode } from "@Palavyr-Types";
import React from "react";
import { NodeOptionalProps } from "../Contracts";

export const UnsetNodeButton = ({ node }: NodeOptionalProps) => {
    const shouldShow =
        (node.nodeIsSet() && (!node.isPalavyrAnabranchMember || node.isAnabranchLocked) && !node.isAnabranchLocked && (node.isTerminal || node.nodeTypeCode === NodeTypeCode.I)) ||
        node.nodeTypeCode === NodeTypeCode.VII;

    const onClick = () => {
        node.removeSelf();
        node.UpdateTree();
    };

    return shouldShow ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
