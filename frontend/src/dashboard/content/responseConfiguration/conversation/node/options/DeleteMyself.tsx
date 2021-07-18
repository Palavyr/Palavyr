import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { IPalavyrNode } from "../../Contracts";

export interface IDeleteMyself {
    node: IPalavyrNode;
}

export const DeleteMyself = ({ node }: IDeleteMyself) => {
    const shouldShow =
        !node.isRoot &&
        !node.isTerminal &&
        !node.isAnabranchLocked &&
        !node.isMultiOptionType &&
        !node.isPalavyrAnabranchStart &&
        node.nodeIsSet() &&
        node.parentNodeReferences.Length === 1 &&
        !node.parentNodeReferences.Single().isMultiOptionType;

    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const onClick = () => {
        node.DeleteCurrentNode(nodeTypeOptions);
    };

    return shouldShow ? <SinglePurposeButton size="small" buttonText="Delete" variant="outlined" color="primary" onClick={onClick} /> : <></>;
};
