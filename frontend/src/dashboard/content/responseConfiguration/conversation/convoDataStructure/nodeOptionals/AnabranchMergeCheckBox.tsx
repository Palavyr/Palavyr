import { Tooltip } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useState } from "react";
import { NodeCheckBox } from "../../nodes/nodeInterface/NodeCheckBox";
import { IPalavyrNode, NodeOptionalProps } from "../Contracts";

export const AnabranchMergeCheckBox = ({ node }: NodeOptionalProps) => {
    const disabled = node.isPalavyrAnabranchStart && node.isPalavyrAnabranchEnd;

    const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>) => {
        const checked = event.target.checked;
        const origin = node.anabranchContext.anabranchOriginId;
        const anabranchOriginNode = node.palavyrLinkedList.findNode(origin);

        if (checked) {
            node.isPalavyrAnabranchEnd = true;
            anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(node);
            setAnabranchMergeChecked(true);
        } else {
            node.isPalavyrAnabranchEnd = false;
            setAnabranchMergeChecked(false);
            anabranchOriginNode.recursiveDereferenceThisAnabranchOrigin(node);
        }
        node.UpdateTree();
    };

    const shouldShow = () => {
        const isChildOfAnabranchType = node.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
        const _shouldShow = !node.isPalavyrAnabranchStart && node.isPalavyrAnabranchMember && !node.isTerminal && !isChildOfAnabranchType && (node.isMemberOfLeftmostBranch || node.isPalavyrAnabranchStart);
        return _shouldShow;
    };

    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    return shouldShow() ? (
        <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
            <span>
                <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked)} />
            </span>
        </Tooltip>
    ) : (
        <></>
    );
};
