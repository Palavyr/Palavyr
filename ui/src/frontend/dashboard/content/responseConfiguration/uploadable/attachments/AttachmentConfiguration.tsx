import React, { useState, useCallback, useEffect } from "react";
import { AttachmentList } from "./AttachmentList";
import { AttachmentPreview } from "./AttachmentPreview";
import { useParams } from "react-router-dom";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { useContext } from "react";
import { FileAssetResource } from "@Palavyr-Types";
import { UploadOrSelectFromExisting } from "@common/uploads/UploadOrChooseFromExisting";
import { cloneDeep } from "lodash";

const summary = "Upload";
const uploadDetails = <div className="alert alert-info">Use this dialog to upload attachments that will be sent standard with the response for this intent.</div>;

export const AttachmentConfiguration = () => {
    const { repository } = useContext(DashboardContext);

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);

    const [, setLoaded] = useState<boolean>(false);
    const [currentPreview, setCurrentPreview] = useState<FileAssetResource | null>();
    const [attachmentList, setAttachmentList] = useState<FileAssetResource[]>([]);
    const [currentFileAssetId, setCurrentFileAssetId] = useState<string>("");

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
        const shouldDisable = planTypeMeta && attachmentList.length >= planTypeMeta.allowedAttachments;
        return shouldDisable;
    };

    const onSelectChange = async (_: any, option: FileAssetResource) => {
        setCurrentFileAssetId(option.fileId);
        await repository.Configuration.FileAssets.LinkFileAssetToIntent(option.fileId, areaIdentifier);
        setAttachmentList(cloneDeep([...attachmentList.filter(x => x.fileId != option.fileId), option]));
        setSuccessText("Attachment Added");
        setSuccessOpen(true);
        setCurrentPreview(null);
    };

    return (
        <>
            <HeaderStrip title="Attachments" subtitle="Upload files you wish to send to your potential clients." />
            <UploadOrSelectFromExisting
                excludableFileAssets={attachmentList}
                currentFileAssetId={currentFileAssetId}
                disable={shouldDisableUploadButton()}
                handleFileSave={handleFileSave}
                onSelectChange={onSelectChange}
                summary={summary}
                uploadDetails={uploadDetails}
            />
            <AttachmentList fileList={attachmentList} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment} />
            {currentPreview ? <AttachmentPreview preview={currentPreview} /> : null}
        </>
    );
};
