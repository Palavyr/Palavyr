import React, { useState, useCallback, useEffect } from "react";
import { Conversation, NodeTypeOptions, TreeErrors } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { cloneDeep } from "lodash";
import { Button, Divider, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Align } from "dashboard/layouts/positioning/Align";
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from "@material-ui/icons/Redo";
import { isDevelopmentStage } from "@api-client/clientUtils";
import PalavyrErrorBoundary from "@common/components/Errors/PalavyrErrorBoundary";
import { ConversationHistoryTracker } from "../nodes/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrLinkedList";
import { TreeErrorPanel } from "../MissingDynamicNodes";

import "./stylesStructuredConvoTree.css";

const useStyles = makeStyles(() => ({
    conversation: {
        position: "static",
        overflow: "auto",
    },
    treeErrorContainer: {
        margin: "0.5rem 0rem 1rem 2rem",
    },
    convoTreeMetaButtons: {
        marginLeft: "0.7rem",
        borderRadius: "10px",
    },
}));

export const StructuredConvoTree = () => {
    const repository = new PalavyrRepository();
    const cls = useStyles();

    const { setIsLoading, planTypeMeta } = React.useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [, setLoaded] = useState<boolean>(false);

    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptions>([]);
    const [treeErrors, setTreeErrors] = useState<TreeErrors>();

    const [conversationHistory, setConversationHistory] = useState<PalavyrLinkedList[]>([]);
    const [conversationHistoryPosition, setConversationHistoryPosition] = useState<number>(0);
    const [showDebugData, setShowDebugData] = useState<boolean>(false);

    const [linkedNodeList, setLinkedNodes] = useState<PalavyrLinkedList>();
    const [rawNodeList, setRawNodeList] = useState<Conversation>([]);
    const historyTracker = new ConversationHistoryTracker(setConversationHistory, setConversationHistoryPosition, setLinkedNodes);

    const loadNodes = useCallback(async () => {
        if (planTypeMeta) {
            const nodes = await repository.Conversations.GetConversation(areaIdentifier);
            setRawNodeList(nodes);

            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const nodesLinkedList = new PalavyrLinkedList(nodes, nodeTypeOptions, areaIdentifier, () => null);

            setNodeTypeOptions(nodeTypeOptions);
            setLinkedNodes(nodesLinkedList);

            setIsLoading(false);
            setConversationHistory([cloneDeep(nodesLinkedList)]);
        }

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, planTypeMeta]);

    useEffect(() => {
        setIsLoading(true);
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    useEffect(() => {
        if (linkedNodeList) {
            const nodeList = linkedNodeList.compile();
            if (nodeList.length > 0) {
                (async () => {
                    const treeErrors = await repository.Conversations.GetErrors(areaIdentifier, nodeList);
                    setTreeErrors(treeErrors);
                })();
            }
        }
        // Disabling this here because we don't want to rerender on requriedNodes change (thought that seems almost what we want, but actually isn't)
        // We compute this on the nodeList in fact, and the requiredNodes only change when we change areaIdentifier (or update the dynamic tables option on the other tab)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, linkedNodeList]);

    const toggleDebugData = () => {
        setShowDebugData(!showDebugData);
        setLinkedNodes(cloneDeep(linkedNodeList));
        // setNodes(cloneDeep(nodeList));
    };


    const onSave = async () => {
        if (linkedNodeList) {
            const updatedConversation = await repository.Conversations.ModifyConversation(linkedNodeList.compile(), areaIdentifier);
            historyTracker.addConversationHistoryToQueue(linkedNodeList, conversationHistoryPosition, conversationHistory);
            // setNodes(updatedConversation);
            // setLinkedNodes(new PalavyrLinkedList(updatedConversation, areaIdentifier, () => null));
            return true;
        } else {
            return false;
        }
    };

    const setNodesWithHistory = (updatedNodeList: PalavyrLinkedList) => {
        const freshNodeList = cloneDeep(updatedNodeList);
        historyTracker.addConversationHistoryToQueue(freshNodeList, conversationHistoryPosition, conversationHistory);
        setLinkedNodes(linkedNodeList);
    };

    const resetTree = async () => {
        if (linkedNodeList && planTypeMeta) {
            const head = linkedNodeList.rootNode.compileConvoNode(areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const newList = new PalavyrLinkedList([head], nodeTypeOptions, areaIdentifier, () => null);
            setNodesWithHistory(newList);
        }
    };

    const Tree = linkedNodeList !== undefined ? linkedNodeList.renderNodeTree() : null;

    return (
        <ConversationTreeContext.Provider value={{ nodeTypeOptions, setNodes: setNodesWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData }}>
            <AreaConfigurationHeader
                divider={treeErrors?.anyErrors}
                title="Palavyr"
                subtitle="Your palavyr is the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
            />
            <Align>
                <SaveOrCancel position="right" onSave={onSave} />
                <Button
                    variant="contained"
                    className={cls.convoTreeMetaButtons}
                    startIcon={<UndoIcon />}
                    onClick={() => {
                        historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Undo
                </Button>
                <Button
                    variant="contained"
                    className={cls.convoTreeMetaButtons}
                    endIcon={<RedoIcon />}
                    onClick={() => {
                        historyTracker.stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Redo
                </Button>
                {isDevelopmentStage() && (
                    <>
                        <Button variant="contained" className={cls.convoTreeMetaButtons} onClick={toggleDebugData}>
                            Toggle Debug Data
                        </Button>
                        <Button variant="contained" className={cls.convoTreeMetaButtons} onClick={resetTree}>
                            Reset Tree
                        </Button>
                    </>
                )}
            </Align>
            {isDevelopmentStage() && (
                <>
                    {showDebugData &&
                        conversationHistory.map((convo: PalavyrLinkedList, index: number) => {
                            return (
                                <div key={index}>
                                    <Divider />
                                    {convo.compile().map((x) => " | " + x.nodeType)}
                                </div>
                            );
                        })}
                </>
            )}
            <PalavyrErrorBoundary>
                <div className={cls.conversation}>
                    <div className={cls.treeErrorContainer}>{treeErrors && <TreeErrorPanel treeErrors={treeErrors} />}</div>
                    <fieldset className="fieldset" id="tree-test">
                        <PalavyrErrorBoundary>{Tree !== null ? <Tree key="tree-start" /> : null}</PalavyrErrorBoundary>
                    </fieldset>
                </div>
            </PalavyrErrorBoundary>
        </ConversationTreeContext.Provider>
    );
};
