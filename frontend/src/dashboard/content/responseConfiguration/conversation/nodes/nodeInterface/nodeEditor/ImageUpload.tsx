import { Typography } from "@material-ui/core";
import { ConvoNode } from "@Palavyr-Types";
import { Upload } from "dashboard/content/responseConfiguration/uploadable/Upload";
import React from "react";
import { useState } from "react";

export interface ImageUploadProps {
    node: ConvoNode;
}

// initialState?: boolean;
// modalState: boolean;
// toggleModal(): void;
// handleFileSave(files: File[]): void;
// summary: string;
// buttonText: string;
// uploadDetails: JSX.Element;
// acceptedFiles: Array<string>;

export const ImageUpload = ({ node }: ImageUploadProps) => {
    // case 1: no image
    // case 2: image exists

    const [modal, setModal] = useState(false)

    const toggleModal = () => {
        setModal(!modal);
    }

    const fileSave = (files: File[]) => {
        // TODO: complete this -- create the server endpoints to handle th file save
        // and don't forget to delete the removed ones uplong convo edit.
    }

    return (
        <>
            <Typography variant="h5">Image Node</Typography>
            {node.imageKey !== "" && <Typography variant="h5">Upload an image</Typography>}
            <Upload
                initialState={false}
                modalState={modal}
                toggleModal={() => toggleModal()}
                handleFileSave={(files: File[]) => fileSave(files)}
                summary="This is a summary"
                buttonText="Upload"
                uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                acceptedFiles={["jpeg", "png", "mp4", "small videos?", "pdfs", "wordDocx"]}
            />
        </>
    );
};
