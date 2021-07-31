import { PalavyrHtmlTextEditor } from "@common/components/PalavyrTextEditor";
import { TextField } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useEffect } from "react";

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

export const HtmlTextEditor = ({ initialText, text, setText }: TextEditorProps) => {
    useEffect(() => {
        if (initialText) {
            setText(initialText);
        }
    }, []);

    const nodeEditorConfig = {
        toolbar: ["heading", "|", "bold", "italic", "|", "numberedList", "bulletedList", "|", "link", "table", "mediaEmbed", "|", "undo", "redo"],
    };

    return <PalavyrHtmlTextEditor editorControl={setText} initialData={initialText} editorConfig={nodeEditorConfig} />;
};
