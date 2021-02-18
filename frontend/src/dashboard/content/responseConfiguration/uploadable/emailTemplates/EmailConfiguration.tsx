import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Upload } from "../Upload";
import { ViewEmailTemplate } from "./ViewTemplate";
import { EmailEditor } from "./EmailEditor";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Divider, makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { Settings, VariableDetail } from "@Palavyr-Types";
import { EditorDetails } from "./EditorDetails";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { OsTypeToggle } from "../../areaSettings/enableAreas/OsTypeToggle";
import { EmailSubject } from "dashboard/content/settings/subject/EmailSubject";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const buttonText = "Add Email Template";
const summary = "Upload a new Email Response";

const useStyles = makeStyles(() => ({
    saveOrCancel: {
        marginTop: "2rem",
        textAlign: "right",
    },
}));

export const EmailConfiguration = () => {
    var client = new ApiClient();
    var cls = useStyles();
    let fileReader = new FileReader();
    let fallbackFileReader = new FileReader();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [emailTemplate, setEmailTemplate] = useState<string>("");
    const [fallbackEmailTemplate, setFallbackEmailTemplate] = useState<string>("");

    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>([]);

    const [modalState, setModalState] = useState<boolean>(false);
    const [fallbackModalState, setFallbackModalState] = useState<boolean>(false);

    const { setIsLoading } = React.useContext(DashboardContext);

    const toggleModal = () => {
        setModalState(!modalState);
    };
    const toggleFallbackModal = () => {
        setFallbackModalState(!fallbackModalState);
    };
    const [accordState, setAccordState] = useState<boolean>(false);
    const [fallbackAccordState, setFallbackAccordState] = useState<boolean>(false);

    const toggleAccord = () => {
        setAccordState(!accordState);
    };
    const toggleFallbackAccord = () => {
        setFallbackAccordState(!fallbackAccordState);
    };

    const [editorAccordstate, setEditorAccordState] = useState<boolean>(false);
    const [fallbackEditorAccordstate, setFallbackEditorAccordState] = useState<boolean>(false);

    const toggleEditorAccord = () => {
        setEditorAccordState(!editorAccordstate);
    };
    const toggleFallbackEditorAccord = () => {
        setFallbackEditorAccordState(!fallbackEditorAccordstate);
    };

    const [useAreaFallbackEmail, setUseAreaFallbackEmail] = useState<boolean | null>(null);
    const [settings, setSettings] = useState<Partial<Settings>>({ useAreaFallbackEmail: false });

    const handleFileRead = async () => {
        const content = fileReader.result;
        if (content && typeof content == "string") {
            const { data: emailTemplate } = await client.Configuration.Email.SaveAreaEmailTemplate(areaIdentifier, content);
            setEmailTemplate(emailTemplate);
            setModalState(false);
            setAccordState(false);
        } else {
            console.log("Failed to read file contents.");
        }
    };

    const handleFallbackFileRead = async () => {
        const content = fallbackFileReader.result;
        if (content && typeof content == "string") {
            const { data: emailTemplate } = await client.Configuration.Email.SaveAreaFallbackEmailTemplate(areaIdentifier, content);
            setFallbackEmailTemplate(emailTemplate);
            setFallbackModalState(false);
            setFallbackAccordState(false);
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

    const handleFallbackFileSave = (files: File[]) => {
        if (files[0] != null) {
            fallbackFileReader.onloadend = handleFallbackFileRead;
            fallbackFileReader.readAsText(files[0]);
        }
    };

    const saveEditorData = async () => {
        const { data: updatedEmailTemplate } = await client.Configuration.Email.SaveAreaEmailTemplate(areaIdentifier, emailTemplate);
        setEmailTemplate(updatedEmailTemplate);
        setModalState(false);
        setAccordState(false);
        return true;
    };

    const saveFallbackEditorData = async () => {
        const { data: updatedFallbackEmailTemplate } = await client.Configuration.Email.SaveAreaFallbackEmailTemplate(areaIdentifier, fallbackEmailTemplate);
        setFallbackEmailTemplate(updatedFallbackEmailTemplate);
        setFallbackModalState(false);
        setFallbackAccordState(false);
        return true;
    };

    const loadFallbackTemplate = useCallback(async () => {
        const { data: fallbackEmailTemplate } = await client.Configuration.Email.GetAreaFallbackEmailTemplate(areaIdentifier);
        setFallbackEmailTemplate(fallbackEmailTemplate);

        const { data: variableDetails } = await client.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
    }, [areaIdentifier]);

    const loadEmailTemplate = useCallback(async () => {
        const { data: emailTemplate } = await client.Configuration.Email.GetAreaEmailTemplate(areaIdentifier);
        const { data: variableDetails } = await client.Configuration.Email.GetVariableDetails();

        setVariableDetails(variableDetails);
        setEmailTemplate(emailTemplate);

        setLoaded(true);
        return () => {
            setLoaded(false);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const loadSettings = async () => {
        var { data: areaData } = await client.Area.GetArea(areaIdentifier);
        setSettings({
            ...settings,
            useAreaFallbackEmail: areaData.useAreaFallbackEmail,
        });
        setUseAreaFallbackEmail(areaData.useAreaFallbackEmail);
    };

    const loadAreaSubject = useCallback(async () => {
        const { data: emailSubject } = await client.Configuration.Email.GetAreaSubject(areaIdentifier);
        setAreaSubjectState(emailSubject);
    }, [areaIdentifier]);

    const loadFallbackAreaSubject = useCallback(
        async () => {
            const {data: fallbackSubject} = await client.Configuration.Email.GetAreaFallbackSubject(areaIdentifier);
            setAreaFallbackSubjectState(fallbackSubject);
            setIsLoading(false);
        },
        [areaIdentifier],
    )

    useEffect(() => {
        setIsLoading(true);
        loadEmailTemplate();
        loadFallbackTemplate();
        loadSettings();
        loadAreaSubject();
        loadFallbackAreaSubject();
        return () => {
            setLoaded(false);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const uploadDetails = (key: string) => {
        return <EditorDetails key={key} variableDetails={variableDetails} />;
    };

    const onUseAreaFallbackEmailToggle = async () => {
        const { data: updatedUsAreaFallback } = await client.Area.UpdateUseAreaFallbackEmail(!useAreaFallbackEmail, areaIdentifier);
        setUseAreaFallbackEmail(updatedUsAreaFallback);
    };

    const [areaSubjectState, setAreaSubjectState] = useState<string>("");
    const [subjectAccordState, setSubjectAccordState] = useState<boolean>(false);
    const [subjectModalState, setSubjectModalState] = useState<boolean>(false);
    const toggleSubjectModal = () => {
        setSubjectModalState(!subjectModalState);
    };
    const toggleSubjectAccord = () => {
        setSubjectAccordState(!subjectAccordState);
    };
    const onAreaSubjectChange = (event: any) => {
        setAreaSubjectState(event.target.value);
    };
    const onSaveAreaSubject = async () => {
        const { data: updatedSubject } = await client.Configuration.Email.SaveAreaSubject(areaIdentifier, areaSubjectState);
        setAreaSubjectState(updatedSubject);
        return true;
    };

    const [areaFallbackSubjectState, setAreaFallbackSubjectState] = useState<string>("");
    const [subjectFallbackAccordState, setSubjectFallbackAccordState] = useState<boolean>(false);
    const [subjectFallbackModalState, setSubjectFallbackModalState] = useState<boolean>(false);
    const toggleFallbackSubjectModal = () => {
        setSubjectFallbackModalState(!subjectFallbackModalState);
    };
    const toggleFallbackSubjectAccord = () => {
        setSubjectFallbackAccordState(!subjectFallbackAccordState);
    };
    const onAreaFallbackSubjectChange = (event: any) => {
        setAreaFallbackSubjectState(event.target.value);
    };
    const onSaveFallbackAreaSubject = async () => {
        const { data: updatedSubject } = await client.Configuration.Email.SaveAreaFallbackSubject(areaIdentifier, areaFallbackSubjectState);
        setAreaFallbackSubjectState(updatedSubject);
        return true;
    };

    return (
        <>
            <AreaConfigurationHeader title="Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this area." />
            {useAreaFallbackEmail !== null && <OsTypeToggle controlledState={useAreaFallbackEmail} onChange={onUseAreaFallbackEmailToggle} enabledLabel="Use Area Fallback Email" disabledLabel="Use General Fallback Email" />}
            <EmailSubject subject={areaSubjectState} onChange={onAreaSubjectChange} accordState={subjectAccordState} toggleAccord={toggleSubjectAccord} modalState={subjectModalState} toggleModal={toggleSubjectModal}>
                <div className={cls.saveOrCancel}>
                    <SaveOrCancel onSave={onSaveAreaSubject} onCancel={loadAreaSubject} useModal={true} />
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
                uploadDetails={() => uploadDetails("SecondDetails")}
                acceptedFiles={["text/html", "text/plain"]}
            />
            <EmailEditor uploadDetails={() => uploadDetails("firstDetails")} accordState={editorAccordstate} toggleAccord={toggleEditorAccord} setEmailTemplate={setEmailTemplate} emailTemplate={emailTemplate}>
                <div className={cls.saveOrCancel}>
                    <SaveOrCancel onSave={saveEditorData} onCancel={loadEmailTemplate} useModal={true} />
                </div>
            </EmailEditor>
            {loaded && <ViewEmailTemplate emailTemplate={emailTemplate} />}
            <Divider />
            {useAreaFallbackEmail && (
                <>
                    <AreaConfigurationHeader
                        title="Fallback Email Response"
                        subtitle="Use this editor to create a fallback email response that is specific to this area. For example, this email is sent if a 'Too Complicated' node is encountered during the chat."
                    />
                    <EmailSubject
                        subject={areaFallbackSubjectState}
                        onChange={onAreaFallbackSubjectChange}
                        accordState={subjectFallbackAccordState}
                        toggleAccord={toggleFallbackSubjectAccord}
                        modalState={subjectFallbackModalState}
                        toggleModal={toggleFallbackSubjectModal}
                    >
                        <div className={cls.saveOrCancel}>
                            <SaveOrCancel onSave={onSaveFallbackAreaSubject} onCancel={loadFallbackAreaSubject} useModal={true} />
                        </div>
                    </EmailSubject>
                    <Upload
                        modalState={fallbackModalState}
                        toggleModal={toggleFallbackModal}
                        accordState={fallbackAccordState}
                        toggleAccord={toggleFallbackAccord}
                        handleFileSave={handleFallbackFileSave}
                        buttonText={buttonText}
                        summary={summary}
                        uploadDetails={() => uploadDetails("ThirdDetails")}
                        acceptedFiles={["text/html", "text/plain"]}
                    />
                    <EmailEditor uploadDetails={() => uploadDetails("FinalDetails")} accordState={fallbackEditorAccordstate} toggleAccord={toggleFallbackEditorAccord} setEmailTemplate={setFallbackEmailTemplate} emailTemplate={fallbackEmailTemplate}>
                        <div className={cls.saveOrCancel}>
                            <SaveOrCancel onSave={saveFallbackEditorData} onCancel={loadFallbackTemplate} useModal={true} />
                        </div>
                    </EmailEditor>
                    {loaded && <ViewEmailTemplate emailTemplate={fallbackEmailTemplate} />}
                </>
            )}
        </>
    );
};
