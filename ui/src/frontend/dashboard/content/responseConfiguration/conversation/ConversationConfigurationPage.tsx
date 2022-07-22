import React, { useState, useCallback, useEffect } from "react";
import { ConversationDesignerNodeResource, IPalavyrLinkedList, NodeTypeOptionResources, SetState, TreeErrorsResource } from "@Palavyr-Types";
import { useParams } from "react-router-dom";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ConversationHistoryTracker } from "./node/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrDataStructure/PalavyrLinkedList";
import { useContext } from "react";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { StructuredConvoTree } from "./PalavyrConfiguration";

export const ConversationConfigurationPage = () => {
    const { planTypeMeta, repository } = useContext(DashboardContext);
    const { intentId } = useParams<{ intentId: string }>();
    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptionResources>([]);
    const [linkedNodeList, setLinkedNodes] = useState<IPalavyrLinkedList>();
    const [historyTracker, setHistoryTracker] = useState<ConversationHistoryTracker | null>(null);
    const [treeErrors, setTreeErrors] = useState<TreeErrorsResource>();
    const [useNewEditor, setUseNewEditor] = useState<boolean>(true);

    const loadNodes = useCallback(async () => {
        if (planTypeMeta) {
            const nodes = await repository.Conversations.GetConversation(intentId);

            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(intentId, planTypeMeta);

            const tracker = new ConversationHistoryTracker(setLinkedNodes, linkedNodeList, nodeTypeOptions);
            const initialList = new PalavyrLinkedList(nodes, intentId, (treeUpdate: PalavyrLinkedList) => tracker.addConversationHistoryToQueue(treeUpdate), nodeTypeOptions, repository, []);
            tracker.initializeConversation(initialList);

            setNodeTypeOptions(nodeTypeOptions);
            setHistoryTracker(tracker);
        }
    }, [intentId, planTypeMeta]);

    useEffect(() => {
        loadNodes();
    }, [loadNodes, planTypeMeta]);

    const errorCheckCallback = async (setTreeErrors: SetState<TreeErrorsResource>, repository: PalavyrRepository, intentId: string, nodeList: ConversationDesignerNodeResource[]) => {
        const treeErrors = await repository.Conversations.GetErrors(intentId, nodeList);
        setTreeErrors(treeErrors);
    };

    const onSave = async () => {
        if (historyTracker && historyTracker.linkedNodeList && planTypeMeta) {
            const compiledNodes = historyTracker.linkedNodeList.compileToConvoNodes();
            const updatedConvoNodes = await repository.Conversations.ModifyConversation(compiledNodes, intentId);
            const nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(intentId, planTypeMeta);
            const updatedLinkedList = new PalavyrLinkedList(
                updatedConvoNodes,
                intentId,
                (treeUpdate: PalavyrLinkedList) => historyTracker.addConversationHistoryToQueue(treeUpdate),
                nodeTypeOptions,
                repository,
                [] // Yes this is tech debt. I"m sorry future Paul or whoever works on this next :/
            );
            historyTracker.addConversationHistoryToQueue(updatedLinkedList);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            {historyTracker && historyTracker.linkedNodeList && linkedNodeList && planTypeMeta && (
                <StructuredConvoTree
                    planTypeMeta={planTypeMeta}
                    useNewEditor={useNewEditor}
                    treeErrors={treeErrors}
                    setTreeErrors={setTreeErrors}
                    setUseNewEditor={setUseNewEditor}
                    errorCheckCallback={errorCheckCallback}
                    historyTracker={historyTracker}
                    loadNodes={loadNodes}
                    nodeTypeOptions={nodeTypeOptions}
                    onSave={onSave}
                />
            )}
        </>
    );
};
