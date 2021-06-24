import { SetState } from "@Palavyr-Types";
import React from "react";
import { useState } from "react";
import { NodeCheckBox } from "../../nodes/nodeInterface/NodeCheckBox";
import { NodeOptionalProps } from "../Contracts";
import { NodeCreator } from "../NodeCreator";

export const ShowMergeWithPrimarySiblingBranchOption = ({ node }: NodeOptionalProps) => {
    const shouldShow = node.isPalavyrSplitmergeMember && node.isPalavyrSplitmergePrimarybranch && node.nodeIsSet() && !node.isTerminal && !node.isMultiOptionType && node.isPenultimate();

    const onClick = async (event: { target: { checked: boolean } }, setMergeBoxChecked: SetState<boolean>) => {
        const checked = event.target.checked;
        setMergeBoxChecked(checked);
        if (checked) {
            node.RouteToMostRecentSplitMerge();
        } else {
            const nodeCreator = new NodeCreator();
            nodeCreator.addDefaultChild(node, "Continue");
        }
    };

    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(node.isPalavyrSplitmergeEnd);
    return shouldShow ? <NodeCheckBox label="Merge with primary sibling branch" checked={mergeBoxChecked} onChange={(event) => onClick(event, setMergeBoxChecked)} /> : <></>;
};
