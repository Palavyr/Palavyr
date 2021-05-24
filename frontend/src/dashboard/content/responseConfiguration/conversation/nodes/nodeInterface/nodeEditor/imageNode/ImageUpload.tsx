import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { Upload } from "dashboard/content/responseConfiguration/uploadable/Upload";
import React, { useContext } from "react";
import { useState } from "react";
import { SelectFromExistingImages } from "./SelectFromExistingImages";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

export interface ImageUploadProps {
    node: ConvoNode;
    setImageName: SetState<string>;
    setImageLink: SetState<string>;
    currentImageId: string;
    initialState?: boolean;
}

const useStyles = makeStyles((theme) => ({
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));

export const NodeImageUpload = ({ node, setImageName, setImageLink, currentImageId, initialState = false }: ImageUploadProps) => {
    const { setIsLoading, setSuccessOpen, setSuccessText } = useContext(DashboardContext);

    const repository = new PalavyrRepository();
    const cls = useStyles();
    const [modal, setModal] = useState(false);

    const toggleModal = () => {
        setModal(!modal);
    };

    const fileSave = async (files: File[]) => {
        setIsLoading(true);
        const formData = new FormData();

        let result: FileLink[];
        if ((files.length = 1)) {
            formData.append("files", files[0]);
            result = await repository.Configuration.Images.saveSingleImage(formData);
            setSuccessText("Image Uploaded");
        } else if (files.length > 1) {
            files.forEach((file: File) => {
                formData.append("files", file);
            });
            result = await repository.Configuration.Images.saveMultipleImages(formData);
            setSuccessText("Images Uploaded");
        } else {
            return;
        }

        await repository.Configuration.Images.savePreExistingImage(result[0].fileId, node.nodeId);
        setIsLoading(false);
        setSuccessOpen(true);
    };

    return (
        <>
            <div className={cls.imageBlock}>
                <SelectFromExistingImages node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} />
            </div>
            <Divider />
            <div className={cls.imageBlock}>
                <Upload
                    dropzoneType="area"
                    initialState={initialState}
                    modalState={modal}
                    toggleModal={() => toggleModal()}
                    handleFileSave={(files: File[]) => fileSave(files)}
                    summary="Upload a file."
                    buttonText="Upload"
                    uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                    acceptedFiles={["image/png", "image/jpg"]}
                />
            </div>
        </>
    );
};
