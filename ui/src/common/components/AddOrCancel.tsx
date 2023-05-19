import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { AnyVoidFunction } from "@Palavyr-Types";
import { CustomAlert } from "./customAlert/CutomAlert";
import PlaylistAddIcon from "@material-ui/icons/PlaylistAdd";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";

export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onAdd: AnyVoidFunction;
    onCancel?: AnyVoidFunction;
    useModal?: boolean;
    addText?: string;
    cancelText?: string;
    disabled?: boolean;
}

const useStyles = makeStyles<{}>((theme: any) => ({
    saveButton: {
        border: `1px solid ${theme.palette.primary}`,
        borderRadius: "10px",
        background: `${theme.palette.primary}`,
        "&:hover": {
            background: "#757ce8",
        },
    },
    cancelButton: {
        border: `1px solid ${theme.palette.secondary}`,
        borderRadius: "10px",
        background: `${theme.palette.secondary}`,
        "&:hover": {
            background: "#FF4785",
        },
    },
    button: {
        marginLeft: "0.1rem",
        marginRight: "0.1rem",
    },
    saveCancelWrapper: {
        height: "100%",
        verticalAlign: "middle",
    },
}));

export const AddOrCancel = ({ disabled, onAdd, onCancel, useModal, addText = "Add", cancelText = "Cancel", size = "small" }: ISaveOrCancel) => {
    const classes = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);

    return (
        <>
            {
                <Button
                    disabled={disabled}
                    endIcon={<PlaylistAddIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.saveButton)}
                    onClick={async () => {
                        await onAdd();
                        useModal && setAlertState(true);
                    }}
                    size={size}
                >
                    {addText}
                </Button>
            }
            {onCancel && (
                <Button
                    disabled={disabled}
                    endIcon={<DeleteOutlineIcon />}
                    variant="outlined"
                    className={classNames(classes.button, classes.cancelButton)}
                    onClick={async () => {
                        await onCancel();
                        useModal && setAlertState(true);
                    }}
                    size={size}
                >
                    {cancelText}
                </Button>
            )}
            {alertState && useModal && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={{ title: "Save Successful", message: "" }} />}
        </>
    );
};
