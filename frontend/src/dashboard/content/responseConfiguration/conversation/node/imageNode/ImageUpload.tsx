import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Divider, makeStyles, Typography } from "@material-ui/core";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { Upload } from "dashboard/content/responseConfiguration/uploadable/Upload";
import React, { useContext, useEffect } from "react";
import { useState } from "react";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { useHistory } from "react-router-dom";

export interface ImageUploadProps {
    node: ConvoNode;
    setImageName: SetState<string>;
    setImageLink: SetState<string>;
    currentImageId: string;
    setModalState: SetState<boolean>;
    initialState?: boolean;
}

const useStyles = makeStyles((theme) => ({
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));

export const NodeImageUpload = ({ node, setImageName, setImageLink, currentImageId, initialState = false, setModalState }: ImageUploadProps) => {
    const cls = useStyles();
    const { setIsLoading, setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
    const history = useHistory();

    useEffect(() => {
        if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
            history.push("/dashboard/please-subscribe");
        }
    }, [planTypeMeta]);

    const repository = new PalavyrRepository();
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
        setModalState(false);
    };

    return (
        <>
            <div className={cls.imageBlock}>
                {/* <SelectFromExistingImages node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} /> */}
            </div>
            <Divider />
            <div className={cls.imageBlock}>
                {planTypeMeta && planTypeMeta.allowedImageUpload && (
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
                )}
            </div>
        </>
    );
};
