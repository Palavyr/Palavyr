import React, { ChangeEvent } from "react";
import { SaveOrCancel } from "./SaveOrCancel";
import { TextField, makeStyles } from "@material-ui/core";
import { PalavyrAccordian } from "./PalavyrAccordian";

export interface IExpandableTextBox {
    title: string;
    children: React.ReactNode;
    updatableValue: string;
    onChange(e: ChangeEvent<HTMLInputElement>): void;
    onSave(): Promise<boolean>;
    initialState?: boolean;
}

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    textField: {
        padding: "1rem",
        border: "none",
    },
}));

export const ExpandableTextBox = ({ updatableValue, title, onChange, onSave, children, initialState }: IExpandableTextBox) => {
    const cls = useStyles();
    return (
        <PalavyrAccordian title={title} initialState={initialState ?? false} actions={<SaveOrCancel onSave={onSave} />}>
            {children}
            <div className={cls.textField}>
                <TextField fullWidth multiline placeholder="Place text here" rows={3} value={updatableValue} onChange={(e: ChangeEvent<HTMLInputElement>) => onChange(e)}></TextField>
            </div>
        </PalavyrAccordian>
    );
};
