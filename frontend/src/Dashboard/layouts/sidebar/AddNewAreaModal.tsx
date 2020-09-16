import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogTitle from "@material-ui/core/DialogTitle";
import { AreaTable } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { DialogContent } from "@material-ui/core";


interface IAddNewAreaModal {
    open: boolean;
    handleClose: () => void;
    setNewArea: (newAreaObject: AreaTable) => void;
}

export const AddNewAreaModal = ({ open, handleClose, setNewArea }: IAddNewAreaModal) => {
    const [areaName, setAreaName] = useState<string>("");

    return (
        <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Add a new Area</DialogTitle>
            <DialogContent>
                <TextField autoFocus margin="dense" value={areaName} onChange={(event) => setAreaName(event.target.value)} id="name" label="New Area Name" type="text" fullWidth />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} color="primary">
                    Cancel
                </Button>
                <Button
                    onClick={async () => {
                        var client = new ApiClient();
                        var newArea = await client.Area.createArea(areaName)
                        setNewArea(newArea.data);
                        handleClose();
                    }}
                    color="primary"
                >
                    Add
                </Button>
            </DialogActions>
        </Dialog>
    );
};
