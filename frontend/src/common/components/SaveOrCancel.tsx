import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { CustomAlert } from "./customAlert/CutomAlert";
import SaveIcon from "@material-ui/icons/Save";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";
import CircularProgress from "@material-ui/core/CircularProgress";

export type AlertMessage = {
    title: string;
    message: string;
};
export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onSave(e?: any): Promise<boolean>;
    onCancel?(): Promise<any> | any;
    onDelete?(): Promise<any>;
    customSaveMessage?: AlertMessage;
    customCancelMessage?: AlertMessage;
    useModal?: boolean;
    timeout?: number;
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
            background: "#757ce8",
        },
    },
    saveCancelWrapper: {
        height: "100%",
        verticalAlign: "middle",
    },
}));

export const SaveOrCancel = ({ onSave, onCancel, onDelete, customSaveMessage, customCancelMessage, useModal, size = "small", timeout = 2000}: ISaveOrCancel) => {
    const classes = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);
    const [cancelAlertState, setCancelAlertState] = useState<boolean>(false);
    const [isSaving, setIsSaving] = useState<boolean>(false);

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
                                if (cancelAlertState) setCancelAlertState(false); // TODO: these don't show modal for static tables.
                                setAlertState(true);
                            } else {
                                setCancelAlertState(true);
                            }
                            setIsSaving(false);
                        }, timeout);
                    }}
                    size={size}
                >
                    Save
                </Button>
            }
            {onDelete && (
                <Button
                    startIcon={<DeleteOutlineIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.delButton)}
                    onClick={async () => {
                        await onDelete();
                    }}
                    size={size}
                >
                    Delete
                </Button>
            )}
            {onCancel && (
                <Button
                    variant="outlined"
                    className={classNames(classes.button, classes.cancelButton)}
                    onClick={async () => {
                        await onCancel();
                        if (alertState) setAlertState(false);
                        setCancelAlertState(true);
                    }}
                    size={size}
                >
                    Cancel
                </Button>
            )}
            {alertState && useModal && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={customSaveMessage ?? { title: "Save Successful", message: "" }} />}
            {alertState && useModal && <CustomAlert setAlert={setCancelAlertState} alertState={cancelAlertState} alert={customCancelMessage ?? { title: "Cancelled", message: "" }} />}
        </>
    );
};
