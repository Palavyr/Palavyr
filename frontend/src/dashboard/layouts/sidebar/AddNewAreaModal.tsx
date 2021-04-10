import React, { useState } from "react";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogTitle from "@material-ui/core/DialogTitle";
import { AreaTable } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { DialogContent, makeStyles } from "@material-ui/core";
import { AddOrCancel } from "@common/components/AddOrCancel";

const useStyles = makeStyles((theme) => ({
    dialog: {
        backgroundColor: theme.palette.background.default,
    },
    dialogContent: {},
    dialogTitle: {},
    dialogActions: {},
}));

export interface IAddNewAreaModal {
    open: boolean;
    handleClose(): void;
    setNewArea(newAreaObject: AreaTable): void;
}

export const AddNewAreaModal = ({ open, handleClose, setNewArea }: IAddNewAreaModal) => {
    const [areaName, setAreaName] = useState<string>("");
    const classes = useStyles();
    const client = new ApiClient();

    const onAdd = async () => {
        if (areaName !== "") {
            const {data: newArea} = await client.Area.createArea(areaName);
            setNewArea(newArea);
        }
        handleClose();
        setAreaName("");
    };

    const textFieldOnChange = (event: { target: { value: React.SetStateAction<string> } }) => {
        setAreaName(event.target.value);
    };

    return (
        <Dialog fullWidth classes={{ root: classes.dialog, paper: classes.dialog }} open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Add a new Area</DialogTitle>
            <DialogContent>
                <TextField autoFocus margin="dense" value={areaName} onChange={textFieldOnChange} id="name" label="New Area Name" type="text" fullWidth />
            </DialogContent>
            <DialogActions>
                <AddOrCancel onAdd={onAdd} onCancel={handleClose} addText="Add" cancelText="Cancel" />
            </DialogActions>
        </Dialog>
    );
};
