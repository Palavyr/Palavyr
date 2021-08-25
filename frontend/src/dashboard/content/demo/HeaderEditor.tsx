import React from "react";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { cloneDeep } from "lodash";

interface IEditor {
    initialData?: string;
    setEditorState: (data: string) => void;
    label?: string;
}

export const HeaderEditor = ({ setEditorState, initialData }: IEditor) => {
    const initData = cloneDeep(initialData ? initialData : "");

    return (
        <CKEditor
            style={{ alignText: "center" }}
            editor={ClassicEditor}
            config={{
                toolbar: ["heading", "|", "bold", "italic", "numberedList", "bulletedList", "|", "alignment", "|", "link"],
                // toolbar: {
                //     items: [
                //         "heading",
                //         "|",
                //         "alignment",
                //         "|",
                //         "bold",
                //         "italic",
                //         "strikethrough",
                //         "underline",
                //         "subscript",
                //         "superscript",
                //         "|",
                //         "link",
                //         "|",
                //         "bulletedList",
                //         "numberedList",
                //         "todoList",
                //         "-", // break point
                //         "fontfamily",
                //         "fontsize",
                //         "fontColor",
                //         "fontBackgroundColor",
                //         "|",
                //         "code",
                //         "codeBlock",
                //         "|",
                //         "insertTable",
                //         "|",
                //         "outdent",
                //         "indent",
                //         "|",
                //         // "uploadImage",
                //         // "blockQuote",
                //         // "|",
                //         "undo",
                //         "redo",
                //     ],
                //     shouldNotGroupWhenFull: false,
                // },
            }}
            data={initData}
            onInit={editor => {
                // You can store the "editor" and use when it is needed.
            }}
            onChange={(event, editor) => {
                const data = editor.getData();
                setEditorState(data);
            }}
            onBlur={(event, editor) => {}}
            onFocus={(event, editor) => {}}
        />
    );
};
