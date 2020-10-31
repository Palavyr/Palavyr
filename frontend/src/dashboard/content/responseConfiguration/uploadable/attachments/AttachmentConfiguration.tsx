import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { FileLink } from "@Palavyr-Types";
import { Upload } from "../Upload";
import { AttachmentList } from "./AttachmentList";
import { AttachmentPreview } from "./AttachmentPreview";
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp";


const buttonText = "Add PDF Attachment";
const summary = "Upload a new PDF attachment to send with responses.";
const uploadDetails = () => {
    return (
        <div className="alert alert-info">
            Use this dialog to upload attachments that will be sent standard with the response for this area.
        </div>

    )
}

interface IAttachmentConfiguration {
    areaIdentifier: string;
}


export const AttachmentConfiguration = ({ areaIdentifier }: IAttachmentConfiguration) => {
    var client = new ApiClient();

    const [, setLoaded] = useState<boolean>(false);
    const [currentPreview, setCurrentPreview] = useState<FileLink>();
    const [attachmentList, setAttachmentList] = useState<Array<FileLink>>([]);

    const [modalState, setModalState] = useState(false);
    const toggleModal = () => {
        setModalState(!modalState);
    }

    const [accordState, setAccordState] = useState(false);
    const toggleAccord = () => {
        setAccordState(!accordState)
    }
    const removeAttachment = async (fileId: string) => {
        var res = await client.Configuration.Attachments.removeAttachment(areaIdentifier, fileId)
        setAttachmentList(res.data);
    }

    const loadAttachments = useCallback(async () => {
        var res = await client.Configuration.Attachments.fetchAttachmentLinks(areaIdentifier);
        setAttachmentList(res.data);
        setLoaded(true);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        loadAttachments()
        if (attachmentList.length === 0) {
            setAccordState(true)
        }
        return () => {
            setLoaded(false);
            setCurrentPreview(null!)
        }
    }, [areaIdentifier, attachmentList.length, loadAttachments])

    const handleFileSave = async (files: File[]) => {
        var formData = new FormData();
        let res;
        if (files.length === 1) {
            formData.append('files', files[0])
            res = await client.Configuration.Attachments.saveSingleAttachment(areaIdentifier, formData);
        } else if (files.length > 1) {
            files.forEach((file: File) => {
                formData.append('files', file)
            })
            res = await client.Configuration.Attachments.saveManyAttachments(areaIdentifier, formData);
        } else {
            res = [];
        }
        setAttachmentList(res.data)
        setCurrentPreview(null!);
    }

    return (
        <>
            <AttachmentsHelp />
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
                acceptedFiles={['application/pdf']}
            />
            <AttachmentList fileList={attachmentList} setCurrentPreview={setCurrentPreview} removeAttachment={removeAttachment} />
            {currentPreview ? <AttachmentPreview preview={currentPreview} /> : null}
        </>
    );
};
