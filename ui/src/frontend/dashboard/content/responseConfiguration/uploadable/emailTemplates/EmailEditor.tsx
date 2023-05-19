import React from "react";
import { makeStyles, Divider } from "@material-ui/core";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { SetState } from "@Palavyr-Types";
import { PalavyrHtmlTextEditor } from "@common/components/PalavyrTextEditor";

export interface IEdit {
    accordianTitle: string;
    setEmailTemplate: SetState<string>;
    emailTemplate: string;
    children: React.ReactNode;
    uploadDetails: JSX.Element;
}

const useStyles = makeStyles<{}>(() => ({
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

    const emailEditorConfig = {
        toolbar: ["heading", "|", "bold", "italic", "numberedList", "bulletedList", "|", "indent", "outdent", "|", "link", "table", "mediaEmbed", "|", "undo", "redo"],
    };

    return (
        <PalavyrAccordian title={accordianTitle}>
            <div className={classes.table}>
                <div>{uploadDetails}</div>
                <br></br>
                <PalavyrHtmlTextEditor editorControl={setEmailTemplate} initialData={emailTemplate} editorConfig={emailEditorConfig} />
                <Divider />
                {children}
            </div>
        </PalavyrAccordian>
    );
};
