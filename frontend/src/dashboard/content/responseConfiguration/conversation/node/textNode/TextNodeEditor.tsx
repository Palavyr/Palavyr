import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Dialog, DialogTitle, DialogContent, DialogActions } from "@material-ui/core";
import { NodeTypeOptions } from "@Palavyr-Types";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { INodeReferences, IPalavyrNode } from "../../Contracts";
import NodeUpdater from "../actions/NodeUpdater";
import { MultiChoiceTextEditor } from "./MutichoiceTextEditor";
import { TextEditor } from "./TextEditor";

export interface TextNodeEditorProps {
    userText: string;
    editorIsOpen: boolean;
    closeEditor: () => void;
    childNodeReferences: INodeReferences;
    currentNode: IPalavyrNode;
    isMultiOptionType: boolean;
    shouldShowMultiOption: boolean;
    isAnabranchLocked: boolean;
}

export const TextNodeEditor = ({ isMultiOptionType, shouldShowMultiOption, isAnabranchLocked, currentNode, childNodeReferences, userText, editorIsOpen, closeEditor }: TextNodeEditorProps) => {
    const [options, setOptions] = useState<string[]>([]);
    const [text, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);
    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const { repository } = useContext(DashboardContext);

    useEffect(() => {
        setText(userText);
        const referenceOptions = childNodeReferences.collectPathOptions();
        setOptions(referenceOptions);
    }, []);

    const addOptionOnClick = () => {
        options.push("");
        setOptions(options);
        setSwitchState(!switchState);
    };

    const handleMultiOptionUpdateNode = (valueOptions: string[], nodeTypeOptions: NodeTypeOptions) => {
        NodeUpdater.updateNode(currentNode, valueOptions, nodeTypeOptions);
    };

    const handleTextOnlyUpdate = async (userText: string) => {
        const areaId = currentNode.palavyrLinkedList.areaId;
        const updatedNode = await repository.Conversations.ModifyConversationNodeText(currentNode.nodeId, areaId, userText);
        NodeUpdater.updateText(currentNode, updatedNode === null ? userText : updatedNode.text);
    };

    return (
        <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
            <DialogTitle>Edit a conversation node</DialogTitle>
            <DialogContent>
                {editorIsOpen &&
                    (isMultiOptionType ? (
                        shouldShowMultiOption ? (
                            <MultiChoiceTextEditor
                                locked={isAnabranchLocked}
                                switchState={switchState}
                                setSwitchState={setSwitchState}
                                text={text}
                                setText={setText}
                                options={options}
                                setOptions={setOptions}
                                onClick={addOptionOnClick}
                            />
                        ) : (
                            <TextEditor initialText={userText} setText={setText} text={text} />
                        )
                    ) : (
                        <TextEditor initialText={userText} setText={setText} text={text} />
                    ))}
            </DialogContent>
            <DialogActions>
                <SaveOrCancel
                    position="right"
                    customSaveMessage="Node Text Updated"
                    customCancelMessage="Changes cancelled"
                    useSaveIcon={false}
                    saveText="Update Node Text"
                    onSave={async () => {
                        await handleTextOnlyUpdate(text);
                        if (isMultiOptionType) {
                            handleMultiOptionUpdateNode(options, nodeTypeOptions);
                        }
                        closeEditor();
                        return true;
                    }}
                    onCancel={closeEditor}
                    timeout={200}
                />
            </DialogActions>
        </Dialog>
    );
};
