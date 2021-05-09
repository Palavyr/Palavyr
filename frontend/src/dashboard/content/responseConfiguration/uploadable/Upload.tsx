import React from "react";
import { Button, makeStyles } from "@material-ui/core";
import { DropzoneDialog } from "material-ui-dropzone";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { getAnchorOrigin } from "@common/components/PalavyrSnackbar";

export interface IUploadAttachment {
    initialState?: boolean;
    modalState: boolean;
    toggleModal(): void;
    handleFileSave(files: File[]): void;
    summary: string;
    buttonText: string;
    uploadDetails: JSX.Element;
    acceptedFiles: Array<string>;
}

export type FileUpload = Blob & {
    readonly lastModified: number;
    readonly name: string;
};

const useStyles = makeStyles((theme) => ({
    snackbarProps: {
        color: theme.palette.common.black,
    },
}));

export const Upload = ({ initialState = false, modalState, toggleModal, handleFileSave, summary, buttonText, uploadDetails, acceptedFiles }: IUploadAttachment) => {
    const onSave = (files: File[], e: any) => {
        handleFileSave(files);
        toggleModal();
    };
    const cls = useStyles();

    const anchorOrigin = getAnchorOrigin("br");
    return (
        <PalavyrAccordian title={summary} initialState={initialState}>
            <div>
                {uploadDetails}
                <br></br>
                <Button onClick={toggleModal} variant="contained" color="primary">
                    {buttonText}
                </Button>
            </div>
            <DropzoneDialog
                alertSnackbarProps={{ anchorOrigin, classes: { root: cls.snackbarProps } }}
                open={modalState}
                onSave={onSave}
                useChipsForPreview
                acceptedFiles={acceptedFiles}
                showPreviews={true}
                maxFileSize={2000000}
                onClose={toggleModal}
            />
        </PalavyrAccordian>
    );
};
