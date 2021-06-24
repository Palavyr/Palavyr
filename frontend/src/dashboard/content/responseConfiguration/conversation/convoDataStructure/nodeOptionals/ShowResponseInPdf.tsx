import React from "react";
import { NodeCheckBox } from "../../nodes/nodeInterface/NodeCheckBox";
import { NodeOptionalProps } from "../Contracts";

export const ShowResponseInPdf = ({ node }: NodeOptionalProps) => {
    const shouldShow = () => {
        const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
        return !node.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(node.nodeType);
    };
    const onChange = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        node.shouldPresentResponse = checked;
        node.setTreeWithHistory(node.palavyrLinkedList);
    };

    return shouldShow() ? <NodeCheckBox label="Show response in PDF" checked={node.shouldPresentResponse} onChange={onChange} /> : <></>;
};
