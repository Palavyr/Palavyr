import React, { useContext, useState } from "react";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogTitle from "@material-ui/core/DialogTitle";
import { AreaTable } from "@Palavyr-Types";
import { DialogContent, makeStyles } from "@material-ui/core";
import { AddOrCancel } from "@common/components/AddOrCancel";
import { DashboardContext } from "../DashboardContext";

const useStyles = makeStyles((theme) => ({
    dialog: {
        backgroundColor: theme.palette.primary.dark,
    },
    dialogContent: {},
    dialogTitle: {},
    dialogActions: {},
    text: {
        color: theme.palette.common.white,
    },
}));

export interface IAddNewAreaModal {
    open: boolean;
    handleClose(): void;
    setNewArea(newAreaObject: AreaTable): void;
}

export const AddNewAreaModal = ({ open, handleClose, setNewArea }: IAddNewAreaModal) => {
    const [areaName, setAreaName] = useState<string>("");
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);

    const onAdd = async () => {
        if (areaName !== "") {
            const newArea = await repository.Area.createArea(areaName);
            setNewArea(newArea);
        }
        handleClose();
        setAreaName("");
    };

    const textFieldOnChange = (event: { target: { value: React.SetStateAction<string> } }) => {
        setAreaName(event.target.value);
    };

    return (
        <Dialog fullWidth classes={{ root: cls.dialog }} open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
            <DialogTitle>Add a new Area</DialogTitle>
            <DialogContent>
                <TextField className={cls.text} autoFocus margin="dense" value={areaName} onChange={textFieldOnChange} id="name" label="New Area Name" type="text" fullWidth />
            </DialogContent>
            <DialogActions>
                <AddOrCancel onAdd={onAdd} onCancel={handleClose} addText="Add" cancelText="Cancel" />
            </DialogActions>
        </Dialog>
    );
};
