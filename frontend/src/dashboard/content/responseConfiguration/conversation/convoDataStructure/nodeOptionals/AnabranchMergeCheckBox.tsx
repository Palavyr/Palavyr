import { Tooltip } from "@material-ui/core";
import { NodeTypeOptions, SetState } from "@Palavyr-Types";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useState } from "react";
import { useEffect } from "react";
import { NodeCheckBox } from "../../nodes/nodeInterface/NodeCheckBox";
import { IPalavyrNode, NodeOptionalProps } from "../Contracts";
import NodeTypeOptionConfigurer from "../NodeTypeOptionConfigurer";

const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>, node: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) => {
    const checked = event.target.checked;
    const origin = node.anabranchContext.anabranchOriginId;
    const anabranchOriginNode = node.palavyrLinkedList.findNode(origin);

    if (checked) {
        node.isPalavyrAnabranchEnd = true;
        node.isAnabranchMergePoint = true;
        anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(node);
        setAnabranchMergeChecked(true);
        NodeTypeOptionConfigurer.ConfigureNodeTypeOptions(node, nodeTypeOptions);
        node.childNodeReferences.forEach((child: IPalavyrNode) => {
            if (!node.isAnabranchType) {
                child.isPalavyrAnabranchMember = false;
            }
            NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
        });
    } else {
        node.dereferenceThisAnabranchMergePoint(anabranchOriginNode, nodeTypeOptions);
        node.isPalavyrAnabranchEnd = false;
        node.isAnabranchMergePoint = false;
        setAnabranchMergeChecked(false);
        node.childNodeReferences.forEach((child: IPalavyrNode) => {
            if (!node.isAnabranchType) {
                child.isPalavyrAnabranchMember = true;
            }
            NodeTypeOptionConfigurer.RecursivelyReconfigureNodeTypeOptions(child, nodeTypeOptions);
        });
    }
    node.UpdateTree();
};

const shouldShow = (node: IPalavyrNode) => {
    const isChildOfAnabranchType = node.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
    const _shouldShow =
        node.nodeIsSet() && !node.isPalavyrAnabranchStart && node.isPalavyrAnabranchMember && !node.isTerminal && !isChildOfAnabranchType && node.anabranchContext.leftmostAnabranch && !node.isAnabranchLocked;

    if (node.isAnabranchMergePoint) {
        return true;
    } else {
        return _shouldShow;
    }
};

export const AnabranchMergeCheckBox = ({ node }: NodeOptionalProps) => {
    const disabled = node.isPalavyrAnabranchStart && node.isPalavyrAnabranchEnd;
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);
    const { nodeTypeOptions } = useContext(ConversationTreeContext);

    useEffect(() => {
        setAnabranchMergeChecked(node.isAnabranchMergePoint);
    }, []);

    return shouldShow(node) ? (
        <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
            <span>
                <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked, node, nodeTypeOptions)} />
            </span>
        </Tooltip>
    ) : (
        <></>
    );
};
