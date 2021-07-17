import React, { useState, useCallback, useEffect } from "react";
import { NodeTypeOptions, TreeErrors } from "@Palavyr-Types";
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
import { ConversationHistoryTracker } from "./node/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrDataStructure/PalavyrLinkedList";
import { TreeErrorPanel } from "./MissingDynamicNodes";
import AddIcon from "@material-ui/icons/Add";
import RemoveIcon from "@material-ui/icons/Remove";
import { ConfigurationNode } from "./node/baseNode/ConfigurationNode";

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
    fieldSet: {
        position: "relative",
        borderWidth: "1px",
        borderStyle: "solid",
        marginBottom: "10px",
        padding: "25px",
    },
    treeWrap: {
        position: "relative",
    },
    floatingSave: {
        position: "fixed",
        right: "50px",
        bottom: "50px",
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
            const nodesLinkedList = new PalavyrLinkedList(nodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions);

            setNodeTypeOptions(nodeTypeOptions);
            setLinkedNodes(nodesLinkedList);

            setIsLoading(false);
            setConversationHistory([cloneDeep(nodesLinkedList)]);
        }
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
            const updatedLinkedList = new PalavyrLinkedList(updatedConvoNodes, areaIdentifier, setTreeWithHistory, nodeTypeOptions);
            historyTracker.addConversationHistoryToQueue(updatedLinkedList, conversationHistoryPosition, conversationHistory);
            setLinkedNodes(updatedLinkedList);
            return true;
        } else {
            return false;
        }
    };

    const resetTree = async () => {
        if (linkedNodeList && planTypeMeta) {
            const head = linkedNodeList.retrieveCleanHeadNode().compileConvoNode(areaIdentifier);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            const newList = new PalavyrLinkedList([head], areaIdentifier, () => null, nodeTypeOptions);
            setTreeWithHistory(newList);
        }
    };

    const [paddingBuffer, setPaddingBuffer] = useState<number>(1);

    return (
        <ConversationTreeContext.Provider value={{ nodeTypeOptions, setNodes: setTreeWithHistory, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData }}>
            <AreaConfigurationHeader
                divider={treeErrors?.anyErrors}
                title="Palavyr"
                subtitle="This Palavyr is the personalized conversation flow you will provide to your potential customers. Consider planning this before implementing since you cannot modify the type of node at the beginning of the conversation without affect the nodes below."
            />
            <Align>
                <SaveOrCancel size="large" position="right" onSave={onSave} />
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
                <Button
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
                                    {convo.compileToConvoNodes().map((x) => " | " + x.nodeType)}
                                </div>
                            );
                        })}
                </>
            )}
            <PalavyrErrorBoundary>
                <div className={cls.conversation}>
                    <div className={cls.treeErrorContainer}>{treeErrors && <TreeErrorPanel treeErrors={treeErrors} />}</div>
                    <div className={cls.floatingSave}>
                        <SaveOrCancel size="large" position="right" onSave={onSave} />
                    </div>
                    <fieldset className={cls.fieldSet}>
                        <PalavyrErrorBoundary>
                            <div className={cls.treeWrap}>{linkedNodeList !== undefined && <ConfigurationNode currentNode={linkedNodeList.rootNode} pBuffer={paddingBuffer} />}</div>
                        </PalavyrErrorBoundary>
                    </fieldset>
                </div>
            </PalavyrErrorBoundary>
        </ConversationTreeContext.Provider>
    );
};
