import React, { useEffect, useState } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Typography, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from "@material-ui/core";
import { NodeTypeOptions, ConvoNode, SetState } from "@Palavyr-Types";
import { MultiChoiceOptions } from "../nodes/nodeInterface/nodeEditor/MultiChoiceOptions";
import { IPalavyrLinkedList } from "./Contracts";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";
import { NodeUpdater } from "./NodeUpdater";

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
        const nodeUpdater = new NodeUpdater();

        return ({ editorIsOpen, closeEditor }) => {
            const [options, setOptions] = useState<string[]>([]);
            const [text, setText] = useState<string>("");
            const [switchState, setSwitchState] = useState<boolean>(true);
            useEffect(() => {
                setText(this.userText);
                const referenceOptions = this.childNodeReferences.collectPathOptions();
                setOptions(referenceOptions);
            }, []);

            const addOptionOnClick = () => {
                options.push("");
                setOptions(options);
                setSwitchState(!switchState);
            };

            const handleUpdateNode = (userText: string, valueOptions: string[]) => {
                nodeUpdater.updateNode(this, userText, valueOptions);
            };

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {editorIsOpen &&
                            (this.isMultiOptionType ? (
                                this.shouldShowMultiOption ? (
                                    <MultiChoiceTextEditor
                                        switchState={switchState}
                                        setSwitchState={setSwitchState}
                                        text={text}
                                        setText={setText}
                                        options={options}
                                        setOptions={setOptions}
                                        onClick={addOptionOnClick}
                                    />
                                ) : (
                                    <TextEditor initialText={this.userText} setText={setText} text={text} />
                                )
                            ) : (
                                <TextEditor initialText={this.userText} setText={setText} text={text} />
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
}

export type MultiChoiceEditorProps = {
    switchState: boolean;
    setSwitchState: SetState<boolean>;
    text: string;
    setText: SetState<string>;
    options: string[];
    setOptions: SetState<string[]>;
    onClick: (text: string, options: string[]) => void;
};
export const MultiChoiceTextEditor = ({ switchState, setSwitchState, text, setText, options, setOptions, onClick }: MultiChoiceEditorProps) => {
    return (
        <>
            <TextField margin="dense" value={text} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
            <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={() => onClick(text, options)} />
        </>
    );
};

export type TextEditorProps = {
    initialText: string;
    setText: SetState<string>;
    text: string;
};

export const TextEditor = ({ initialText, text, setText }: TextEditorProps) => {
    useEffect(() => {
        setText(initialText);
    }, []);
    return <TextField margin="dense" value={text} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />;
};
