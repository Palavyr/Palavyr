import React, { useState, useEffect } from "react";
import { ConvoNode, ValueOptionDelimiter } from "@Palavyr-Types";
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions } from "@material-ui/core";
import { MultiChoiceOptions } from "./MultiChoiceOptions";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { updateMultiTypeOption } from "../../nodeUtils/mutliOptionUtils";
import { updateSingleOptionType } from "../../nodeUtils/commonNodeUtils";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";

export interface IConversationNodeEditor {
    modalState: boolean;
    setModalState: (state: boolean) => void;
    node: ConvoNode;
    parentNode: ConvoNode | null;
}

export const ConversationNodeEditor = ({ modalState, setModalState, node, parentNode }: IConversationNodeEditor) => {
    const [options, setOptions] = useState<Array<string>>([]);
    const [textState, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);

    const { nodeList, setNodes } = React.useContext(ConversationTreeContext);

    useEffect(() => {
        setText(node.text);
        if (node.isMultiOptionType && !isNullOrUndefinedOrWhitespace(node.valueOptions)) {
            setOptions(node.valueOptions.split(ValueOptionDelimiter));
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [node]);

    const handleCloseModal = () => {
        setModalState(false);
    };

    const handleUpdateNode = async (value: string, valueOptions: string[]) => {
        const updatedNode = { ...node };
        updatedNode.text = value;
        if (node.isMultiOptionType) {
            updateMultiTypeOption(updatedNode, nodeList, valueOptions, setNodes); // create new nodes and update the Database
        } else {
            updateSingleOptionType(updatedNode, nodeList, setNodes);
        }
    };

    const addMultiChoiceOptionsOnClick = () => {
        options.push("");
        setOptions(options);
        setSwitchState(!switchState);
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
                {node.isMultiOptionType && node.shouldShowMultiOption && (
                    <>
                        <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                    </>
                )}
            </DialogContent>
            <DialogActions>
                <SaveOrCancel
                    saveText="Ok"
                    onSave={async (e) => {
                        e.preventDefault();
                        await handleUpdateNode(textState, options);
                        handleCloseModal();
                        return true;
                    }}
                    onCancel={handleCloseModal}
                    timeout={200}
                />
            </DialogActions>
        </Dialog>
    );
};