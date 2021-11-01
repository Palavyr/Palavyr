import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Divider, Typography } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import { Upload } from "dashboard/content/responseConfiguration/uploadable/Upload";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { IPalavyrNode } from "../../Contracts";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { SelectFromExistingImages } from "./SelectFromExistingImages";

interface ImageUploadProps {
    currentNode: IPalavyrNode;
    nodeId: string;
    closeEditor: () => void;
    currentImageId: string;
    setCurrentImageId: SetState<string>;
    setImageLink: SetState<string>;
    setImageName: SetState<string>;
    initialState: boolean;
    repository: PalavyrRepository;
    imageId?: string;
}

export const ImageUpload = ({ setCurrentImageId, currentNode, nodeId, imageId, closeEditor, currentImageId, setImageLink, setImageName, repository, initialState = false }: ImageUploadProps) => {
    const cls = useNodeInterfaceStyles({});
    const history = useHistory();
    const [uploadModal, setUploadModal] = useState(false);

    const { setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
    useEffect(() => {
        if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
            history.push("/dashboard/please-subscribe");
        }
    }, [planTypeMeta]);

    const toggleModal = () => {
        setUploadModal(!uploadModal);
    };

    const fileSave = async (files: File[]) => {
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

        await repository.Configuration.Images.savePreExistingImage(result[0].fileId, nodeId);
        setSuccessOpen(true);
        closeEditor();
    };

    return (
        <>
            {currentNode && (
                <>
                    <div className={cls.imageBlock}>
                        <SelectFromExistingImages
                            nodeId={nodeId}
                            imageId={imageId}
                            repository={repository}
                            setImageId={(imageId: string) => {
                                currentNode.imageId = imageId;
                                setCurrentImageId(imageId);
                                currentNode.UpdateTree();
                            }}
                            currentImageId={currentImageId}
                            setImageLink={setImageLink}
                            setImageName={setImageName}
                        />
                    </div>
                    <Divider />
                    <div className={cls.imageBlock}>
                        {planTypeMeta && planTypeMeta.allowedImageUpload && (
                            <Upload
                                dropzoneType="area"
                                initialState={initialState}
                                modalState={uploadModal}
                                toggleModal={() => toggleModal()}
                                handleFileSave={(files: File[]) => fileSave(files)}
                                summary="Upload a file."
                                buttonText="Upload"
                                uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                                acceptedFiles={["image/png", "image/jpg", "image/gif"]}
                            />
                        )}
                    </div>
                </>
            )}
        </>
    );
};
