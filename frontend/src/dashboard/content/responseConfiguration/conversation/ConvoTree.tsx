import React, { useState, useCallback, useEffect } from "react";
import { Conversation, NodeTypeOptions, RequiredDetails } from "@Palavyr-Types";
import { getRootNode, addNodes } from "./nodes/conversationNodeUtils";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import { makeStyles } from "@material-ui/core";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";
import { useParams } from "react-router-dom";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import "./ConvoTree.css";

const useStyles = makeStyles((theme) => ({
    conversation: {
        position: "static",
        overflow: "auto",
    },
}));

export const ConvoTree = () => {
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { areaName: treeName } = React.useContext(DashboardContext);

    const [loaded, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Conversation>([]); // nodeList and state updater for the tree
    const rootNode = getRootNode(nodeList);
    const [nodeOptionList, setNodeOptionList] = useState<NodeTypeOptions>([]);

    // const [requiredNodes, setRequiredNodes] = useState<Array<string[]>>([]);
    const [missingNodeTypes, setMissingNodeTypes] = useState<string[]>([]);

    const classes = useStyles();

    const loadNodes = useCallback(async () => {
        const client = new ApiClient();

        const { data: nodes } = await client.Conversations.GetConversation(areaIdentifier);
        const { data: nodeOptionList } = await client.Conversations.GetNodeOptionsList(areaIdentifier);

        // TODO: Rename this. This will take the full node list from the server.
        setNodeOptionList(nodeOptionList);
        setNodes(cloneDeep(nodes));

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const getMissingNodes = useCallback(async () => {
        const client = new ApiClient();
        const { data: missingNodes } = await client.Conversations.GetMissingNodes(areaIdentifier);
        setMissingNodeTypes(missingNodes);
    }, []);

    useEffect(() => {
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    useEffect(() => {
        if (nodeList.length > 0) {
            getMissingNodes();
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, nodeList]);

    // const getEndingSequenceNodes = (nodeList: Conversation) => {
    //     return nodeList.filter((node) => node.nodeType === NodeTypeOptionsDefinition.EndingSequence.value);
    // }
    // const traverseThetreeFromBottom = (node: ConvoNode, nodeList: Conversation, requiredNodes: Array<RequiredDetails>): Array<RequiredDetails> => {

    //     if (node.isRoot) {
    //         return requiredNodes;
    //     } else {
    //         var requiredNodesClone = cloneDeep(requiredNodes);

    //         if (requiredNodesClone.map(x => x.type).includes(node.nodeType)) {
    //             requiredNodesClone.splice(requiredNodesClone.map(x => x.type).indexOf(node.nodeType), 1);
    //         }

    //         var nextNode = nodeList.filter(x => {
    //             return x.nodeChildrenString.split(",").includes(node.nodeId)
    //         })[0]

    //         return traverseThetreeFromBottom(nextNode, nodeList, requiredNodesClone)
    //     }
    // }

    // const getMissingNodes = (nodeList: Conversation, requiredNodes: Array<RequiredDetails>) => {
    //     const allMissingNodeTypes: Array<RequiredDetails> = [];
    //     const terminalNodes = getEndingSequenceNodes(nodeList);

    //     // for each terminal node, work your way back through the tree by collecting each parent node, and check if
    //     // that node if of the required type; if none are the required type, then add that type to the missing nodes.
    //     // --> If there are N terminal nodes and M dynamic tables, then we can expect at most N x M missing Nodes.
    //     for (var i = 0; i < terminalNodes.length; i++) {
    //         var missingNodes = traverseThetreeFromBottom(terminalNodes[i], nodeList, requiredNodes)
    //         missingNodes.forEach(x => allMissingNodeTypes.push(x))
    //     }
    //     return allMissingNodeTypes;
    // }
    // const nodeTypes = getMissingNodes(nodeList, requiredNodes);

    // }

    return (
        <>
            <ConversationHelp />
            <div className={classes.conversation}>
                {missingNodeTypes.length > 0 && <MissingDynamicNodes missingNodeTypes={missingNodeTypes} />}
                <form onSubmit={() => null}>
                    <fieldset className="fieldset" id="tree-test">
                        <legend>{treeName}</legend>
                        <div className="main-tree tree-wrap">
                            {nodeList.length > 0 ? (
                                <ConversationNode
                                    key={"tree-start"}
                                    parentId={rootNode.nodeId}
                                    node={rootNode}
                                    nodeList={nodeList}
                                    setNodes={setNodes}
                                    addNodes={addNodes}
                                    parentState={true}
                                    changeParentState={() => null}
                                    nodeOptionList={nodeOptionList}
                                />
                            ) : null}
                        </div>
                    </fieldset>
                </form>
            </div>
        </>
    );
};
