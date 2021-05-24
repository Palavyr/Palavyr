import React from "react";
import { Button, makeStyles } from "@material-ui/core";
import { DropzoneArea, DropzoneDialog, FileObject } from "material-ui-dropzone";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { getAnchorOrigin } from "@common/components/PalavyrSnackbar";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";

export interface IUploadAttachment {
    initialState?: boolean;
    modalState?: boolean;
    toggleModal?(): void;
    handleFileSave(files: File[]): void;
    summary: string;
    buttonText: string;
    uploadDetails: JSX.Element;
    acceptedFiles: Array<string>;
    dropzoneType?: "dialog" | "area";
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

export const Upload = ({ dropzoneType = "dialog", initialState = false, modalState, toggleModal, handleFileSave, summary, buttonText, uploadDetails, acceptedFiles }: IUploadAttachment) => {
    const onSave = (rawFiles: File[], e: any) => {
        const files = rawFiles.filter((x: File) => !isNullOrUndefinedOrWhitespace(x));
        if (files.length === 0) return;

        onChange(files);
        if (toggleModal) {
            toggleModal();
        }
    };

    const onChange = (rawFiles: File[]) => {
        const files = rawFiles.filter((x: File) => !isNullOrUndefinedOrWhitespace(x));
        if (files.length === 0) return;

        handleFileSave(files);
    };

    const cls = useStyles();

    const anchorOrigin = getAnchorOrigin("br");
    return (
        <PalavyrAccordian title={summary} initialState={initialState}>
            <div>
                {uploadDetails}
                <br></br>
                {dropzoneType === "dialog" && (
                    <Button onClick={toggleModal} variant="contained" color="primary">
                        {buttonText}
                    </Button>
                )}
            </div>
            {dropzoneType === "dialog" ? (
                <DropzoneDialog
                    alertSnackbarProps={{ anchorOrigin, classes: { root: cls.snackbarProps } }}
                    open={modalState}
                    onSave={onSave}
                    filesLimit={1}
                    useChipsForPreview
                    acceptedFiles={acceptedFiles}
                    showPreviews={true}
                    maxFileSize={2000000}
                    onClose={toggleModal}
                />
            ) : (
                <DropzoneArea
                    previewText=""
                    showAlerts={false}
                    getPreviewIcon={(_: FileObject) => <></>}
                    showPreviewsInDropzone={false}
                    alertSnackbarProps={{ anchorOrigin, classes: { root: cls.snackbarProps } }}
                    clearOnUnmount
                    filesLimit={1}
                    onChange={onChange}
                    acceptedFiles={acceptedFiles}
                    showPreviews={false}
                    maxFileSize={2000000}
                />
            )}
        </PalavyrAccordian>
    );
};
