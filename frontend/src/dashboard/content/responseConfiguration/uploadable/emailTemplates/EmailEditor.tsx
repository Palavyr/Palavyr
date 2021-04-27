import React from "react";
import { makeStyles, Divider } from "@material-ui/core";

// TODO: SWITCHING TO https://www.youtube.com/watch?v=kykC7i9VUE4&ab_channel=DarwinTutorials
// https://ckeditor.com/docs/ckeditor5/latest/builds/guides/integration/frameworks/react.html
import CKEditor from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { cloneDeep } from "lodash";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { SetState } from "@Palavyr-Types";

export interface IEdit {
    accordianTitle: string;
    setEmailTemplate: SetState<string>;
    emailTemplate: string;
    children: React.ReactNode;
    uploadDetails: JSX.Element;
}

const useStyles = makeStyles(() => ({
    heading: {
        fontWeight: "bold",
    },
    editorContainer: {
        width: "100%",
    },
    table: {
        width: "100%",
        marginTop: "0.3rem",
        marginBottom: "0.3rem",
    },
}));

export const EmailEditor = ({ accordianTitle, uploadDetails, emailTemplate, setEmailTemplate, children }: IEdit) => {
    const classes = useStyles();
    const initData = cloneDeep(emailTemplate);

    const editorConfig = {
        toolbar: ["heading", "|", "bold", "italic", "numberedList", "bulletedList", "|", "indent", "outdent", "|", "link", "table", "mediaEmbed", "|", "undo", "redo"],
    };

    return (
        <PalavyrAccordian title={accordianTitle}>
            <div className={classes.table}>
                <div>{uploadDetails}</div>
                <br></br>
                <div className={classes.editorContainer}>
                    <CKEditor
                        id="editor"
                        editor={ClassicEditor}
                        data={initData}
                        config={editorConfig}
                        onInit={() => {
                            // You can store the "editor" and use when it is needed.
                            // console.log('Editor is ready to use!', editor);
                        }}
                        onChange={(event, editor) => {
                            const data = editor.getData();
                            setEmailTemplate(data);
                        }}
                        onBlur={(event, editor) => {
                            console.log("Blur.", editor);
                        }}
                        onFocus={(event, editor) => {
                            console.log("Focus.", editor);
                        }}
                    />
                </div>
                <Divider />
                {children}
            </div>
        </PalavyrAccordian>
    );
};
