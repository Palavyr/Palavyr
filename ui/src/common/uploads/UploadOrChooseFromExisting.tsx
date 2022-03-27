import { Divider, makeStyles, Typography } from "@material-ui/core";
import { FileAssetResource } from "@Palavyr-Types";
import { Upload } from "frontend/dashboard/content/responseConfiguration/uploadable/Upload";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useState } from "react";
import { ACCEPTED_FILES } from "@constants";
import { SelectFromExistingFileAssets } from "./SelectFromExistingFileAssets";

export interface UploadOrChooseFromExistingProps {
    handleFileSave: (files: File[]) => void;
    currentFileAssetId?: string | null;
    onSelectChange: (_: any, option: FileAssetResource) => void;
    initialAccordianState?: boolean;
    summary?: string;
    uploadDetails?: string | JSX.Element;
    disable?: boolean;
    excludableFileAssets?: FileAssetResource[];
}

export const UploadOrChooseFromExisting = ({
    currentFileAssetId,
    handleFileSave,
    onSelectChange,
    initialAccordianState = false,
    excludableFileAssets,
    summary,
    uploadDetails,
    disable,
}: UploadOrChooseFromExistingProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [uploadModal, setUploadModal] = useState(false);

    const toggleModal = () => {
        setUploadModal(!uploadModal);
    };

    return (
        <>
            <div className={cls.imageBlock}>
                <SelectFromExistingFileAssets disable={disable} repository={repository} excludableFileAssets={excludableFileAssets} onSelectChange={onSelectChange} currentFileAssetId={currentFileAssetId} />
            </div>
            <Divider />
            <div className={cls.imageBlock}>
                <Upload
                    dropzoneType="area"
                    initialAccordianState={initialAccordianState}
                    modalState={uploadModal}
                    toggleModal={() => toggleModal()}
                    handleFileSave={handleFileSave}
                    summary={summary ?? "Upload a file."}
                    buttonText={"Upload"}
                    uploadDetails={uploadDetails ?? <Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                    acceptedFiles={ACCEPTED_FILES}
                    disable={disable}
                />
            </div>
        </>
    );
};

const useStyles = makeStyles(theme => ({
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));
