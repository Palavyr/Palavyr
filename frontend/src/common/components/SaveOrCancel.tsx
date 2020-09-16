import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { AnyVoidFunction } from "@Palavyr-Types";
import { CustomAlert } from "./customAlert/CutomAlert";
import SaveIcon from '@material-ui/icons/Save';
import DeleteOutlineIcon from '@material-ui/icons/DeleteOutline';


export interface ISaveOrCancel {
    size?: "small" | "medium" | "large" | undefined;
    onSave: AnyVoidFunction;
    onCancel?: AnyVoidFunction;
    onDelete?: AnyVoidFunction;
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
        '&:hover': {
            background: "#757ce8"
        }
    },
    saveCancelWrapper: {
        height: "100%",
        verticalAlign: "middle",
    }
})))

export const SaveOrCancel = ({ onSave, onCancel, onDelete, size = "small" }: ISaveOrCancel) => {

    const classes = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);

    return (
        <>
        {/* <div className={classes.saveCancelWrapper}> */}
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
                        async () => {
                            await onSave()
                            setAlertState(true);
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
                        async () => {
                            await onCancel()
                            setAlertState(true);
                        }
                    }
                    size={size}
                >
                    Cancel
                </Button>
            }
            {
                alertState && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={{ title: "Save Successful", message: "" }} />
            }
        {/* </div> */}
        </>
    );
};
