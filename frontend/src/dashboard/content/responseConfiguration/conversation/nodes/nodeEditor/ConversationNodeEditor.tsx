import React, { useState, useEffect } from "react";
import { ConvoNode, Conversation, ValueOptionDelimiter } from "@Palavyr-Types";
import { NodeTypeOptionsDefinition } from "../NodeTypeOptions";
import { ApiClient } from "@api-client/Client";
import { cloneDeep } from "lodash";
import { updateNodeList, createNewChildIDs, addNodes } from "../conversationNodeUtils";
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from "@material-ui/core";
import { MultiChoiceOptions } from "./MultiChoiceOptions";


export interface INodeEditorModal {
    modalState: boolean;
    setModalState: (state: boolean) => void;
    node: ConvoNode;
    changeParentState: (parentState: boolean) => void;
    parentState: boolean;
    setNodes: (nodeList: Conversation) => void;
    nodeList: Conversation;
}


const isMultiOptionType = (nodeType: string) => {

    return (
        nodeType === NodeTypeOptionsDefinition.MultipleChoiceAsPath.value
        || nodeType === NodeTypeOptionsDefinition.MultipleChoiceContinue.value
    )
}


export const NodeEditorModal = ({ modalState, setModalState, node, nodeList, changeParentState, parentState, setNodes }: INodeEditorModal) => {

    const client = new ApiClient();

    const [options, setOptions] = useState<Array<string>>([]);
    const [textState, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);

    useEffect(() => {
        setText(node.text);
        if (isMultiOptionType(node.nodeType)) {
            setOptions(node.valueOptions.split(ValueOptionDelimiter))
        }

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []) // TODO: LOOK HERE <==================================== I added this in as a test

    const handleCloseModal = () => {
        setModalState(false);
    }

    const handleUpdateNode = async (value: string, valueOptions: string[]) => {

        var nodeData = cloneDeep((await client.Conversations.GetConversationNode(node.nodeId)).data as ConvoNode);
        nodeData.text = value;

        if (isMultiOptionType(node.nodeType)) {
            let optionPaths;
            let childIds;
            if (node.nodeType === NodeTypeOptionsDefinition.MultipleChoiceAsPath.value) {
                optionPaths = valueOptions;
                const numChildren: number = optionPaths.filter(x => x !== null && x !== "").length
                childIds = createNewChildIDs(numChildren);
            } else {
                optionPaths = NodeTypeOptionsDefinition.MultipleChoiceContinue.pathOptions;
                const numChildren: number = optionPaths.filter(x => x !== null && x !== "").length
                childIds = createNewChildIDs(numChildren);
            }
            nodeData.valueOptions = valueOptions.join(ValueOptionDelimiter);
            await addNodes(nodeData, nodeList, childIds, optionPaths, valueOptions, setNodes); // create new nodes and update the Database
        } else {
            await client.Conversations.PutConversationNode(nodeData.nodeId, nodeData);
            var newNodeList = updateNodeList(nodeList, nodeData);
            setNodes(newNodeList);
        }
    }

    return (
        <Dialog open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Edit a conversation node</DialogTitle>
            <DialogContent>
                <TextField
                    margin="dense"
                    value={textState}
                    multiline rows={4}
                    onChange={(event) => { setText(event.target.value) }}
                    id="question"
                    label="Question or Information"
                    type="text"
                    fullWidth
                />
                {
                    isMultiOptionType(node.nodeType) &&
                    <>
                        <hr></hr>
                        <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} />
                    </>
                }

            </DialogContent>
            <DialogActions>
                <Button
                    onClick={
                        async (e) => {
                            e.preventDefault();
                            handleUpdateNode(textState, options);
                            // await handleUpdateText(textState)
                            // if (isMultiOptionType(node.nodeType)) {
                            //     await handleUpdateOptions(options)
                            // }
                            handleCloseModal();
                        }
                    }
                    color="primary" variant="contained"
                >
                    Save
                </Button>
                <Button
                    onClick={handleCloseModal}
                    color="secondary"
                    variant="contained"
                >
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    );
};
