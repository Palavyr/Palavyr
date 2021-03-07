import { ConvoNode, NodeOption, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState, useEffect, useContext } from "react";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { ConversationNodeInterface } from "./ConversationNodeInterface";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import "./ConversationNode.css";
import { getChildNodes } from "./nodeUtils/commonNodeUtils";

export interface IConversationNode {
    node: ConvoNode;
    parentState: boolean;
    changeParentState: (parentState: boolean) => void;
    nodeOptionList: NodeTypeOptions;
    parentNode: ConvoNode | null;
    siblingIndex: number;
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

export const ConversationNode = ({ node, siblingIndex, parentNode, parentState, changeParentState, nodeOptionList }: IConversationNode) => {
    const { nodeList } = useContext(ConversationTreeContext);

    const [nodeState, changeNodeState] = useState<boolean>(true);
    const [loaded, setLoaded] = useState(false);

    let childNodes = getChildNodes(node.nodeChildrenString, nodeList);

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
    console.log("Stepped lines from here to ");
    console.log(steppedLineNodes);

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
                        siblingIndex={siblingIndex}
                        changeParentState={changeParentState}
                        optionPath={node.optionPath}
                        nodeOptionList={checkedNodeOptionList(nodeOptionList, parentNode, siblingIndex)}
                    />
                </div>
                {childNodes.length > 0 && (
                    <div key={node.nodeId} className="tree-row">
                        {node.shouldRenderChildren
                            ? childNodes.map((nextNode, index) => (
                                  <ConversationNode key={nextNode.nodeId + "-" + index.toString()} siblingIndex={index} node={nextNode} parentNode={node} parentState={nodeState} changeParentState={changeNodeState} nodeOptionList={nodeOptionList} />
                              ))
                            : null}
                    </div>
                )}
            </div>
            {loaded &&
                steppedLineNodes.map((id: string) => {
                    return id !== "" ? <SteppedLineTo key={node.nodeId + "-" + id + "-" + "stepped-line"} from={node.nodeId} to={id} fromAnchor="top" toAnchor="bottom" orientation="v" {...connectionStyle} /> : null;
                })}
        </>
    );
};

const checkedNodeOptionList = (nodeOptionList: NodeTypeOptions, parentNode: ConvoNode | null, siblingIndex: number) => {
    if (parentNode && parentNode.isSplitMergeType && siblingIndex > 0) {
        const compatible = nodeOptionList.filter((option: NodeOption) => option.groupName === "Provide Info" || option.groupName === "Info Collection");
        return compatible;
    } else {
        return nodeOptionList;
    }
};
