import React, { useState, useCallback, useEffect } from "react";
import { Conversation, NodeTypeOptions } from "@Palavyr-Types";
import { getRootNode } from "./nodes/conversationNodeUtils";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import { makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import "./ConvoTree.css";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";

const useStyles = makeStyles(() => ({
    conversation: {
        position: "static",
        overflow: "auto",
    },
}));

export const ConvoTree = () => {

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [, setLoaded] = useState<boolean>(false);
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

        setNodeOptionList(nodeOptionList);
        setNodes(cloneDeep(nodes));

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    const getMissingNodes = useCallback(async () => {
        const client = new ApiClient();
        const { data: missingNodes } = await client.Conversations.GetMissingNodes(areaIdentifier);
        setMissingNodeTypes(missingNodes);
    }, []);

    useEffect(() => {
        if (nodeList.length > 0) {
            getMissingNodes();
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, nodeList]);

    return (
        <>
            <AreaConfigurationHeader
                divider={missingNodeTypes.length > 0}
                title="Palavyr"
                subtitle="Your palavyr is the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing. Please be careful - all changes to your palavyr are immediately saved and we have not yet implemented a 'back' or 'undo' feature."
            />
            <div className={classes.conversation}>
                <div style={{margin: "0.5rem 0rem 1rem 2rem"}}>{missingNodeTypes.length > 0 && <MissingDynamicNodes missingNodeTypes={missingNodeTypes} />}</div>
                <form onSubmit={() => null}>
                    <fieldset className="fieldset" id="tree-test">
                        {/* <legend>{treeName}</legend> */}
                        <div className="main-tree tree-wrap">
                            {nodeList.length > 0 ? (
                                <ConversationNode key={"tree-start"} parentId={rootNode.nodeId} node={rootNode} nodeList={nodeList} setNodes={setNodes} parentState={true} changeParentState={() => null} nodeOptionList={nodeOptionList} />
                            ) : null}
                        </div>
                    </fieldset>
                </form>
            </div>
        </>
    );
};
