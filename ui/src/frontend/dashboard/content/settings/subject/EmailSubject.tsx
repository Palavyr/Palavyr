import React from "react";
import { Accordion, AccordionSummary, Typography, AccordionDetails, Button, makeStyles, TextField, Divider } from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { FontStyleOptions, Variant } from "@material-ui/core/styles/createTypography";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";

export interface IUploadAttachment {
    onChange(event: { target: { value: React.SetStateAction<string> } }): void;
    subject: string;
    accordianTitle: string;
    children: React.ReactNode;
}

const useStyles = makeStyles<{}>((theme: any) => ({
    textdiv: {},
    textfield: {},
    table: {
        width: "100%",
        marginTop: "0.3rem",
        marginBottom: "0.3rem",
    },
}));
//
export const EmailSubject = ({ onChange, accordianTitle, subject, children }: IUploadAttachment) => {
    const cls = useStyles();
    return (
        <PalavyrAccordian title={accordianTitle}>
            <div className={cls.table}>
                <div className={cls.textdiv}>
                    <TextField fullWidth className={cls.textfield} onChange={onChange} placeholder="Update your subject..." />
                </div>
                <Divider />
                <p>
                    <span>
                        <strong>Current Subject: </strong>
                        {subject}
                    </span>
                </p>
                {children}
            </div>
        </PalavyrAccordian>
    );
};
