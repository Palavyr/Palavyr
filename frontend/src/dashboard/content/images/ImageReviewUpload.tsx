import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { makeStyles, Typography } from "@material-ui/core";
import { FileLink, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SessionStorage } from "localStorage/sessionStorage";
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
}

export const ImageReviewUpload = ({ setImageRecords }: ImageReviewUploadProps) => {
    const cls = useStyles();
    const repository = new PalavyrRepository();
    const { setIsLoading, setSuccessText, setSuccessOpen } = useContext(DashboardContext);

    const fileSave = async (rawFiles: File[]) => {
        const files = rawFiles.filter((x: File) => !isNullOrUndefinedOrWhitespace(x));
        if (files.length === 0) return;

        setIsLoading(true);
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
        setIsLoading(false);
        setSuccessOpen(true);
    };

    return (
        <div className={cls.imageBlock}>
            <Upload
                dropzoneType="area"
                initialState={false}
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
