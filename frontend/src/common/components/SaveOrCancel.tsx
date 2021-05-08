import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import SaveIcon from "@material-ui/icons/Save";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";
import CircularProgress from "@material-ui/core/CircularProgress";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

export type AlertMessage = {
    title: string;
    message: string;
};
export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onSave(e?: any): Promise<boolean>;
    onCancel?(): Promise<any> | any;
    onDelete?(): Promise<any>;
    customSaveMessage?: string;
    customCancelMessage?: string;
    timeout?: number;
    saveText?: string;
    cancelText?: string;
    deleteText?: string;
    position?: "left" | "right" | "center";
}

const useStyles = makeStyles((theme) => ({
    saveButton: {
        border: `1px solid ${theme.palette.primary}`,
        borderRadius: "10px",
        background: `${theme.palette.primary}`,
    },
    cancelButton: {
        border: `1px solid ${theme.palette.secondary}`,
        borderRadius: "10px",
        background: `${theme.palette.secondary}`,
    },
    delButton: {
        border: `1px solid ${theme.palette.secondary}`,
        borderRadius: "10px",
        background: `${theme.palette.secondary}`,
    },
    button: {
        marginLeft: "0.1rem",
        marginRight: "0.1rem",
        background: "white",
        "&:hover": {
            background: theme.palette.success.main,
        },
    },
    saveCancelWrapper: {
        height: "100%",
        verticalAlign: "middle",
    },
}));

export const SaveOrCancel = ({ onSave, onCancel, onDelete, customSaveMessage, customCancelMessage, size = "small", timeout = 2000, saveText = "Save", cancelText = "Cancel", deleteText = "Delete" }: ISaveOrCancel) => {
    const classes = useStyles();
    const [isSaving, setIsSaving] = useState<boolean>(false);

    const { setSuccessText, successOpen, setSuccessOpen, setWarningText, warningOpen, setWarningOpen, setSnackPosition } = React.useContext(DashboardContext);
    setSnackPosition("b");
    return (
        <>
            {
                <Button
                    startIcon={isSaving ? <CircularProgress size={20} /> : <SaveIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.saveButton)}
                    onClick={async (e) => {
                        setIsSaving(true);
                        setTimeout(async () => {
                            var res = await onSave(e);
                            if (res === true || res === null) {
                                if (warningOpen) setWarningOpen(false);
                                setSuccessText(customSaveMessage ?? "Save Successful");
                                setSuccessOpen(true);
                            } else {
                                setWarningText(customCancelMessage ?? "Cancelled");
                                setWarningOpen(true);
                            }
                            setIsSaving(false);
                        }, timeout);
                    }}
                    size={size}
                >
                    {saveText}
                </Button>
            }
            {onCancel && (
                <Button
                    variant="outlined"
                    className={classNames(classes.button, classes.cancelButton)}
                    onClick={async () => {
                        await onCancel();
                        if (successOpen) setSuccessOpen(false);
                        setWarningText(customCancelMessage ?? "Cancelled");
                        setWarningOpen(true);
                    }}
                    size={size}
                >
                    {cancelText}
                </Button>
            )}
            {onDelete && (
                <Button
                    startIcon={<DeleteOutlineIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.delButton)}
                    onClick={async () => {
                        await onDelete();
                        setWarningText("Delete Successful");
                        setWarningOpen(true);
                    }}
                    size={size}
                >
                    {deleteText}
                </Button>
            )}
        </>
    );
};
