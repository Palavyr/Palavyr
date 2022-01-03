import { PalavyrHtmlTextEditor } from "@common/components/PalavyrTextEditor";
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
            <PalavyrHtmlTextEditor initialData={text} editorControl={setText} />
            <MultiChoiceOptions locked={locked} options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={() => onClick(text, options)} />
        </>
    );
};
