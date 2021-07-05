import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { NodeOptionalProps } from "../Contracts";

export const UnsetNodeButton = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.nodeIsSet() && (node.isTerminal || node.childNodeReferences.AllChildrenUnset() || node.nodeType === "Loopback") && !node.isPalavyrAnabranchEnd;
    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const onClick = () => {
        node.removeSelf(nodeTypeOptions);
    };

    return shouldShow ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
