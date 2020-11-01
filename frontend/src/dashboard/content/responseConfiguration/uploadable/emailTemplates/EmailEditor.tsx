import React from 'react';
import { Accordion, AccordionSummary, Typography, AccordionDetails, makeStyles, Divider } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

// TODO: SWITCHING TO https://www.youtube.com/watch?v=kykC7i9VUE4&ab_channel=DarwinTutorials
// https://ckeditor.com/docs/ckeditor5/latest/builds/guides/integration/frameworks/react.html
import CKEditor from '@ckeditor/ckeditor5-react';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { cloneDeep } from 'lodash';

interface IEdit {
    accordState: any;
    toggleAccord: any;
    setEmailTemplate: any;
    emailTemplate: any;
    children: React.ReactNode;
}

const useStyles = makeStyles(theme => ({
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
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
    accordianBody: {
        backgroundColor: "#C7ECEE",
    }
}))


const editorDetails = () => {

    return (
        <div className="alert alert-info">
            <div>
                Use this editor to create an HTML email template that will be sent as the email response for this area.
                </div><br></br>
            <div>
                When composing the email template, you may choose to include variables that will be substituted from your account details and from the chat. Currently the supported variables are:
                        <div>
                    <ul>
                        <li>&#123;%Name%&#125; : The name provided by the client in the chat dialog.</li>
                        <li>&#123;%Company%&#125; : Your company name provided in Settings. </li>
                        <li>&#123;%Logo%&#125; : The logo you provided in Settings</li>
                    </ul>
                </div>

            </div>
        </div>
    )
}

export const EmailEditor = ({ accordState, toggleAccord, emailTemplate, setEmailTemplate, children }: IEdit) => {

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
                        {editorDetails()}
                    </div>
                    <br></br>
                    <div className={classes.editorContainer}>
                        <CKEditor
                            id="editor"
                            editor={ClassicEditor}
                            data={initData}
                            config={editorConfig}
                            onInit={editor => {
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
