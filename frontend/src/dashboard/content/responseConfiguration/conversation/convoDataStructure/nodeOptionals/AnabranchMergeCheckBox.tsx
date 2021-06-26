import { Tooltip } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useState } from "react";
import { useEffect } from "react";
import { NodeCheckBox } from "../../nodes/nodeInterface/NodeCheckBox";
import { IPalavyrNode, NodeOptionalProps } from "../Contracts";

const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>, node: IPalavyrNode) => {
    const checked = event.target.checked;
    const origin = node.anabranchContext.anabranchOriginId;
    const anabranchOriginNode = node.palavyrLinkedList.findNode(origin);

    if (checked) {
        node.isPalavyrAnabranchEnd = true;
        node.isAnabranchMergePoint = true
        anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(node);
        setAnabranchMergeChecked(true);
    } else {
        node.isPalavyrAnabranchEnd = false;
        node.isAnabranchMergePoint = false;

        setAnabranchMergeChecked(false);
        anabranchOriginNode.recursiveDereferenceThisAnabranchOrigin(node);
    }
    node.UpdateTree();
};

const shouldShow = (node: IPalavyrNode) => {
    const isChildOfAnabranchType = node.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
    const _shouldShow = node.nodeIsSet() && !node.isPalavyrAnabranchStart && node.isPalavyrAnabranchMember && !node.isTerminal && !isChildOfAnabranchType && node.anabranchContext.leftmostAnabranch;
    return _shouldShow;
};

export const AnabranchMergeCheckBox = ({ node }: NodeOptionalProps) => {
    const disabled = node.isPalavyrAnabranchStart && node.isPalavyrAnabranchEnd;
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    useEffect(() => {
        setAnabranchMergeChecked(node.isAnabranchMergePoint);
    }, []);

    return shouldShow(node) ? (
        <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
            <span>
                <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked, node)} />
            </span>
        </Tooltip>
    ) : (
        <></>
    );
};
