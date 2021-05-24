import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import SaveIcon from "@material-ui/icons/Save";
import DeleteOutlineIcon from "@material-ui/icons/DeleteOutline";
import CircularProgress from "@material-ui/core/CircularProgress";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SnackbarPositions } from "@Palavyr-Types";

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
    timeout = 2000,
    saveText = "Save",
    cancelText = "Cancel",
    deleteText = "Delete",
    useSaveIcon = true,
    buttonType = "button"
}: ISaveOrCancel) => {
    const cls = useStyles();
    const [isSaving, setIsSaving] = useState<boolean>(false);

    const { setSuccessText, successOpen, setSuccessOpen, setWarningText, warningOpen, setWarningOpen, setSnackPosition } = React.useContext(DashboardContext);
    const setPosition = () => {
        const anchorPosition = positionMap[position];
        setSnackPosition(anchorPosition);
    };
    return (
        <>
            {
                <Button
                    type={buttonType}
                    startIcon={isSaving ? <CircularProgress size={20} /> : useSaveIcon ? <SaveIcon /> : <></>}
                    variant="outlined"
                    className={classNames(cls.button, cls.saveButton)}
                    onClick={async (e) => {
                        setIsSaving(true);
                        setTimeout(async () => {
                            if (onSave) {
                                var res = await onSave(e);
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
                                setIsSaving(false);
                            }
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
                    className={classNames(cls.button, cls.cancelButton)}
                    onClick={async () => {
                        setPosition();
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
                    className={classNames(cls.button, cls.delButton)}
                    onClick={async () => {
                        setPosition();
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
