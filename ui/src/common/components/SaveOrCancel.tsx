import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import SaveIcon from "@material-ui/icons/Save";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";
import CircularProgress from "@material-ui/core/CircularProgress";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { SnackbarPositions } from "@Palavyr-Types";
import CancelIcon from "@material-ui/icons/Cancel";

export type AlertMessage = {
    title: string;
    message: string;
};
export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onSave?(e?: any): Promise<boolean>;
    onCancel?(): Promise<any> | any;
    onDelete?(): Promise<any>;
    customSaveMessage?: string;
    customCancelMessage?: string;
    timeout?: number;
    saveText?: string;
    cancelText?: string;
    deleteText?: string;
    position?: "left" | "right" | "center";
    useSaveIcon?: boolean;
    buttonType?: "button" | "submit";
    zIndex?: number;
}

const useStyles = makeStyles(theme => ({
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

type PositionMap = {
    [key: string]: SnackbarPositions;
};
const positionMap: PositionMap = {
    right: "br",
    left: "bl",
    center: "b",
};

export const SaveOrCancel = ({
    onSave,
    onCancel,
    onDelete,
    customSaveMessage,
    customCancelMessage,
    position = "center",
    size = "small",
    timeout = 1500,
    saveText = "Save",
    cancelText = "Cancel",
    deleteText = "Delete",
    useSaveIcon = true,
    buttonType = "button",
    zIndex = 1000,
}: ISaveOrCancel) => {
    const cls = useStyles();
    const [isSaving, setIsSaving] = useState<boolean>(false);
    const [isDeleting, setIsDeleting] = useState<boolean>(false);
    const [isCancelling, setIsCancelling] = useState<boolean>(false);

    const { setSuccessText, successOpen, setSuccessOpen, setWarningText, warningOpen, setWarningOpen, setSnackPosition } = React.useContext(DashboardContext);
    const setPosition = () => {
        const anchorPosition = positionMap[position];
        setSnackPosition(anchorPosition);
    };

    const onSaveClick = async (e: any) => {
        setIsSaving(true);
        setTimeout(async () => {
            if (onSave) {
                try {
                    const res = await onSave(e);
                    if (res === true || res === null) {
                        setPosition();
                        if (warningOpen) setWarningOpen(false);
                        setSuccessText(customSaveMessage ?? "Save Successful");
                        setSuccessOpen(true);
                    } else {
                        setPosition();
                        setWarningText(customCancelMessage ?? "Cancelled");
                        setWarningOpen(true);
                    }
                } catch {
                    setIsSaving(false);
                }
            }
            setIsSaving(false);
        }, timeout);
    };

    const onCancelClick = async () => {
        if (onCancel) {
            setIsCancelling(true);
            setPosition();
            await onCancel();
            if (successOpen) setSuccessOpen(false);
            setWarningText(customCancelMessage ?? "Cancelled");
            setWarningOpen(true);
            setIsCancelling(false);
        }
    };

    const onDeleteClick = async () => {
        if (onDelete) {
            setIsDeleting(true);
            setPosition();
            await onDelete();
            setWarningText("Delete Successful");
            setWarningOpen(true);
            setIsDeleting(false);
        }
    };

    return (
        <>
            {
                <Button
                    style={{ zIndex: zIndex }}
                    type={buttonType}
                    startIcon={isSaving ? <CircularProgress size={20} /> : useSaveIcon ? <SaveIcon onClick={onSaveClick} style={{ background: "transparent" }} /> : <></>}
                    variant="outlined"
                    className={classNames(cls.button, cls.saveButton)}
                    onClick={onSaveClick}
                    size={size}
                >
                    {saveText}
                </Button>
            }
            {onCancel && (
                <Button
                    style={{ zIndex: zIndex }}
                    variant="outlined"
                    className={classNames(cls.button, cls.cancelButton)}
                    onClick={onCancelClick}
                    size={size}
                    startIcon={isCancelling ? <CircularProgress size={20} /> : useSaveIcon ? <CancelIcon onClick={onCancelClick} style={{ background: "transparent" }} /> : <></>}
                >
                    {cancelText}
                </Button>
            )}
            {onDelete && (
                <Button
                    style={{ zIndex: zIndex }}
                    startIcon={isDeleting ? <CircularProgress size={20} /> : useSaveIcon ? <DeleteOutlineIcon onClick={onSaveClick} style={{ background: "transparent" }} /> : <></>}
                    variant="outlined"
                    className={classNames(cls.button, cls.delButton)}
                    onClick={onDeleteClick}
                    size={size}
                >
                    {deleteText}
                </Button>
            )}
        </>
    );
};
