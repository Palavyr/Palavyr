import { makeStyles } from "@material-ui/core";
import React from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { NameForm } from "./FormInputs/NameForm";
import { EmailForm } from "./FormInputs/EmailForm";
import { SetState } from "@Palavyr-Types";

export interface CollectDetailsFormProps {
    setKickoff: SetState<boolean>;
}

export interface BaseFormProps {
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
}

const useStyles = makeStyles(theme => ({
    baseDialogCollectionForm: {
        zIndex: 9999,
        position: "absolute",
    },
    dialogBackgroundCollectionForm: {
        backgroundColor: "rgba(255, 255, 255, 50)",
        zIndex: 9999,
    },
    dialogPaperCollectionForm: {
        zIndex: 9999,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
        backgroundColor: "rgba(255, 255, 255, 50)",
    },
    dialogPaperScrollPaperCollectionForm: {
        maxHeight: "none",
    },
    dialogContentCollectionForm: {
        paddingTop: 0,
        paddingBottom: 0,
    },
    buttonCollectionForm: {
        margin: "0.5rem",
        textAlign: "center",
        marginTop: "1.3rem",
    },
}));

export interface ContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: any;
    phonePattern: any;
    onChange: any;
    detailsSet: boolean;
    localeOptions: any;
    submitButton: React.ReactNode;
    disabled: boolean;
}
export interface MiniContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: SetState<boolean>;
    submitButton: React.ReactNode;
    disabled: boolean;
}
export const MiniContactForm = ({ disabled, onFormSubmit, setDetailsSet, submitButton, formProps }: MiniContactFormProps) => {
    const cls = useStyles();
    return (
        <form onSubmit={onFormSubmit}>
            <NameForm {...formProps} disabled={disabled} />
            <EmailForm {...formProps} setDetailsSet={setDetailsSet} disabled={disabled} />
            <div style={{ display: "flex", justifyContent: "right" }}>{submitButton}</div>
        </form>
    );
};
