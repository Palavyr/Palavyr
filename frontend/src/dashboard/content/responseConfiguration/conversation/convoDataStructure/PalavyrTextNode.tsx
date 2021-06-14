import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { MultiChoiceOptions } from "../nodes/nodeInterface/nodeEditor/MultiChoiceOptions";
import { NodeBody } from "./NodeBody";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrTextNode extends PalavyrNode {
    constructor(containerList, nodeTypeOptions, repository, rawNode, nodeList, rerender, leftMostBranch) {
        super(containerList, nodeTypeOptions, repository, rawNode, nodeList, rerender, leftMostBranch);
    }

    public renderNodeFace() {
        const cls = useNodeInterfaceStyles();
        return ({ openEditor }) => {
            return (
                <NodeBody openEditor={openEditor}>
                    <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                        {this.userText}
                    </Typography>
                </NodeBody>
            );
        };
    }

    public renderNodeEditor() {
        return ({ editorIsOpen, closeEditor }) => {
            const [options, setOptions] = useState<Array<string>>([]);
            const [textState, setText] = useState<string>("");

            const handleUpdateNode = (value: string, valueOptions: string[]) => {
                const updatedNode = { ...this.rawNode };
                updatedNode.text = value;
                if (this.isMultiOptionType) {
                    // updateMultiTypeOption(updatedNode, nodeList, valueOptions, setNodes);
                } else {
                    // updateSingleOptionType(updatedNode, nodeList, setNodes);
                }
            };

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>{this.renderTextEditor(setText, setOptions, textState, options)()}</DialogContent>
                    <DialogActions>
                        <SaveOrCancel
                            position="right"
                            customSaveMessage="Node Text Updated"
                            customCancelMessage="Changes cancelled"
                            useSaveIcon={false}
                            saveText="Update Node Text"
                            onSave={async () => {
                                handleUpdateNode(textState, options);
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
    }

    public renderTextEditor(setText, setOptions, textState, options) {
        return () => {
            const [switchState, setSwitchState] = useState<boolean>(true);

            useEffect(() => {
                setText(this.userText);
                if (this.isMultiOptionType && !isNullOrUndefinedOrWhitespace(this.valueOptions)) {
                    setOptions(this.valueOptions);
                }
            }, [this]);

            return (
                <>
                    <TextField margin="dense" value={textState} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
                    {this.renderMultiOptionInputs(setOptions, options, switchState, setSwitchState)()}
                </>
            );
        };
    }

    public renderMultiOptionInputs(setOptions, options, switchState, setSwitchState) {
        return () => {
            const addMultiChoiceOptionsOnClick = () => {
                options.push("");
                setOptions(options);
                setSwitchState(!switchState);
            };
            return (
                <>
                    {this.isMultiOptionType && this.shouldShowMultiOption && (
                        <>
                            <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                        </>
                    )}
                </>
            );
        };
    }
}
