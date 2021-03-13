import { ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState, useEffect, useContext } from "react";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { ConversationNodeInterface } from "./ConversationNodeInterface";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { getChildNodesSortedByOptionPath, getUnsortedChildNodes } from "./nodeUtils/commonNodeUtils";
import { findMostRecentSplitMerge } from "./nodeUtils/splitMergeUtils";
import { _getParentNode } from "./nodeUtils/_coreNodeUtils";

import "./ConversationNode.css";
export interface IConversationNode {
    node: ConvoNode;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    nodeOptionList: NodeTypeOptions;
}

export type lineStyle = {
    borderColor: "black" | string;
    borderStyle: "solid";
    borderWidth: number;
    zIndex: number;
};

export const connectionStyle: lineStyle = {
    borderColor: "#54585A",
    borderStyle: "solid",
    borderWidth: 1,
    zIndex: 0,
};

export const ConversationNode = ({ node, parentState, changeParentState, nodeOptionList }: IConversationNode) => {
    const { nodeList } = useContext(ConversationTreeContext);

    const [nodeState, changeNodeState] = useState<boolean>(true);
    const [loaded, setLoaded] = useState(false);

    const { isDecendentOfSplitMerge, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling, orderedChildren } = findMostRecentSplitMerge(node, nodeList);
    console.group(node.text);
    console.log(node);
    console.log("Is decendant: " + isDecendentOfSplitMerge);
    console.log("Decendant level: " + decendentLevelFromSplitMerge);
    console.log("RootSiblingIndex: " + splitMergeRootSiblingIndex);
    console.log("RootSiblingNodeId: " + nodeIdOfMostRecentSplitMergePrimarySibling);
    console.log("Ordered Children: " + orderedChildren);
    console.groupEnd();
    const parentNode = _getParentNode(node, nodeList);
    const childNodes = node.isSplitMergeType ? getUnsortedChildNodes(node.nodeChildrenString, nodeList) : getChildNodesSortedByOptionPath(node.nodeChildrenString, nodeList);

    useEffect(() => {
        setLoaded(true);
        return () => setLoaded(false);
    }, []);

    if (node === null) {
        return null;
    }

    const steppedLineNodes: string[] = [];

    if (parentNode) {
        steppedLineNodes.push(parentNode.nodeId);
        if (parentNode.isSplitMergeType) {
            // plan - always check sibling nodes. If any sibling nodes are 'do not render children', then we assume that any siblings need to be merged in.
            const siblingNodeIds = parentNode.nodeChildrenString.split(",").filter((x: string) => x !== node.nodeId);
            siblingNodeIds.map((id: string) => steppedLineNodes.push(id));
        }
    }

    const nodeWrapper = "tree-item-" + node?.nodeId;
    return (
        <>
            <div className={"tree-item " + nodeWrapper}>
                <div className="tree-block-wrap">
                    <ConversationNodeInterface
                        key={node.nodeId}
                        node={node}
                        parentState={parentState}
                        parentNode={parentNode}
                        changeParentState={changeParentState}
                        optionPath={node.optionPath}
                        nodeOptionList={nodeOptionList}
                        orderedChildren={orderedChildren}
                        isDecendentOfSplitMerge={isDecendentOfSplitMerge}
                        decendentLevelFromSplitMerge={decendentLevelFromSplitMerge}
                        splitMergeRootSiblingIndex={splitMergeRootSiblingIndex}
                        nodeIdOfMostRecentSplitMergePrimarySibling={nodeIdOfMostRecentSplitMergePrimarySibling}
                    />
                </div>
                {childNodes.length > 0 && (
                    <div key={node.nodeId} className="tree-row">
                        {node.shouldRenderChildren
                            ? childNodes.filter((n: ConvoNode) => n !== undefined).map((nextNode, index) => (
                                  <ConversationNode key={nextNode.nodeId + "-" + index.toString()} node={nextNode} parentState={nodeState} changeParentState={changeNodeState} nodeOptionList={nodeOptionList} />
                              ))
                            : null}
                    </div>
                )}
            </div>
            {loaded &&
                steppedLineNodes.map((id: string, index: number) => {
                    return id !== "" && (index === 0 || index === steppedLineNodes.length - 1) ? (
                        <SteppedLineTo key={node.nodeId + "-" + id + "-" + "stepped-line"} from={node.nodeId} to={id} fromAnchor="top" toAnchor="bottom" orientation="v" {...connectionStyle} />
                    ) : null;
                })}
        </>
    );
};
