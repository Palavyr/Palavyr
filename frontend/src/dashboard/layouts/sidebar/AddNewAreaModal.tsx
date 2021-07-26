import React, { useContext, useState } from "react";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogTitle from "@material-ui/core/DialogTitle";
import { AreaTable } from "@Palavyr-Types";
import { DialogContent, makeStyles } from "@material-ui/core";
import { AddOrCancel } from "@common/components/AddOrCancel";
import { DashboardContext } from "../DashboardContext";
import { ADD_NEW_AREA_BACKDROP_zINDEX, ADD_NEW_AREA_DIALOG_BOX_zINDEX } from "@constants";

const useStyles = makeStyles((theme) => ({
    dialog: {
        zIndex: ADD_NEW_AREA_DIALOG_BOX_zINDEX,
    },
    backdrop: {
        "& .MuiBackdrop-root": {
            zIndex: ADD_NEW_AREA_BACKDROP_zINDEX,
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

export interface IAddNewAreaModal {
    open: boolean;
    handleClose(): void;
    setNewArea(newAreaObject: AreaTable): void;
}

export const AddNewAreaModal = ({ open, handleClose, setNewArea }: IAddNewAreaModal) => {
    const [areaName, setAreaName] = useState<string>("");
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [buttonDisabled, setButtonDisabled] = useState<boolean>(false);

    const onAdd = async () => {
        setButtonDisabled(true);
        if (areaName !== "") {
            const newArea = await repository.Area.createArea(areaName);
            setNewArea(newArea);
        }
        handleClose();
        setAreaName("");
        setButtonDisabled(false);
    };

    const textFieldOnChange = (event: { target: { value: string } }) => {
        setAreaName(event.target.value);
    };

    return (
        <Dialog BackdropProps={{ className: cls.backdrop }} fullWidth classes={{ root: cls.dialog }} open={open} onClose={handleClose}>
            <DialogTitle>Add a new Area</DialogTitle>
            <DialogContent>
                <TextField className={cls.text} autoFocus margin="dense" value={areaName} onChange={textFieldOnChange} id="name" label="New Area Name" type="text" fullWidth />
            </DialogContent>
            <DialogActions>
                <AddOrCancel disabled={buttonDisabled} onAdd={onAdd} onCancel={handleClose} addText="Add" cancelText="Cancel" />
            </DialogActions>
        </Dialog>
    );
};
