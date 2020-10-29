import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { AnyVoidFunction } from "@Palavyr-Types";
import { CustomAlert } from "./customAlert/CutomAlert";
import SaveIcon from '@material-ui/icons/Save';
import DeleteOutlineIcon from '@material-ui/icons/DeleteOutline';

export type AlertMessage = {
    title: string;
    message: string;
}
export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onSave: (e?: any) => boolean | null | void | Promise<void>;
    onCancel?: AnyVoidFunction;
    onDelete?: AnyVoidFunction;
    customSaveMessage?: AlertMessage;
    customCancelMessage?: AlertMessage;
    useModal?: boolean;
}

const useStyles = makeStyles((theme => ({
    saveButton: {
        border: `1px solid ${theme.palette.primary}`,
        borderRadius: "10px",
        background: `${theme.palette.primary}`
    },
    cancelButton: {
        border: `1px solid ${theme.palette.secondary}`,
        borderRadius: "10px",
        background: `${theme.palette.secondary}`

    },
    delButton: {
        border: `1px solid ${theme.palette.secondary}`,
        borderRadius: "10px",
        background: `${theme.palette.secondary}`
    },
    button: {
        marginLeft: "0.1rem",
        marginRight: "0.1rem",
        background: "white",
        '&:hover': {
            background: "#757ce8"
        }
    },
    saveCancelWrapper: {
        height: "100%",
        verticalAlign: "middle",
    }
})))

export const SaveOrCancel = ({ onSave, onCancel, onDelete, customSaveMessage, customCancelMessage, useModal, size = "small" }: ISaveOrCancel) => {

    const classes = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);
    const [cancelAlertState, setCancelAlertState] = useState<boolean>(false);

    return (
        <>
            {
                onDelete &&
                <Button
                    startIcon={<DeleteOutlineIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.delButton)}
                    onClick={onDelete}
                    size={size}
                >
                    Delete
                </Button>
            }
            {
                <Button
                    startIcon={<SaveIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.saveButton)}
                    onClick={
                        async (e) => {
                            var res = await onSave(e);
                            if (res === true || res === null) {
                                if (cancelAlertState) setCancelAlertState(false);
                                setAlertState(true);
                            }
                        }
                    }
                    size={size}
                >
                    Save
                </Button>
            }
            {
                onCancel &&
                <Button
                    variant="outlined"
                    className={classNames(classes.button, classes.cancelButton)}
                    onClick={
                        async (e) => {
                            await onCancel(e);
                            if (alertState) setAlertState(false);
                            setCancelAlertState(true);
                        }
                    }
                    size={size}
                >
                    Cancel
                </Button>
            }
            {
                alertState && useModal && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={customSaveMessage ?? { title: "Save Successful", message: "" }} />
            }
            {
                alertState && useModal && <CustomAlert setAlert={setCancelAlertState} alertState={cancelAlertState} alert={customCancelMessage ?? { title: "Cancelled", message: "" }} />
            }

        </>
    );
};
