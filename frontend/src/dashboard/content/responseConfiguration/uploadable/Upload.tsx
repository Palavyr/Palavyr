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
        fontWeight: "bold"
    },
    accordian: {
        width: "100%",
    },
    accordianHead: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
    accordianBody: {
        backgroundColor: "#C7ECEE",
    }

}))


export const Upload = ({ accordState, toggleAccord, modalState, toggleModal, handleFileSave, summary, buttonText, areaIdentifier, uploadDetails, acceptedFiles }: IUploadAttachment) => {

    const classes = useStyles();
    return (
        <Accordion className={classes.accordian} expanded={accordState} >
            <AccordionSummary className={classes.accordianHead} onClick={toggleAccord} expandIcon={<ExpandMoreIcon />} aria-controls="panel2a-content" id="panel2a-header">
                <Typography className={classes.heading}>
                    {summary}
                </Typography>
            </AccordionSummary>
            <AccordionDetails className={classes.accordianBody}>
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
                    useChipsForPreview
                    acceptedFiles={acceptedFiles}
                    showPreviews={true}
                    maxFileSize={2000000}
                    onClose={toggleModal}
                />
            </AccordionDetails>
        </Accordion>
    )
}