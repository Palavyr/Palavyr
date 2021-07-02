import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import React from "react";
import { NodeOptionalProps } from "../Contracts";

export const UnsetNodeButton = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.nodeIsSet() && (node.isTerminal || node.childNodeReferences.AllChildrenUnset() || node.nodeType === "Loopback");

    const onClick = () => {
        node.removeSelf();
        node.UpdateTree();
    };

    return shouldShow ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
