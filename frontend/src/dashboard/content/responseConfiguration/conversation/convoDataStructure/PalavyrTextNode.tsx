import React, { useEffect, useState } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Typography, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from "@material-ui/core";
import { NodeTypeOptions, ConvoNode, SetState } from "@Palavyr-Types";
import { MultiChoiceOptions } from "../nodes/nodeInterface/nodeEditor/MultiChoiceOptions";
import { IPalavyrLinkedList } from "./Contracts";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrTextNode extends PalavyrNode {
    constructor(
        containerList: IPalavyrLinkedList,
        nodeTypeOptions: NodeTypeOptions,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftmostBranch: boolean
    ) {
        super(containerList, nodeTypeOptions, repository, node, nodeList, setTreeWithHistory, leftmostBranch);
    }

    public renderNodeFace() {
        const cls = useNodeInterfaceStyles();
        return ({ openEditor }) => {
            return this.renderPalavyrNodeBody()({
                openEditor,
                children: (
                    <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                        {this.userText}
                    </Typography>
                ),
            });
        };
    }

    public renderNodeEditor() {
        return ({ editorIsOpen, closeEditor }) => {
            const [options, setOptions] = useState<string[]>([]);
            const [text, setText] = useState<string>("");

            const handleUpdateNode = (userText: string, valueOptions: string[]) => {
                this.userText = userText;
                if (this.isMultiOptionType && this.shouldShowMultiOption) {
                    this.createOrTruncateChildNodes(valueOptions, this.isMultiOptionType);
                } else {
                    this.setTreeWithHistory(this.palavyrLinkedList);
                }
            };

            // isMultiOptionType // is either by path or continue, or is a dynamic type as path or continue
            // this.shouldShowMultiOption // true: by path, false: continue

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>{this.isMultiOptionType ? this.renderMultiChoiceTextEditor(text, setText, options, setOptions)() : this.renderTextEditor(setText, text)()}</DialogContent>
                    <DialogActions>
                        <SaveOrCancel
                            position="right"
                            customSaveMessage="Node Text Updated"
                            customCancelMessage="Changes cancelled"
                            useSaveIcon={false}
                            saveText="Update Node Text"
                            onSave={async () => {
                                handleUpdateNode(text, options);
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

    public renderMultiChoiceTextEditor(text: string, setText: SetState<string>, options: string[], setOptions: SetState<string[]>) {
        return () => {
            const [switchState, setSwitchState] = useState<boolean>(true);

            useEffect(() => {
                setText(this.userText);
                setOptions(this.valueOptions);
            }, [options, this.valueOptions]);

            const addMultiChoiceOptionsOnClick = () => {
                options.push("");
                // need to add node reference if we should be making paths (multi choice as path)
                setOptions(options);
                setSwitchState(!switchState);
            };
            return (
                <>
                    <TextField margin="dense" value={text} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
                    <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                </>
            );
        };
    }

    public renderTextEditor(setText: SetState<string>, text: string) {
        return () => {
            useEffect(() => {
                setText(this.userText);
            }, []);
            return <TextField margin="dense" value={text} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />;
        };
    }
}
