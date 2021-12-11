import React, { useState, useCallback, useEffect } from "react";
import { ConvoNode, NodeTypeOptions, SetState, TreeErrors } from "@Palavyr-Types";
import { useParams } from "react-router-dom";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ConversationHistoryTracker } from "./node/ConversationHistoryTracker";
import { PalavyrLinkedList } from "./PalavyrDataStructure/PalavyrLinkedList";
import { useContext } from "react";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { StructuredConvoTree } from "./PalavyrConfiguration";
import { IPalavyrLinkedList } from "@Palavyr-Types";

export const IntroConversationConfigurationPage = () => {
    const { planTypeMeta, repository } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [nodeTypeOptions, setNodeTypeOptions] = useState<NodeTypeOptions>([]);
    const [linkedNodeList, setLinkedNodes] = useState<IPalavyrLinkedList>();
    const [historyTracker, setHistoryTracker] = useState<ConversationHistoryTracker | null>(null);
    const [treeErrors, setTreeErrors] = useState<TreeErrors>();
    const [useNewEditor, setUseNewEditor] = useState<boolean>(true);

    const loadNodes = useCallback(async () => {
        const nodes = await repository.Conversations.GetConversation(areaIdentifier);

        const nodeTypeOptions = await repository.Conversations.GetIntroNodeOptionsList();

        const tracker = new ConversationHistoryTracker(setLinkedNodes, linkedNodeList, nodeTypeOptions);
        const initialList = new PalavyrLinkedList(nodes, areaIdentifier, (treeUpdate: IPalavyrLinkedList) => tracker.addConversationHistoryToQueue(treeUpdate), nodeTypeOptions, repository);
        tracker.initializeConversation(initialList);

        setNodeTypeOptions(nodeTypeOptions);
        setHistoryTracker(tracker);
    }, [areaIdentifier, planTypeMeta]);

    useEffect(() => {
        loadNodes();
    }, [loadNodes, planTypeMeta]);

    const errorCheckCallback = async (setTreeErrors: SetState<TreeErrors>, repository: PalavyrRepository, areaIdentifier: string, nodeList: ConvoNode[]) => {
        const treeErrors = await repository.Conversations.GetIntroErrors(areaIdentifier, nodeList);
        setTreeErrors(treeErrors);
    };

    const onSave = async () => {
        if (historyTracker && historyTracker.linkedNodeList && planTypeMeta) {
            const compiledNodes = historyTracker.linkedNodeList.compileToConvoNodes();
            const updatedConvoNodes = await repository.Settings.Account.updateIntroduction(areaIdentifier, compiledNodes);
            let nodeTypeOptions = await repository.Conversations.GetNodeOptionsList(areaIdentifier, planTypeMeta);
            nodeTypeOptions = nodeTypeOptions.filter(x => x.value === "ProvideInfo");

            const updatedLinkedList = new PalavyrLinkedList(
                updatedConvoNodes,
                areaIdentifier,
                (updatedTree: IPalavyrLinkedList) => historyTracker.addConversationHistoryToQueue(updatedTree),
                nodeTypeOptions,
                repository
            );
            historyTracker.addConversationHistoryToQueue(updatedLinkedList);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            {historyTracker && linkedNodeList && (
                <StructuredConvoTree
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