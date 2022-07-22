import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { UploadOrSelectFromExisting } from "@common/uploads/UploadOrChooseFromExisting";
import { makeStyles, Typography } from "@material-ui/core";
import { ResponseVariable } from "@Palavyr-Types";
import { EmailSubject } from "frontend/dashboard/content/settings/subject/EmailSubject";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { Upload } from "../Upload";
import { EditorDetails } from "./EditorDetails";
import { EmailEditor } from "./EmailEditor";
import { ViewEmailTemplate } from "./ViewTemplate";

const useEmailStyles = makeStyles(() => ({
    saveOrCancel: {
        marginTop: "2rem",
        textAlign: "right",
    },
}));

export interface EmailConfigurationComponentProps {
    variableDetails: ResponseVariable[];
    saveEmailTemplate: (emailTemplate: string) => Promise<string>;
    saveEmailSubject: (emailSubject: string) => Promise<string>;
    getCurrentEmailTemplate: () => Promise<string>;
    getCurrentEmailSubject: () => Promise<string>;
}

export const EmailConfigurationComponent = ({ variableDetails, saveEmailTemplate, saveEmailSubject, getCurrentEmailTemplate, getCurrentEmailSubject }: EmailConfigurationComponentProps) => {
    const { planTypeMeta, setSuccessOpen, setSuccessText } = useContext(DashboardContext);
    const cls = useEmailStyles();

    const fallbackFileReader = new FileReader();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [emailTemplate, setEmailTemplate] = useState<string>("");
    const [modalState, setmodalState] = useState<boolean>(false);
    const [intentSubjectState, setIntentSubjectState] = useState<string>("");

    const toggleModal = () => {
        setmodalState(!modalState);
    };

    const handleFileRead = async () => {
        const content = fallbackFileReader.result;
        if (content && typeof content == "string") {
            const emailTemplate = await saveEmailTemplate(content);
            setEmailTemplate(emailTemplate);
            setmodalState(false);
        } else {
            console.log("Failed to read file contents.");
        }
    };

    const handleFileSave = (files: File[]) => {
        if (files[0] != null) {
            fallbackFileReader.onloadend = handleFileRead;
            fallbackFileReader.readAsText(files[0]);
        }
    };

    const saveEditorData = async () => {
        const updatedEmailTemplate = await saveEmailTemplate(emailTemplate);
        setEmailTemplate(updatedEmailTemplate);

        setmodalState(false);
        return true;
    };

    const onIntentSubjectChange = (event: any) => setIntentSubjectState(event.target.value);
    const onSaveIntentSubject = async () => {
        const updatedSubject = await saveEmailSubject(intentSubjectState);
        setIntentSubjectState(updatedSubject);
        return true;
    };

    const loadIntentSubject = useCallback(async () => {
        const currentSubject = await getCurrentEmailSubject();
        setIntentSubjectState(currentSubject);
    }, []);

    const loadTemplate = useCallback(async () => {
        const currentEmailTemplate = await getCurrentEmailTemplate();
        setEmailTemplate(currentEmailTemplate);
        setLoaded(true);
    }, []);

    useEffect(() => {
        loadTemplate();
        loadIntentSubject();
        return () => {
            setLoaded(false);
        };
    }, [loadTemplate, loadIntentSubject]);

    return (
        <>
            <EmailSubject subject={intentSubjectState} accordianTitle="Email Subject Line" onChange={onIntentSubjectChange}>
                <div className={cls.saveOrCancel}>
                    <SaveOrCancel onSave={onSaveIntentSubject} onCancel={loadIntentSubject} />
                </div>
            </EmailSubject>
            <hr />
            <Upload
                modalState={modalState}
                toggleModal={toggleModal}
                handleFileSave={handleFileSave}
                buttonText="Add Email Template"
                summary="Upload"
                uploadDetails={<EditorDetails key="Upload" variableDetails={variableDetails} />}
                acceptedFiles={["text/html", "text/plain"]}
            />
            {planTypeMeta && planTypeMeta.allowedInlineEmailEditor ? (
                <EmailEditor
                    accordianTitle="Edit in the Browser"
                    uploadDetails={<EditorDetails key={"Editor"} variableDetails={variableDetails} />}
                    setEmailTemplate={setEmailTemplate}
                    emailTemplate={emailTemplate}
                >
                    <div className={cls.saveOrCancel}>
                        <SaveOrCancel onSave={saveEditorData} onCancel={loadTemplate} />
                    </div>
                </EmailEditor>
            ) : (
                <Typography style={{ padding: "0.6rem", fontWeight: "bold" }}> Subscribe to get access to the inline email editor </Typography>
            )}
            {loaded && <ViewEmailTemplate emailTemplate={emailTemplate} />}
        </>
    );
};
