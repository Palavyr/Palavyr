import { Conversation, ConvoNode, NodeTypeOptions, Responses } from "@Palavyr-Types";
import React, { useState, useEffect } from "react";
import { getChildNodes } from "./conversationNodeUtils";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { v4 as uuid } from "uuid";
import { ConversationNodeInterface } from "./ConversationNodeInterface";
import "./ConversationNode.css";

export interface IConversationNode {
    nodeList: Conversation;
    node: ConvoNode;
    parentId: string | undefined;
    setNodes: (nodeList: Conversation) => void;
    parentState: boolean
    changeParentState: (parentState: boolean) => void;
    nodeOptionList: NodeTypeOptions;
    setTransactions: (transactions: ConvoNode[]) => void, // array of convoNodes - not quite the same thing as a 'Conversation' type
    setIdsToDelete: (idsToDelete: string[]) => void
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

export const ConversationNode = ({ nodeList, node, parentId, setNodes, setTransactions, setIdsToDelete, parentState, changeParentState, nodeOptionList }: IConversationNode) => {

    const [nodeState, changeNodeState] = useState<boolean>(true);
    const [loaded, setLoaded] = useState(false)

    let childNodes = getChildNodes(node.nodeChildrenString, nodeList);

    useEffect(() => {
        setLoaded(true)
        return () => setLoaded(false)
    }, []);

    if (node === null) {
        return null
    }

    const nodeWrapper = "tree-item-" + node?.nodeId;
    return (
        <>
            <div className={"tree-item " + nodeWrapper}>
                <div className="tree-block-wrap">
                    <ConversationNodeInterface
                        node={node} // node object
                        nodeList={nodeList} // array of node objects
                        setNodes={setNodes}
                        parentState={parentState}
                        changeParentState={changeParentState}
                        optionPath={node.optionPath}
                        nodeOptionList={nodeOptionList}
                        setIdsToDelete={setIdsToDelete}
                        setTransactions={setTransactions}
                    />
                </div>
                {childNodes.length > 0 && ( // if there are childNodes, then render them.
                    <div key={uuid()} className="tree-row">
                        {
                            childNodes.map((nextNode) => (
                                <ConversationNode
                                    key={nextNode.nodeId}
                                    node={nextNode}
                                    nodeList={nodeList}
                                    parentId={node.nodeId}
                                    setNodes={setNodes}
                                    parentState={nodeState}
                                    changeParentState={changeNodeState}
                                    nodeOptionList={nodeOptionList}
                                    setIdsToDelete={setIdsToDelete}
                                    setTransactions={setTransactions}
                                />

                            ))
                        }
                    </div>
                )}
            </div>
            {loaded && <SteppedLineTo key={node.nodeId} from={node.nodeId} to={parentId ? parentId : ""} fromAnchor="top" toAnchor="bottom" orientation="v" {...connectionStyle} />}
        </>
    );
};
