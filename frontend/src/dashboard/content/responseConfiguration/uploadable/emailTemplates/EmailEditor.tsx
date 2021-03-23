import React, { Dispatch, SetStateAction } from 'react';
import { Accordion, AccordionSummary, Typography, AccordionDetails, makeStyles, Divider } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

// TODO: SWITCHING TO https://www.youtube.com/watch?v=kykC7i9VUE4&ab_channel=DarwinTutorials
// https://ckeditor.com/docs/ckeditor5/latest/builds/guides/integration/frameworks/react.html
import CKEditor from '@ckeditor/ckeditor5-react';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { cloneDeep } from 'lodash';

export interface IEdit {
    accordState: boolean;
    toggleAccord(): void;
    setEmailTemplate: Dispatch<SetStateAction<string>>;
    emailTemplate: string;
    children: React.ReactNode;
    uploadDetails: () => React.ReactNode;
}

const useStyles = makeStyles(() => ({
    heading: {
        fontWeight: "bold"
    },
    editorContainer: {
        width: "100%"
    },
    table: {
        width: "100%",
        marginTop: "0.3rem",
        marginBottom: "0.3rem"
    },
    accordian: {
        width: "100%"
    },
    accordianHead: {
        background: "linear-gradient(354deg, rgb(1,161,214,1) 10%, rgba(0,212,255,1) 70%)",
    },
    accordianBody: {
        backgroundColor: "#FAFCE8",
    }
}))

export const EmailEditor = ({ uploadDetails, accordState, toggleAccord, emailTemplate, setEmailTemplate, children }: IEdit) => {

    const classes = useStyles();
    const initData = cloneDeep(emailTemplate);

    const editorConfig = {
        toolbar: [ 'heading', '|', 'bold', 'italic', 'numberedList', 'bulletedList', '|', 'indent', 'outdent', '|', 'link', 'table', 'mediaEmbed', '|', 'undo', 'redo' ]
    }

    return (
        <Accordion className={classes.accordian} expanded={accordState} >
            <AccordionSummary className={classes.accordianHead} onClick={toggleAccord} expandIcon={<ExpandMoreIcon />} aria-controls="panel3a-content" id="panel3a-header">
                <Typography className={classes.heading}>
                    Use an editor to craft your response email
                </Typography>
            </AccordionSummary>
            <AccordionDetails className={classes.accordianBody} >
                <div className={classes.table}>
                    <div>
                       {uploadDetails()}
                    </div>
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
                                console.log('Blur.', editor);
                            }}
                            onFocus={(event, editor) => {
                                console.log('Focus.', editor);
                            }}
                        />
                    </div>
                    <Divider />
                    {children}
                </div>
            </AccordionDetails>
        </Accordion>
    );
}
