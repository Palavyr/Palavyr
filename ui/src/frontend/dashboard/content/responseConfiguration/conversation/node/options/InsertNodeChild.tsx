import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { ConversationTreeContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { IPalavyrNode } from "../../Contracts";

export interface IInsertNodeChild {
    node: IPalavyrNode;
}

export const InsertNodeChild = ({ node }: IInsertNodeChild) => {
    const shouldShow =
        !node.isAnabranchLocked &&
        !node.isAnabranchLocked &&
        !node.isPalavyrAnabranchStart &&
        !node.isTerminal &&
        node.nodeIsSet() &&
        !node.isMultiOptionType &&
        !node.isLoopbackAnchorType &&
        node.nodeType !== "Loopback";

    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const onClick = () => {
        node.InsertChildNodeLink(nodeTypeOptions);
    };
    return shouldShow ? <SinglePurposeButton size="small" buttonText="Insert Child" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
