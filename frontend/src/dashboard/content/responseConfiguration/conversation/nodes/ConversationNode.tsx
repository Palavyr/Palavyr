import { Conversation, ConvoNode, Responses } from "@Palavyr-Types";
import { NodeTypeOptions } from "./NodeTypeOptions";
import { makeStyles } from "@material-ui/core";
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
    addNodes: (parentNode: ConvoNode, nodeList: Conversation, newIDs: Array<string>, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void) => void;
    setNodes: (nodeList: Conversation) => void;
    parentState: boolean
    changeParentState: (parentState: boolean) => void;
    dynamicNodeTypes: NodeTypeOptions; // TODO: Rejig the interface prop and make this prop not a thing
}

export type lineStyle = {
    borderColor: "black";
    borderStyle: "solid";
    borderWidth: number;
    zIndex: number;
};

export const connectionStyle: lineStyle = {
    borderColor: "black",
    borderStyle: "solid",
    borderWidth: 1,
    zIndex: 0,
};

export const ConversationNode = ({ nodeList, node, parentId: parentId, addNodes, setNodes, parentState, changeParentState, dynamicNodeTypes }: IConversationNode) => {

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
                        addNodes={addNodes} // func to add nodes to nodelist
                        setNodes={setNodes}
                        parentState={parentState}
                        changeParentState={changeParentState}
                        optionPath={node.optionPath}
                        dynamicNodeTypes={dynamicNodeTypes}
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
                                    addNodes={addNodes}
                                    setNodes={setNodes}
                                    parentState={nodeState}
                                    changeParentState={changeNodeState}
                                    dynamicNodeTypes={dynamicNodeTypes}
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
