import { ConvoNode } from "@Palavyr-Types";
import React, { useState, useEffect, useContext } from "react";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { ConversationNodeInterface } from "./ConversationNodeInterface";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { getChildNodesToRender } from "./nodeUtils/commonNodeUtils";
import { _getAllParentNodeIds, _getParentNode, _splitNodeChildrenString } from "./nodeUtils/_coreNodeUtils";

import "./ConversationNode.css";

export interface IConversationNode {
    node: ConvoNode;
    reRender: () => void;
}

export type lineStyle = {
    borderColor: "white" | string;
    borderStyle: "solid";
    borderWidth: number;
    zIndex: number;
};

export const connectionStyle: lineStyle = {
    borderColor: "white",//"#54585A",
    borderStyle: "solid",
    borderWidth: 1,
    zIndex: 0,
};


export const ConversationNode = ({ node, reRender }: IConversationNode) => {
    const { nodeList } = useContext(ConversationTreeContext);
    const [nodeState, changeNodeState] = useState<boolean>(true);
    const [loaded, setLoaded] = useState(false);

    const allParentNodes = _getAllParentNodeIds(node, nodeList);
    const childNodes = getChildNodesToRender(node, nodeList);

    const nextReRender = () => {
        changeNodeState(!nodeState);
    }

    useEffect(() => {
        setLoaded(true);
        return () => setLoaded(false);
    }, []);

    if (node === null) {
        return null;
    }

    let steppedLineNodes: string[] = [];

    if (allParentNodes && allParentNodes.length == 1) {
        steppedLineNodes.push(allParentNodes[0].nodeId);
        if (allParentNodes[0].isSplitMergeType) {
            const siblingNodeIds = _splitNodeChildrenString(allParentNodes[0].nodeChildrenString).filter((x: string) => x !== node.nodeId);
            siblingNodeIds.map((id: string) => steppedLineNodes.push(id));
        }
    } else if (allParentNodes) {
        steppedLineNodes = allParentNodes.map(x => x.nodeId);
    }
    const nodeWrapper = "tree-item-" + node?.nodeId;
    return (
        <>
            <div className={"tree-item " + nodeWrapper}>
                <div className="tree-block-wrap">
                    <ConversationNodeInterface
                        key={node.nodeId}
                        node={node}
                        reRender={reRender}
                    />
                </div>
                {childNodes.length > 0 && (
                    <div key={node.nodeId} className="tree-row">
                        {node.shouldRenderChildren
                            ? childNodes
                                  .filter((n: ConvoNode) => n !== undefined)
                                  .map((nextNode, index) => <ConversationNode key={nextNode.nodeId + "-" + index.toString()} node={nextNode} reRender={nextReRender} />)
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
