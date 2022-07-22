import React from "react";
import { Button, makeStyles, Typography } from "@material-ui/core";
import { DropzoneArea, DropzoneDialog, FileObject } from "material-ui-dropzone";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { getAnchorOrigin } from "@common/components/PalavyrSnackbar";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";

export interface IUploadAttachment {
    initialAccordianState?: boolean;
    modalState?: boolean;
    toggleModal?(): void;
    handleFileSave(files: File[]): void;
    summary: string;
    buttonText: string;
    uploadDetails: string | JSX.Element;
    acceptedFiles: Array<string>;
    dropzoneType?: "dialog" | "intent";
    disableButton?: boolean;
    disable?: boolean;
}

export type FileUpload = Blob & {
    readonly lastModified: number;
    readonly name: string;
};

const useStyles = makeStyles(theme => ({
    snackbarProps: {
        color: theme.palette.common.black,
    },
}));

export const Upload = ({ dropzoneType = "dialog", initialAccordianState = false, modalState, toggleModal, handleFileSave, summary, buttonText, uploadDetails, acceptedFiles, disable }: IUploadAttachment) => {
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
        <>
            <PalavyrAccordian title={summary} initialState={initialAccordianState} disable={disable}>
                <div>
                    {uploadDetails}
                    <br></br>
                    {dropzoneType === "dialog" && (
                        <>
                            <Button onClick={toggleModal} variant="contained" color="primary" disabled={disable}>
                                {buttonText}
                            </Button>
                            {disable && (
                                <Typography display="block">
                                    <strong>Upgrade your subscription to enable uploads</strong>
                                </Typography>
                            )}
                        </>
                    )}
                </div>
                {dropzoneType === "dialog" && !disable ? (
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
                    <>
                        {!disable && (
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
                    </>
                )}
            </PalavyrAccordian>
            {disable && (
                <Typography display="block">
                    <strong>Upgrade your subscription to enable uploads</strong>
                </Typography>
            )}
        </>
    );
};
