import { TextField } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React from "react";
import { MultiChoiceOptions } from "./MultiChoiceOptions";

export type MultiChoiceEditorProps = {
    switchState: boolean;
    setSwitchState: SetState<boolean>;
    text: string;
    setText: SetState<string>;
    options: string[];
    setOptions: SetState<string[]>;
    onClick: (text: string, options: string[]) => void;
    locked: boolean;
};
export const MultiChoiceTextEditor = ({ switchState, setSwitchState, text, setText, options, setOptions, onClick, locked }: MultiChoiceEditorProps) => {
    return (
        <>
            <TextField margin="dense" value={text} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
            <MultiChoiceOptions locked={locked} options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={() => onClick(text, options)} />
        </>
    );
};
