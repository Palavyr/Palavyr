// TODO: SWITCHING TO https://www.youtube.com/watch?v=kykC7i9VUE4&ab_channel=DarwinTutorials
// https://ckeditor.com/docs/ckeditor5/latest/builds/guides/integration/frameworks/react.html
import React from "react";
import { makeStyles } from "@material-ui/core";
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { cloneDeep } from "lodash";
import { SetState } from "@Palavyr-Types";

const useStyles = makeStyles(() => ({
    editorContainer: {
        width: "100%",
        zIndex: 99999,
    },
}));

export interface IPalavyrHtmlTextEditor {
    editorControl: SetState<string>;
    initialData: string;
    editorConfig?: any;
}

export const PalavyrHtmlTextEditor = ({ editorControl, initialData, editorConfig }: IPalavyrHtmlTextEditor) => {
    const cls = useStyles();

    const initData = cloneDeep(initialData);
    const config = editorConfig ?? {
        toolbar: ["heading", "|", "bold", "italic", "numberedList", "bulletedList", "|", "indent", "outdent", "|", "link", "table", "mediaEmbed", "|", "undo", "redo"],
    };

    return (
        <div className={cls.editorContainer}>
            <CKEditor
                id="editor"
                editor={ClassicEditor}
                data={initData}
                config={config}
                onInit={() => {
                    // You can store the "editor" and use when it is needed.
                    // console.log('Editor is ready to use!', editor);
                }}
                onChange={(event, editor) => {
                    const data = editor.getData();
                    editorControl(data);
                }}
                onBlur={(event, editor) => {
                    const linkTextInput = document.getElementsByClassName("ck-input-text");
                    for (let index = 0; index < linkTextInput.length; index++) {
                        const t = linkTextInput[index] as HTMLElement;
                        t.style.zIndex = "9996";
                    }
                    const linkBox = document.getElementsByClassName("ck-balloon-panel_with-arrow");
                    for (let index = 0; index < linkBox.length; index++) {
                        const b = linkBox[index] as HTMLElement;
                        b.style.zIndex = "9997";
                    }

                    const form = document.getElementsByClassName("ck-link-form");
                    for (let index = 0; index < form.length; index++) {
                        const f = form[index] as HTMLElement;
                        f.style.zIndex = "9998";
                    }

                    const content = document.getElementsByClassName("ck-balloon-rotator__content");
                    for (let index = 0; index < content.length; index++) {
                        const c = content[index] as HTMLElement;
                        c.style.zIndex = "9999";
                    }
                }}
                onFocus={(event, editor) => {
                    console.log("Focus.", editor);
                }}
            />
        </div>
    );
};
