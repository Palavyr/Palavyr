import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Dialog, DialogTitle, DialogContent, DialogActions, makeStyles } from "@material-ui/core";
import { NodeTypeOptions } from "@Palavyr-Types";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { INodeReferences, IPalavyrNode } from "../../Contracts";
import NodeUpdater from "../actions/NodeUpdater";
import { MultiChoiceTextEditor } from "./MutichoiceTextEditor";
import { HtmlTextEditor, TextEditor } from "./TextEditor";

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

const useStyles = makeStyles((theme) => ({
    editor: {
        minHeight: "250px",
    },
}));

export const TextNodeEditor = ({ isMultiOptionType, shouldShowMultiOption, isAnabranchLocked, currentNode, childNodeReferences, userText, editorIsOpen, closeEditor }: TextNodeEditorProps) => {
    const [options, setOptions] = useState<string[]>([]);
    const [text, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);
    const { nodeTypeOptions } = useContext(ConversationTreeContext);
    const { repository } = useContext(DashboardContext);

    const cls = useStyles();

    useEffect(() => {
        setText(userText);
        const referenceOptions = childNodeReferences.collectPathOptions();
        setOptions(referenceOptions);
    }, [childNodeReferences]);

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
            <DialogContent className={cls.editor}>
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
                            <HtmlTextEditor initialText={userText} setText={setText} text={text} />
                        )
                    ) : (
                        <HtmlTextEditor initialText={userText} setText={setText} text={text} />
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
