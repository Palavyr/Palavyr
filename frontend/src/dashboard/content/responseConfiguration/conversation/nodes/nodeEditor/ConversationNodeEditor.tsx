import React, { useState, useEffect } from "react";
import { ConvoNode, Conversation, ValueOptionDelimiter } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { createNewChildIDs, addNodes, removeNodeByID } from "../conversationNodeUtils";
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions } from "@material-ui/core";
import { MultiChoiceOptions } from "./MultiChoiceOptions";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";

export interface IConversationNodeEditor {
    modalState: boolean;
    setModalState: (state: boolean) => void;
    node: ConvoNode;
    // setNodes: (nodeList: Conversation) => void;
    // nodeList: Conversation;
    // setTransactions: (transactions: ConvoNode[]) => void; // array of convoNodes - not quite the same thing as a 'Conversation' type
    // setIdsToDelete: (idsToDelete: string[]) => void;
}

const updateNodeWithinNodeList = (nodeData: ConvoNode, nodeList: Conversation, setTransactions: (transactions: ConvoNode[]) => void, setIdsToDelete: (idsToDelete: string[]) => void, transactions: ConvoNode[], idsToDelete: string[]) => {
    // replace the old node with the new node in the list
    const filteredNodeList = removeNodeByID(nodeData.nodeId, nodeList);
    filteredNodeList.push(nodeData);

    if (!idsToDelete.includes(nodeData.nodeId)) {
        setIdsToDelete([...idsToDelete, nodeData.nodeId])
    }

    const filteredTransactions = transactions.filter((x: ConvoNode) => x.nodeId !== nodeData.nodeId);
    filteredTransactions.push(nodeData);
    setTransactions(cloneDeep(filteredTransactions))
    return filteredNodeList;
};

export const ConversationNodeEditor = ({ modalState, setModalState, node }: IConversationNodeEditor) => {
    // const client = new ApiClient();

    const [options, setOptions] = useState<Array<string>>([]);
    const [textState, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);

    const { setTransactions, setIdsToDelete, transactions, idsToDelete, nodeList, setNodes, conversationHistory, setConversationHistory } = React.useContext(ConversationTreeContext);

    useEffect(() => {
        setText(node.text);
        if (node.isMultiOptionType) {
            setOptions(node.valueOptions.split(ValueOptionDelimiter));
        }

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const handleCloseModal = () => {
        setModalState(false);
    };

    const handleUpdateNode = async (value: string, valueOptions: string[]) => {
        // const { data: nodeData } = cloneDeep(await client.Conversations.GetConversationNode(node.nodeId));
        const nodeData = cloneDeep(nodeList.filter((nodeListNode: ConvoNode) => nodeListNode.nodeId === node.nodeId)[0]);

        nodeData.text = value;

        if (node.isMultiOptionType) {
            const optionPaths = valueOptions;
            const numChildren = optionPaths.filter((x) => x !== null && x !== "").length;
            const childIds = createNewChildIDs(numChildren);
            nodeData.valueOptions = valueOptions.join(ValueOptionDelimiter);
            await addNodes(nodeData, nodeList, childIds, optionPaths, valueOptions, setNodes, setTransactions, setIdsToDelete, conversationHistory, setConversationHistory); // create new nodes and update the Database

        } else {
            const updatedNodeList = updateNodeWithinNodeList(nodeData, nodeList, setTransactions, setIdsToDelete, transactions, idsToDelete);
            setNodes(updatedNodeList);
            setConversationHistory(updatedNodeList);
        }
    };

    return (
        <Dialog fullWidth open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Edit a conversation node</DialogTitle>
            <DialogContent>
                <TextField
                    margin="dense"
                    value={textState}
                    multiline
                    rows={4}
                    onChange={(event) => {
                        setText(event.target.value);
                    }}
                    id="question"
                    label="Question or Information"
                    type="text"
                    fullWidth
                />
                {node.isMultiOptionType && (
                    <>
                        <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} />
                    </>
                )}
            </DialogContent>
            <DialogActions>
                <SaveOrCancel
                    onSave={async (e) => {
                        e.preventDefault();
                        await handleUpdateNode(textState, options);
                        handleCloseModal();
                        return true;
                    }}
                    onCancel={handleCloseModal}
                />
            </DialogActions>
        </Dialog>
    );
};
