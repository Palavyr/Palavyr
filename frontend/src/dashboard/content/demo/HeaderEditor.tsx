import React from "react";
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { cloneDeep } from "lodash";

interface IEditor {
    initialData: string;
    setEditorState: (data: string) => void;
    label?: string;
}

export const HeaderEditor = ({ setEditorState, initialData }: IEditor) => {
    const initData = cloneDeep(initialData ?? "");

    return (
        <CKEditor
            style={{ alignText: "center" }}
            editor={ClassicEditor}
            config={{
                toolbar: ["heading", "|", "bold", "italic", "numberedList", "bulletedList", "|", "link"],
            }}
            data={initData}
            onInit={(editor) => {
                // You can store the "editor" and use when it is needed.
                // console.log('Editor is ready to use!', editor);
            }}
            onChange={(event, editor) => {
                const data = editor.getData();
                setEditorState(data);
            }}
            onBlur={(event, editor) => {
                console.log("Blur.", editor);
            }}
            onFocus={(event, editor) => {
                console.log("Focus.", editor);
            }}
        />
    );
};
