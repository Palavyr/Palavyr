import React from "react";
import { Handle, Position } from "react-flow-renderer";
import { NodeInterface } from "../node/baseNode/NodeInterface";

type NodeFlowInterfaceProps = {
    data: any;
};
export const NodeFlowInterface = ({ data }: NodeFlowInterfaceProps) => {
    const currentNode = data.currentNode;
    return (
        <>
            <NodeInterface
                currentNode={currentNode}
                isRoot={currentNode.isRoot}
                nodeType={currentNode.nodeType}
                userText={currentNode.userText}
                shouldPresentResponse={currentNode.shouldPresentResponse}
                isMemberOfLeftmostBranch={currentNode.isMemberOfLeftmostBranch}
                imageId={currentNode.imageId}
                nodeId={currentNode.nodeId}
                joinedChildReferenceString={currentNode.childNodeReferences.joinedReferenceString}
                shouldDisableNodeTypeSelector={currentNode.shouldDisableNodeTypeSelector}
                optionPath={currentNode.optionPath}
            />
            <Handle style={{ border: "0px", background: "none" }} id={`b`} type="source" position={Position.Bottom} />
            <Handle style={{ border: "0px", background: "none" }} id={`a`}  type="target" position={Position.Top} />
        </>
    );
};
