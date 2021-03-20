import React, { useState, useCallback, useEffect } from "react";
import { Conversation, NodeTypeOptions, PRODUCTION } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { ConversationNode } from "./nodes/ConversationNode";
import { MissingDynamicNodes } from "./MissingDynamicNodes";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from "@material-ui/icons/Redo";

import "./ConvoTree.css";
import { getRootNode } from "./nodes/nodeUtils/commonNodeUtils";
import { ConversationHistoryTracker } from "./nodes/ConversationHistoryTracker";
import { currentEnvironment } from "@api-client/clientUtils";

const useStyles = makeStyles(() => ({
    conversation: {
        backgroundColor: "#282630",
        position: "static",
        overflow: "auto",
    },
}));

export const ConvoTree = () => {
    var client = new ApiClient();
    const classes = useStyles();

    const { setIsLoading } = React.useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [, setLoaded] = useState<boolean>(false);
    const [nodeList, setNodes] = useState<Conversation>([]); // nodeList and state updater for the tree
    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptions>([]);
    const [missingNodeTypes, setMissingNodeTypes] = useState<string[]>([]);
    const [conversationHistory, setConversationHistory] = useState<Conversation[]>([]);
    const [conversationHistoryPosition, setConversationHistoryPosition] = useState<number>(0);
    const [showDebugData, setShowDebugData] = useState<boolean>(true);

    const rootNode = getRootNode(nodeList);

    const historyTracker = new ConversationHistoryTracker(setConversationHistory, setConversationHistoryPosition, setNodes);

    const toggleDebugData = () => {
        setShowDebugData(!showDebugData);
        setNodes(cloneDeep(nodeList));
    }

    const loadNodes = useCallback(async () => {
        const client = new ApiClient();

        const { data: nodes } = await client.Conversations.GetConversation(areaIdentifier);
        const { data: nodeTypeOptions } = await client.Conversations.GetNodeOptionsList(areaIdentifier);

        setNodeTypeOptions(nodeTypeOptions);
        setNodes(cloneDeep(nodes));
        setIsLoading(false);
        setConversationHistory([cloneDeep(nodes)]);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const onSave = async () => {
        const { data: updatedConversation } = await client.Conversations.ModifyConversation(nodeList, areaIdentifier);
        console.log(updatedConversation);
        historyTracker.addConversationHistoryToQueue(updatedConversation, conversationHistoryPosition, conversationHistory);
        setNodes(updatedConversation);
        return true;
    };

    const setNodesWithHistory = (updatedNodeList: Conversation) => {
        var freshNodeList = cloneDeep(updatedNodeList);
        setNodes(freshNodeList);
        historyTracker.addConversationHistoryToQueue(freshNodeList, conversationHistoryPosition, conversationHistory);
    };

    useEffect(() => {
        setIsLoading(true);
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    useEffect(() => {
        if (nodeList.length > 0) {
            (async () => {
                const { data: missingNodes } = await client.Conversations.GetMissingNodes(areaIdentifier, nodeList);
                setMissingNodeTypes(missingNodes);
            })();
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, nodeList]);

    return (
        <ConversationTreeContext.Provider value={{ nodeList, nodeTypeOptions, setNodes: setNodesWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData }}>
            <AreaConfigurationHeader
                divider={missingNodeTypes.length > 0}
                title="Palavyr"
                subtitle="Your palavyr is the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
            />
            <AlignCenter>
                <SaveOrCancel onSave={onSave} useModal />
                <Button
                    variant="outlined"
                    style={{ marginLeft: "0.7rem", borderRadius: "10px" }}
                    startIcon={<UndoIcon />}
                    onClick={() => {
                        historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Undo
                </Button>
                <Button
                    variant="outlined"
                    style={{ marginLeft: "0.7rem", borderRadius: "10px" }}
                    endIcon={<RedoIcon />}
                    onClick={() => {
                        historyTracker.stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Redo
                </Button>
                {currentEnvironment !== PRODUCTION && (
                    <Button
                        variant="outlined"
                        style={{ marginLeft: "0.7rem", borderRadius: "10px" }}
                        onClick={toggleDebugData}
                    >
                        Toggle Debug Data
                    </Button>
                )}
            </AlignCenter>
            <div className={classes.conversation}>
                <div style={{ margin: "0.5rem 0rem 1rem 2rem" }}>{missingNodeTypes.length > 0 && <MissingDynamicNodes missingNodeTypes={missingNodeTypes} />}</div>
                <form onSubmit={() => null}>
                    <fieldset className="fieldset" id="tree-test">
                        <div className="main-tree tree-wrap">{nodeList.length > 0 ? <ConversationNode key="tree-start" node={rootNode} reRender={() => null} /> : null}</div>
                    </fieldset>
                </form>
            </div>
        </ConversationTreeContext.Provider>
    );
};
