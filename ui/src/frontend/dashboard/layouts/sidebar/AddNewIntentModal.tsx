import React, { useContext, useState } from "react";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogTitle from "@material-ui/core/DialogTitle";
import { DialogContent, makeStyles } from "@material-ui/core";
import { AddOrCancel } from "@common/components/AddOrCancel";
import { DashboardContext } from "../DashboardContext";
import { ADD_NEW_INTENT_BACKDROP_zINDEX, ADD_NEW_INTENT_DIALOG_BOX_zINDEX } from "@constants";
import { IntentResource } from "@common/types/api/EntityResources";


const useStyles = makeStyles<{}>((theme: any) => ({
    dialog: {
        zIndex: ADD_NEW_INTENT_DIALOG_BOX_zINDEX,
    },
    backdrop: {
        "& .MuiBackdrop-root": {
            zIndex: ADD_NEW_INTENT_BACKDROP_zINDEX,
        },

        backgroundColor: theme.palette.primary.dark,
    },
    dialogContent: {
        backgroundColor: "blue",
    },
    dialogTitle: {},
    dialogActions: {},
    text: {
        color: theme.palette.common.white,
    },
}));

export interface AddNewIntentModalProps {
    open: boolean;
    handleClose(): void;
    setNewIntent(): void;
}

export const AddNewIntentModal = ({ open, handleClose, setNewIntent }: AddNewIntentModalProps) => {
    const [intentName, setIntentName] = useState<string>("");
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [buttonDisabled, setButtonDisabled] = useState<boolean>(false);

    const onAdd = async () => {
        setButtonDisabled(true);
        if (intentName.trim() !== "") {
            await repository.Intent.CreateIntent(intentName);
            setNewIntent();
        }
        handleClose();
        setIntentName("");
        setButtonDisabled(false);
    };

    const textFieldOnChange = (event: { target: { value: string } }) => {
        setIntentName(event.target.value);
    };

    return (
        <Dialog BackdropProps={{ className: cls.backdrop }} fullWidth classes={{ root: cls.dialog }} open={open} onClose={handleClose}>
            <DialogTitle>Add a new intent</DialogTitle>
            <DialogContent>
                <TextField className={cls.text} autoFocus margin="dense" value={intentName} onChange={textFieldOnChange} id="name" label="New Intent Name" type="text" fullWidth />
            </DialogContent>
            <DialogActions>
                <AddOrCancel disabled={buttonDisabled} onAdd={onAdd} onCancel={handleClose} addText="Add" cancelText="Cancel" />
            </DialogActions>
        </Dialog>
    );
};
