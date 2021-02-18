import React from "react";
import { Accordion, AccordionSummary, Typography, AccordionDetails, Button, makeStyles, TextField, Divider } from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";

export interface IUploadAttachment {
    onChange(event: { target: { value: React.SetStateAction<string> } }): void;
    subject: string;
    accordState: boolean;
    toggleAccord(): void;
    modalState: boolean;
    toggleModal(): void;
    children: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    heading: {
        fontWeight: "bold",
    },
    textdiv: {},
    textfield: {},
    table: {
        width: "100%",
        marginTop: "0.3rem",
        marginBottom: "0.3rem",
    },
    accordian: {
        width: "100%",
    },
    accordianHead: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
    accordianBody: {
        backgroundColor: "#C7ECEE",
        width: "100%",
    },
}));

export const EmailSubject = ({ onChange, subject, accordState, toggleAccord, modalState, toggleModal, children }: IUploadAttachment) => {
    const cls = useStyles();
    return (
        <>
            <Accordion className={cls.accordian} expanded={accordState}>
                <AccordionSummary className={cls.accordianHead} onClick={toggleAccord} expandIcon={<ExpandMoreIcon />} aria-controls="panel3a-content" id="panel3a-header">
                    <Typography className={cls.heading}>Update the subject line for this email</Typography>
                </AccordionSummary>
                <AccordionDetails className={cls.accordianBody}>
                    <div className={cls.table}>
                        <div className={cls.textdiv}>
                            <TextField fullWidth className={cls.textfield} onChange={onChange} placeholder="Update your subject..." />
                        </div>
                        <Divider />
                        <p>
                            <span><strong>Current Subject: </strong>{subject}</span>
                        </p>
                        {children}
                    </div>
                </AccordionDetails>
            </Accordion>
        </>
    );
};
