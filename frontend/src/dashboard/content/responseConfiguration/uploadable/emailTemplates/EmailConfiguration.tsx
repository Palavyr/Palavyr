import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Upload } from "../Upload";
import { ViewEmailTemplate } from "./ViewTemplate";
import { EmailEditor } from "./EmailEditor";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { makeStyles, Typography } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { VariableDetail } from "@Palavyr-Types";
import { EditorDetails } from "./EditorDetails";

const buttonText = "Add Email Template";
const summary = "Upload a new Email Response";

const useStyles = makeStyles((theme) => ({
    saveOrCancel: {
        marginTop: "2rem",
        textAlign: "right",
    },
}));

export const EmailConfiguration = () => {
    var client = new ApiClient();
    var classes = useStyles();
    let fileReader = new FileReader();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [emailTemplate, setEmailTemplate] = useState<string>("");
    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>([]);
    const [modalState, setModalState] = useState<boolean>(false);
    const toggleModal = () => {
        setModalState(!modalState);
    };
    const [accordState, setAccordState] = useState<boolean>(false);
    const toggleAccord = () => {
        setAccordState(!accordState);
    };

    const [editorAccordstate, setEditorAccordState] = useState<boolean>(false);
    const toggleEditorAccord = () => {
        setEditorAccordState(!editorAccordstate);
    };

    const handleFileRead = async (e: any) => {
        const content = fileReader.result;
        if (content && typeof content == "string") {
            const { data: emailTemplate } = await client.Configuration.Email.SaveEmailTemplate(areaIdentifier, content);
            setEmailTemplate(emailTemplate);
            setModalState(false);
            setAccordState(false);
        } else {
            console.log("Failed to read file contents.");
        }
    };

    const handleFileSave = (files: File[]) => {
        if (files[0] != null) {
            fileReader.onloadend = handleFileRead;
            fileReader.readAsText(files[0]);
        }
    };

    const saveEditorData = async () => {
        const data = emailTemplate;
        var res = await client.Configuration.Email.SaveEmailTemplate(areaIdentifier, data);
    };

    const loadEmailTemplate = useCallback(async () => {
        const { data: emailTemplate } = await client.Configuration.Email.GetEmailTemplate(areaIdentifier);
        const { data: variableDetails } = await client.Configuration.Email.GetVariableDetails();

        setVariableDetails(variableDetails);
        setEmailTemplate(emailTemplate);
        setLoaded(true);
        return () => {
            setLoaded(false);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        loadEmailTemplate();
        return () => {
            setLoaded(false);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, loadEmailTemplate]);

    const uploadDetails = () => {
        return <EditorDetails variableDetails={variableDetails} />;
    };

    return (
        <>
            <Typography style={{marginTop: "1.4rem"}} align="center" variant="h4">
                Email Response
            </Typography>
            <Typography paragraph align="center">Use this editor to create an HTML email template that will be sent as the email response for this area.</Typography>
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
                acceptedFiles={["text/html", "text/plain"]}
            />
            <EmailEditor uploadDetails={uploadDetails} accordState={editorAccordstate} toggleAccord={toggleEditorAccord} setEmailTemplate={setEmailTemplate} emailTemplate={emailTemplate}>
                <div className={classes.saveOrCancel}>
                    <SaveOrCancel onSave={() => saveEditorData()} onCancel={() => loadEmailTemplate()} useModal={true} />
                </div>
            </EmailEditor>
            {loaded && <ViewEmailTemplate emailTemplate={emailTemplate} />}
        </>
    );
};
