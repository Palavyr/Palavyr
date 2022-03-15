import React, { useState, useCallback, useEffect } from "react";
import { Upload } from "../Upload";
import { AttachmentList } from "./AttachmentList";
import { AttachmentPreview } from "./AttachmentPreview";
import { useParams } from "react-router-dom";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { useContext } from "react";
import { ACCEPTED_FILES } from "@constants";
import { FileAssetResource } from "@Palavyr-Types";

const summary = "Upload a new PDF attachment to send with responses.";
const uploadDetails = <div className="alert alert-info">Use this dialog to upload attachments that will be sent standard with the response for this area.</div>;

export const AttachmentConfiguration = () => {
    const { repository } = useContext(DashboardContext);

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);

    const [, setLoaded] = useState<boolean>(false);
    const [currentPreview, setCurrentPreview] = useState<FileAssetResource | null>();
    const [attachmentList, setAttachmentList] = useState<FileAssetResource[]>([]);

    const [modalState, setModalState] = useState(false);
    const toggleModal = () => {
        setModalState(!modalState);
    };

    const removeAttachment = async (fileId: string) => {
        const fileAssetResources = await repository.Configuration.Attachments.DeleteAttachment(areaIdentifier, fileId);
        setAttachmentList(fileAssetResources);
        setSuccessText("Attachment Removed");
        setSuccessOpen(true);
    };

    const loadAttachments = useCallback(async () => {
        const fileAssetResources = await repository.Configuration.Attachments.GetAttachments(areaIdentifier);
        setAttachmentList(fileAssetResources);
        setLoaded(true);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        loadAttachments();
        return () => {
            setLoaded(false);
            setCurrentPreview(null);
        };
    }, [areaIdentifier, attachmentList.length, loadAttachments]);

    const handleFileSave = async (files: File[]) => {
        if (files.length === 0) return;

        var formData = new FormData();
        files.forEach((file: File) => formData.append("files", file));

        const fileAssetResources = await repository.Configuration.Attachments.UploadAttachments(areaIdentifier, formData);
        setAttachmentList(fileAssetResources);
        setSuccessText("Attachment Uploaded");
        setSuccessOpen(true);
        setCurrentPreview(null);
    };

    const shouldDisableUploadButton = () => {
        return planTypeMeta && attachmentList.length >= planTypeMeta.allowedAttachments;
    };

    return (
        <>
            <HeaderStrip title="Attachments" subtitle="Upload files you wish to send to your potential clients." />
            <Upload
                initialState={attachmentList.length === 0}
                modalState={modalState}
                toggleModal={toggleModal}
                handleFileSave={handleFileSave}
                buttonText="Add Attachment"
                summary={summary}
                uploadDetails={uploadDetails}
                acceptedFiles={ACCEPTED_FILES}
                disableButton={shouldDisableUploadButton()}
            />
            <AttachmentList fileList={attachmentList} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment} />
            {currentPreview ? <AttachmentPreview preview={currentPreview} /> : null}
        </>
    );
};
