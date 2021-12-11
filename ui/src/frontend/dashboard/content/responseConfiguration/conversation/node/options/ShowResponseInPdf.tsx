import React from "react";
import { PalavyrCheckbox } from "@common/components/PalavyrCheckBox";
import { NodeOptionalProps } from "@Palavyr-Types";

export const ShowResponseInPdf = ({ node }: NodeOptionalProps) => {
    const shouldShow = () => {
        const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
        return !node.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(node.nodeType) && !node.isImageNode;
    };
    const onChange = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        node.shouldPresentResponse = checked;
        node.setTreeWithHistory(node.palavyrLinkedList);
    };

    return shouldShow() ? <PalavyrCheckbox label="Show response in PDF" checked={node.shouldPresentResponse} onChange={onChange} /> : <></>;
};
