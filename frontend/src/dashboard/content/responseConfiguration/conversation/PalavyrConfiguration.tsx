import React, { useState, useCallback, useEffect } from "react";
import { NodeTypeOptions, TreeErrors } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Button, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import UndoIcon from "@material-ui/icons/Undo";
import RedoIcon from "@material-ui/icons/Redo";
import { isDevelopmentStage } from "@api-client/clientUtils";
import PalavyrErrorBoundary from "@common/components/Errors/PalavyrErrorBoundary";
import { ConversationHistoryTracker } from "./node/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrDataStructure/PalavyrLinkedList";
import { TreeErrorPanel } from "./MissingDynamicNodes";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";
import { ConfigurationNode } from "./node/baseNode/ConfigurationNode";
import { useContext } from "react";

import "./PalavyrNodeLines/reactflowStyles.css";

const useStyles = makeStyles(() => ({
    conversation: {
        position: "static",
        overflow: "auto",
        height: "100%",
        width: "100%",
    },
    treeErrorContainer: {
        margin: "0.5rem 0rem 1rem 2rem",
    },
    convoTreeMetaButtons: {
        borderRadius: "10px",
    },
    fieldSet: {
        position: "relative",
        borderWidth: "1px",
        borderStyle: "solid",
        marginBottom: "10px",
        padding: "25px",
    },
    treeWrap: {
        position: "relative",
        height: "100%",
    },

    floatingSave: {
        position: "fixed",
        textAlign: "center",
        top: 150,
        height: 500,
        right: 25,
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-evenly",
        alignItems: "center",
        zIndex: 99999,
    },
}));

export const StructuredConvoTree = () => {
    const cls = useStyles();

    const { planTypeMeta, repository } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [, setLoaded] = useState<boolean>(false);

    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptions>([]);
    const [treeErrors, setTreeErrors] = useState<TreeErrors>();

    const [conversationHistory, setConversationHistory] = useState<PalavyrLinkedList[]>([]);
    const [conversationHistoryPosition, setConversationHistoryPosition] = useState<number>(0);
    const [showDebugData, setShowDebugData] = useState<boolean>(false);

    const [linkedNodeList, setLinkedNodes] = useState<PalavyrLinkedList>();
    const historyTracker = new ConversationHistoryTracker(setConversationHistory, setConversationHistoryPosition, setLinkedNodes);

    const setTreeWithHistory = (updatedNodeList: PalavyrLinkedList) => {
        const freshNodeList = cloneDeep(updatedNodeList);
        historyTracker.addConversationHistoryToQueue(freshNodeList, conversationHistoryPosition, conversationHistory);
        setLinkedNodes(freshNodeList);
    };

    const loadNodes = useCallback(async () => {
        if (planTypeMeta) {
            const nodes = await repository.Conversations.GetConversation(areaIdentifier);

            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const nodesLinkedList = new PalavyrLinkedList(nodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);

            setNodeTypeOptions(nodeTypeOptions);
            setLinkedNodes(nodesLinkedList);

            setConversationHistory([cloneDeep(nodesLinkedList)]);
        }
    }, [areaIdentifier, planTypeMeta]);

    useEffect(() => {
        setLoaded(true);
        loadNodes();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadNodes]);

    useEffect(() => {
        if (linkedNodeList) {
            const nodeList = linkedNodeList.compileToConvoNodes();
            if (nodeList.length > 0) {
                (async () => {
                    const treeErrors = await repository.Conversations.GetErrors(areaIdentifier, nodeList);
                    setTreeErrors(treeErrors);
                })();
            }
        }
        return () => {
            setTreeErrors(undefined);
        };
    }, [areaIdentifier, linkedNodeList]);

    const toggleDebugData = () => {
        setShowDebugData(!showDebugData);
        setLinkedNodes(cloneDeep(linkedNodeList));
    };

    const onSave = async () => {
        if (linkedNodeList && planTypeMeta) {
            const compiledNodes = linkedNodeList.compileToConvoNodes();
            const updatedConvoNodes = await repository.Conversations.ModifyConversation(compiledNodes, areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const updatedLinkedList = new PalavyrLinkedList(updatedConvoNodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions, repository);
            historyTracker.addConversationHistoryToQueue(updatedLinkedList, conversationHistoryPosition, conversationHistory);
            setLinkedNodes(updatedLinkedList);
            window.location.reload(); // TODO: Just fix the perf problem. Clicking to many things loads too many listeners, which locks the whole browser on save.
            return true;
        } else {
            return false;
        }
    };

    const resetTree = async () => {
        if (linkedNodeList && planTypeMeta) {
            const head = linkedNodeList.retrieveCleanHeadNode().compileConvoNode(areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const newList = new PalavyrLinkedList([head], areaIdentifier, () => null, nodeTypeOptions, repository);
            setTreeWithHistory(newList);
        }
    };

    const [paddingBuffer, setPaddingBuffer] = useState<number>(1);

    return (
        <ConversationTreeContext.Provider value={{ nodeTypeOptions, setNodes: setTreeWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData }}>
            {/* <AreaConfigurationHeader
            divider={treeErrors?.anyErrors}
            title="Chat Editor"
            subtitle="Use this editor to create the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
        /> */}
            <PalavyrErrorBoundary>
                <div className={cls.conversation}>
                    <div className={cls.treeErrorContainer}>{treeErrors && <TreeErrorPanel treeErrors={treeErrors} />}</div>
                </div>
                <PalavyrErrorBoundary>
                    <div id={"palavyr-tree"} className={cls.treeWrap}>{linkedNodeList !== undefined && <ConfigurationNode currentNode={linkedNodeList.rootNode} pBuffer={paddingBuffer} />}</div>
                </PalavyrErrorBoundary>
            </PalavyrErrorBoundary>
            <div className={cls.floatingSave}>
                <SaveOrCancel size="large" position="right" onSave={onSave} />
                <Button
                    size="small"
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
                    size="small"
                    variant="contained"
                    className={cls.convoTreeMetaButtons}
                    endIcon={<RedoIcon />}
                    onClick={() => {
                        historyTracker.stepConversationForwardOneStep(conversationHistoryPosition, conversationHistory);
                    }}
                >
                    Redo
                </Button>
                <Button
                    size="small"
                    variant="contained"
                    className={cls.convoTreeMetaButtons}
                    endIcon={<RemoveIcon />}
                    onClick={() => {
                        if (paddingBuffer > 0.5) setPaddingBuffer(paddingBuffer - 0.5);
                    }}
                >
                    Spacing
                </Button>
                <Button
                    size="small"
                    variant="contained"
                    className={cls.convoTreeMetaButtons}
                    endIcon={<AddIcon />}
                    onClick={() => {
                        if (paddingBuffer < 10) setPaddingBuffer(paddingBuffer + 0.5);
                    }}
                >
                    Spacing
                </Button>

                {isDevelopmentStage() && (
                    <>
                        <Button size="small" variant="contained" className={cls.convoTreeMetaButtons} onClick={toggleDebugData}>
                            Toggle Debug Data
                        </Button>
                        <Button size="small" variant="contained" className={cls.convoTreeMetaButtons} onClick={resetTree}>
                            Reset Tree
                        </Button>
                    </>
                )}
            </div>
        </ConversationTreeContext.Provider>
    );
};
