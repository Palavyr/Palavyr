import {  ConvoNode, NodeTypeOptions } from "@Palavyr-Types";
import React, { useState, useEffect, useContext } from "react";
import { getChildNodes } from "./conversationNodeUtils";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { ConversationNodeInterface } from "./ConversationNodeInterface";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import "./ConversationNode.css";

export interface IConversationNode {
    node: ConvoNode;
    parentId: string | undefined;
    parentState: boolean
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

export const ConversationNode = ({ node, parentId, parentState, changeParentState, nodeOptionList }: IConversationNode) => {

    const { nodeList } = useContext(ConversationTreeContext);

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
                        node={node}
                        parentState={parentState}
                        changeParentState={changeParentState}
                        optionPath={node.optionPath}
                        nodeOptionList={nodeOptionList}
                    />
                </div>
                {childNodes.length > 0 && (
                    <div key={node.nodeId} className="tree-row">
                        {
                            childNodes.map((nextNode) => (
                                <ConversationNode
                                    key={nextNode.nodeId}
                                    node={nextNode}
                                    parentId={node.nodeId}
                                    parentState={nodeState}
                                    changeParentState={changeNodeState}
                                    nodeOptionList={nodeOptionList}
                                />

                            ))
                        }
                    </div>
                )}
            </div>
            {loaded && <SteppedLineTo key={node.nodeId} from={node.nodeId} to={parentId ?? ""} fromAnchor="top" toAnchor="bottom" orientation="v" {...connectionStyle} />}
        </>
    );
};
