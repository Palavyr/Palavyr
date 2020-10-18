import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Upload } from "../Upload";
import { ViewEmailTemplate } from "./ViewTemplate";
import { EmailEditor } from "./EmailEditor";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { makeStyles } from "@material-ui/core";
import { Statement } from "@common/components/Statement";
import { EmailHelp } from "dashboard/content/help/EmailHelp";


interface IEmailConfiguration {
    areaIdentifier: string;
}

const uploadDetails = () => {
    return (
        <div className="alert alert-info">
            <div>
                Use this dialog to upload a formatted HTML document that will be sent to potential customers enquiring about commercial purchases. This email will be used to send information, attachments, as well as a copy and a temporary
                link to the estimate (the link expires in 24 hours).
                        </div><br></br>
            <div>
                When composing the email template, you may choose to include the clients name (supplied during the chat) and a link to the estimate. To do this, simply include the following placeholders for the name and estimate link
                            respectively (<strong>Note the capitalization</strong>):
                            <div>
                    <ul>
                        <li>Name</li>
                        <li>Estimate</li>
                    </ul>
                </div>

            </div>
        </div>
    )
}
const buttonText = "Add Email Template";
const summary = "Upload a new Email Response";

const useStyles = makeStyles(theme => ({
    saveOrCancel: {
        marginTop: "2rem",
        textAlign: "right"
    }
}))

export const EmailConfiguration = ({ areaIdentifier }: IEmailConfiguration) => {
    var client = new ApiClient();
    var classes = useStyles();
    let fileReader = new FileReader();

    const [loaded, setLoaded] = useState<boolean>(false);

    const [emailTemplate, setEmailTemplate] = useState("");

    const [modalState, setModalState] = useState(false);
    const toggleModal = () => {
        setModalState(!modalState);
    }

    const [accordState, setAccordState] = useState(false);
    const toggleAccord = () => {
        setAccordState(!accordState)
    }

    const [editorAccordstate, setEditorAccordState] = useState(false);
    const toggleEditorAccord = () => {
        setEditorAccordState(!editorAccordstate);
    }

    const handleFileRead = async (e: any) => {
        const content = fileReader.result;
        if (content && typeof content == 'string') {
            var res = await client.Configuration.Email.SaveEmailTemplate(areaIdentifier, content)
            setEmailTemplate(res.data);
            setModalState(false);
            setAccordState(false);
        } else {
            console.log("Failed to read file contents.")
        }
    };

    const handleFileSave = (files: File[]) => {
        if (files[0] != null) {
            fileReader.onloadend = handleFileRead;
            fileReader.readAsText(files[0])
        }
    }

    const saveEditorData = async () => {
        const data = emailTemplate;
        var res = await client.Configuration.Email.SaveEmailTemplate(areaIdentifier, data)
    }

    const loadEmailTemplate = useCallback(async () => {
        var res = (await client.Configuration.Email.GetEmailTemplate(areaIdentifier)).data;
        setEmailTemplate(res);
        setLoaded(true)
        return () => {
            setLoaded(false);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        loadEmailTemplate()
        return () => {
            setLoaded(false)
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, loadEmailTemplate])

    return (
        <>
            <EmailHelp />
            <Upload
                modalState={modalState}
                toggleModal={toggleModal}
                accordState={accordState}
                toggleAccord={toggleAccord}
                handleFileSave={handleFileSave}
                areaIdentifier={areaIdentifier}
                buttonText={buttonText}
                summary={summary}
                uploadDetails={uploadDetails}
                acceptedFiles={['text/html', 'text/plain']}
            />
            <EmailEditor
                accordState={editorAccordstate}
                toggleAccord={toggleEditorAccord}
                setEmailTemplate={setEmailTemplate}
                emailTemplate={emailTemplate}
            >
                <div className={classes.saveOrCancel}>
                    <SaveOrCancel
                        onSave={() => saveEditorData()}
                        onCancel={() => loadEmailTemplate()}
                    />
                </div>
            </EmailEditor>
            {
                loaded &&
                <ViewEmailTemplate
                    emailTemplate={emailTemplate}
                />
            }
        </>
    );
};
