import React from "react"
import { Accordion, AccordionSummary, Typography, AccordionDetails, Button, makeStyles } from "@material-ui/core"
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { DropzoneDialog } from 'material-ui-dropzone';


export interface IUploadAttachment {
    uploadDetails: () => React.ReactNode;
    areaIdentifier: string;
    buttonText: string;
    summary: string;
    handleFileSave: any;
    accordState: any;
    toggleAccord: any;
    modalState: any;
    toggleModal: any;
    acceptedFiles: Array<string>;
}

export type FileUpload = Blob & {
    readonly lastModified: number;
    readonly name: string;
}

const useStyles = makeStyles(theme => ({
    heading: {

    }
}))


export const Upload = ({ accordState, toggleAccord, modalState, toggleModal, handleFileSave, summary, buttonText, areaIdentifier, uploadDetails, acceptedFiles }: IUploadAttachment) => {

    const classes = useStyles();
    return (
        <Accordion expanded={accordState} >
            <AccordionSummary onClick={toggleAccord} expandIcon={<ExpandMoreIcon />} aria-controls="panel2a-content" id="panel2a-header">
                <Typography className={classes.heading}>
                    <strong>{summary}</strong>
                </Typography>
            </AccordionSummary>
            <AccordionDetails>
                <div>
                    {uploadDetails()}
                    <br></br>
                    <Button onClick={toggleModal} variant="contained" color="primary">
                        {buttonText}
                    </Button>
                </div>
                <DropzoneDialog
                    open={modalState}
                    onSave={(files: File[], e) => {
                        handleFileSave(files)
                        toggleModal()
                    }}
                    acceptedFiles={acceptedFiles}
                    showPreviews={true}
                    maxFileSize={2000000}
                    onClose={toggleModal}
                />
            </AccordionDetails>
        </Accordion>
    )
}