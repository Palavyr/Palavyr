import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { makeStyles, Typography } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { Upload } from "../responseConfiguration/uploadable/Upload";

const useStyles = makeStyles((theme) => ({
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));

export interface ImageReviewUploadProps {
    setImageRecords: SetState<FileLink[]>;
    numImages: number
}

export const ImageReviewUpload = ({ setImageRecords, numImages }: ImageReviewUploadProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const { setSuccessText, setSuccessOpen } = useContext(DashboardContext);

    const fileSave = async (rawFiles: File[]) => {
        const files = rawFiles.filter((x: File) => !isNullOrUndefinedOrWhitespace(x));
        if (files.length === 0) return;

        const formData = new FormData();

        let result: FileLink[];
        if ((files.length = 1)) {
            formData.append("files", files[0]);
            result = await repository.Configuration.Images.saveSingleImage(formData);
            setSuccessText("Image Uploaded");
        } else {
            files.forEach((file: File) => {
                formData.append("files", file);
            });
            result = await repository.Configuration.Images.saveMultipleImages(formData);
            setSuccessText("Images Uploaded");
        }
        const links = await repository.Configuration.Images.getImages();
        setImageRecords(links);
        setSuccessOpen(true);
    };

    return (
        <div className={cls.imageBlock}>
            <Upload
                dropzoneType="area"
                initialState={numImages === 0}
                handleFileSave={fileSave}
                summary="Upload a file"
                buttonText="Upload"
                uploadDetails={
                    <Typography align="center" variant="body1">
                        Upload an image, pdf, or other document you wish to share with your users during their palavyr.
                    </Typography>
                }
                acceptedFiles={["image/png", "image/jpg"]}
            />
        </div>
    );
};