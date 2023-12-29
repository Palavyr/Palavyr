import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ACCEPTED_FILES } from "@constants";
import { makeStyles, Typography } from "@material-ui/core";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { Upload } from "../responseConfiguration/uploadable/Upload";


const useStyles = makeStyles<{}>((theme: any) => ({
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));

export interface FileAssetUploadProps {
    setFileAssets: SetState<FileAssetResource[]>;
    numImages: number;
}

export const FileAssetUpload = ({ setFileAssets, numImages }: FileAssetUploadProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const { setSuccessText, setSuccessOpen } = useContext(DashboardContext);

    const handleFileSave = async (rawFiles: File[]) => {
        const files = rawFiles.filter((x: File) => !isNullOrUndefinedOrWhitespace(x));
        if (files.length === 0) return;

        const formData = new FormData();

        files.forEach((file: File) => {
            formData.append("files", file);
        });

        await repository.Configuration.FileAssets.UploadFileAssets(formData);
        setSuccessText("File/s Uploaded");
        const links = await repository.Configuration.FileAssets.GetFileAssets();
        setFileAssets(links);
        setSuccessOpen(true);
    };

    return (
        <div className={cls.imageBlock}>
            <Upload
                dropzoneType="intent"
                initialAccordianState={numImages === 0}
                handleFileSave={handleFileSave}
                summary="Upload a file"
                buttonText="Upload"
                uploadDetails={
                    <Typography align="center" variant="body1">
                        Upload an image, pdf, or other document you wish to share with your users during their chat.
                    </Typography>
                }
                acceptedFiles={ACCEPTED_FILES}
            />
        </div>
    );
};
