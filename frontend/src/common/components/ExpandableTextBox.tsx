import React from "react";
import { SaveOrCancel } from "./SaveOrCancel";
import { TextField, makeStyles } from "@material-ui/core";
import { AnyVoidFunction } from "@Palavyr-Types";
import { PalavyrAccordian } from "./PalavyrAccordian";

export interface IExpandableTextBox {
    title: string;
    children: React.ReactNode;
    updatableValue: string;
    onChange: AnyVoidFunction;
    onSave(): Promise<boolean>;
}

const useStyles = makeStyles((theme) => ({
    textField: {
        padding: "1rem",
        border: "none",
    },
}));

export const ExpandableTextBox = ({ updatableValue, title, onChange, onSave, children }: IExpandableTextBox) => {
    const cls = useStyles();
    return (
        <PalavyrAccordian title={title} initialState={updatableValue === ""} actions={<SaveOrCancel onSave={onSave} />}>
            {children}
            <div className={cls.textField}>
                <TextField fullWidth multiline placeholder="Place text here" rows={3} value={updatableValue} onChange={onChange}></TextField>
            </div>
        </PalavyrAccordian>
    );
};
