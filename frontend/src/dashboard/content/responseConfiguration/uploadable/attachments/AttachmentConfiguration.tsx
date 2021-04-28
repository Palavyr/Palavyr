import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { FileLink } from "@Palavyr-Types";
import { Upload } from "../Upload";
import { AttachmentList } from "./AttachmentList";
import { AttachmentPreview } from "./AttachmentPreview";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const buttonText = "Add PDF Attachment";
const summary = "Upload a new PDF attachment to send with responses.";
const uploadDetails = <div className="alert alert-info">Use this dialog to upload attachments that will be sent standard with the response for this area.</div>;

export const AttachmentConfiguration = () => {
    var client = new ApiClient();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { setIsLoading } = React.useContext(DashboardContext);

    const [, setLoaded] = useState<boolean>(false);
    const [currentPreview, setCurrentPreview] = useState<FileLink | null>();
    const [attachmentList, setAttachmentList] = useState<Array<FileLink>>([]);

    const [modalState, setModalState] = useState(false);
    const toggleModal = () => {
        setModalState(!modalState);
    };

    const removeAttachment = async (fileId: string) => {
        var { data: filelinks } = await client.Configuration.Attachments.removeAttachment(areaIdentifier, fileId);
        setAttachmentList(filelinks);
    };

    const loadAttachments = useCallback(async () => {
        var { data: fileLinks } = await client.Configuration.Attachments.fetchAttachmentLinks(areaIdentifier);
        setAttachmentList(fileLinks);
        setLoaded(true);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        setIsLoading(true);

        loadAttachments();
        return () => {
            setLoaded(false);
            setCurrentPreview(null!);
        };
    }, [areaIdentifier, attachmentList.length, loadAttachments]);

    const handleFileSave = async (files: File[]) => {
        var formData = new FormData();

        if (files.length === 1) {
            formData.append("files", files[0]);
            const { data: fileLinks } = await client.Configuration.Attachments.saveSingleAttachment(areaIdentifier, formData);
            setAttachmentList(fileLinks);
        } else if (files.length > 1) {
            files.forEach((file: File) => {
                formData.append("files", file);
            });
            const { data: fileLinks } = await client.Configuration.Attachments.saveManyAttachments(areaIdentifier, formData);
            setAttachmentList(fileLinks);
        } else {
            const fileLinks = [];
            setAttachmentList(fileLinks);
        }
        setCurrentPreview(null);
    };

    return (
        <>
            <AreaConfigurationHeader title="Attachments" subtitle="Upload PDF and word documents you wish to send to your potential clients." />
            <Upload
                initialState={attachmentList.length === 0}
                modalState={modalState}
                toggleModal={toggleModal}
                handleFileSave={handleFileSave}
                buttonText={buttonText}
                summary={summary}
                uploadDetails={uploadDetails}
                acceptedFiles={["application/pdf"]}
            />
            <AttachmentList fileList={attachmentList} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment} />
            {currentPreview ? <AttachmentPreview preview={currentPreview} /> : null}
        </>
    );
};
