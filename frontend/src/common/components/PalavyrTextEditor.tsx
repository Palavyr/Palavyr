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
                    let data: string = editor.getData();
                    if (data.includes("href")) {
                        const splitted = data.split("href");
                        const newData: string[] = [];
                        for (let index = 0; index < splitted.length; index++) {
                            const element = splitted[index];
                            if (index % 2 === 0) {
                                const targetAdded = [element, `target="_blank"`].join("");
                                newData.push(targetAdded);
                            } else {
                                newData.push(element);
                            }
                        }
                        data = newData.join(" href");
                    }
                    editorControl(data);
                }}
                onBlur={(event, editor) => {
                    const classNames = ["ck-balloon-panel"];
                    classNames.forEach((x: string, level: number) => {
                        const rot = document.getElementsByClassName(x);
                        for (let index = 0; index < rot.length; index++) {
                            const r = rot[index] as HTMLElement;
                            r.style.zIndex = `999${level}`;
                        }
                    });
                }}
                onFocus={(event, editor) => {
                    console.log("Focus.", editor);
                }}
            />
        </div>
    );
};
