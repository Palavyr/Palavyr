import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { ConversationTreeContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { NodeOptionalProps } from "@Palavyr-Types";

export const UnsetNodeButton = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.nodeIsSet() && (node.isTerminal || node.childNodeReferences.AllChildrenUnset() || node.nodeType === "Loopback") && !node.isPalavyrAnabranchEnd;
    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const onClick = () => {
        node.unsetSelf(nodeTypeOptions);
    };

    return shouldShow ? <SinglePurposeButton size="small" buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
