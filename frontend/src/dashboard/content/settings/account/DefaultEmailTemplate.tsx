import { ApiClient } from "@api-client/Client";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { makeStyles, Typography } from "@material-ui/core";
import { PurchaseTypes, VariableDetail } from "@Palavyr-Types";
import { EditorDetails } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/EditorDetails";
import { EmailEditor } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailEditor";
import { ViewEmailTemplate } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/ViewTemplate";
import { Upload } from "dashboard/content/responseConfiguration/uploadable/Upload";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import React, { useCallback, useState } from "react";
import { useEffect } from "react";
import { EmailSubject } from "../subject/EmailSubject";

const buttonText = "Add Email Template";
const summary = "Upload a new Email Response";

const useStyles = makeStyles(() => ({
    saveOrCancel: {
        marginTop: "2rem",
        textAlign: "right",
    },
    root: {
        width: "100%",
        // flexGrow: 1,
        height: "100%",
    },
}));

export const DefaultEmailTemplate = () => {
    const client = new ApiClient();
    let fileReader = new FileReader();

    const { subscription } = React.useContext(DashboardContext);

    const [loaded, setLoaded] = useState<boolean>(false);

    const cls = useStyles();

    const [emailTemplate, setEmailTemplate] = useState<string>("");
    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>([]);
    const [modalState, setModalState] = useState<boolean>(false);
    const toggleModal = () => setModalState(!modalState);

    const [accordState, setAccordState] = useState<boolean>(false);
    const toggleAccord = () => setAccordState(!accordState);

    const [editorAccordstate, setEditorAccordState] = useState<boolean>(false);
    const toggleEditorAccord = () => setEditorAccordState(!editorAccordstate);

    const [subjectState, setSubjectState] = useState<string>("");
    const [subjectAccordState, setSubjectAccordState] = useState<boolean>(false);
    const [subjectModalState, setSubjectModalState] = useState<boolean>(false);

    const toggleSubjectModal = () => setSubjectModalState(!subjectModalState);
    const toggleSubjectAccord = () => setSubjectAccordState(!subjectAccordState);
    const onSubjectChange = (event: { target: { value: React.SetStateAction<string> } }) => setSubjectState(event.target.value);

    const handleFileRead = async () => {
        const content = fileReader.result;
        if (content && typeof content == "string") {
            const { data: emailTemplate } = await client.Configuration.Email.SaveDefaultFallbackEmailTemplate(content);
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
        const { data: updatedEmailTemplate } = await client.Configuration.Email.SaveDefaultFallbackEmailTemplate(emailTemplate);
        setEmailTemplate(updatedEmailTemplate);
        setModalState(false);
        setAccordState(false);
        return true;
    };

    const loadEmailTemplate = useCallback(async () => {
        const { data: emailTemplate } = await client.Configuration.Email.GetDefaultFallbackEmailTemplate();
        const { data: variableDetails } = await client.Configuration.Email.GetVariableDetails();

        setVariableDetails(variableDetails);
        setEmailTemplate(emailTemplate);

        setLoaded(true);
        return () => {
            setLoaded(false);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const uploadDetails = (key: string) => {
        return <EditorDetails key={key} variableDetails={variableDetails} />;
    };

    const saveDefaultEmailSubject = async () => {
        const { data: updatedSubject } = await client.Configuration.Email.SaveDefaultFallbackSubject(subjectState);
        setSubjectState(updatedSubject);
        return true;
    };

    const loadDefaultEmailSubject = async () => {
        const { data: currentSubject } = await client.Configuration.Email.GetDefaultFallbackSubject();
        setSubjectState(currentSubject);
        return true;
    };

    useEffect(() => {
        loadEmailTemplate();
        loadDefaultEmailSubject();
    }, []);

    return (
        <div className={cls.root}>
            <AlignCenter>
                <AreaConfigurationHeader title="Default Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this area." />
            </AlignCenter>
            <EmailSubject subject={subjectState} onChange={onSubjectChange} accordState={subjectAccordState} toggleAccord={toggleSubjectAccord} modalState={subjectModalState} toggleModal={toggleSubjectModal}>
                <div className={cls.saveOrCancel}>
                    <SaveOrCancel onSave={saveDefaultEmailSubject} onCancel={loadDefaultEmailSubject} useModal={true} />
                </div>
            </EmailSubject>
            <Upload
                modalState={modalState}
                toggleModal={toggleModal}
                accordState={accordState}
                toggleAccord={toggleAccord}
                handleFileSave={handleFileSave}
                buttonText={buttonText}
                summary={summary}
                uploadDetails={() => uploadDetails("minorDetails")}
                acceptedFiles={["text/html", "text/plain"]}
            />
            {subscription === PurchaseTypes.Premium || subscription === PurchaseTypes.Pro ? (
                <EmailEditor uploadDetails={() => uploadDetails("otherDetails")} accordState={editorAccordstate} toggleAccord={toggleEditorAccord} setEmailTemplate={setEmailTemplate} emailTemplate={emailTemplate}>
                    <div className={cls.saveOrCancel}>
                        <SaveOrCancel onSave={saveEditorData} onCancel={loadEmailTemplate} useModal={true} />
                    </div>
                </EmailEditor>
            ) : (
                <Typography style={{ padding: "0.6rem", fontWeight: "bold"}}> Subscribe to get access to the inline email editor </Typography>
            )}
            {loaded && <ViewEmailTemplate emailTemplate={emailTemplate} />}
        </div>
    );
};
