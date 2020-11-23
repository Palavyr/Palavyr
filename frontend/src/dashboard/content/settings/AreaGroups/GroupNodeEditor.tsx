import { Groups } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from "@material-ui/core";
import React from "react";

interface IGroupNodeEditorModal {
    modalState: boolean;
    text: string;
    groupId: string;
    setNodes: (nodes: Groups) => void;
    setText: (text: string) => void;
    setModalState: (state: boolean) => void;
}

export const GroupNodeEditorModal = ({ groupId, modalState, text, setText, setModalState, setNodes }: IGroupNodeEditorModal) => {
    var client = new ApiClient();

    const handleCloseModal = () => {
        // we need to save the node data in the original node list and rerender the node
        setModalState(false);
    }

    return (
        <Dialog open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Edit Group Name</DialogTitle>
            <DialogContent>
                <TextField
                    margin="dense"
                    value={text}
                    multiline rows={1}
                    onChange={(event) => { setText(event.target.value) }}
                    id="groupName"
                    label="Group Name"
                    type="text"
                    fullWidth
                />
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={
                        async (e) => {
                            e.preventDefault();
                            var res = await client.Settings.Groups.RemoveGroup(groupId);
                            setNodes(res.data);
                            handleCloseModal();
                        }
                    }

                >
                    Delete Group
                </Button>
                <Button
                    onClick={
                        async (e) => {
                            e.preventDefault();
                            var res = await client.Settings.Groups.UpdateGroupName(text, groupId)
                            setNodes(res.data);
                            handleCloseModal();
                        }
                    }
                    color="primary" variant="contained"
                >
                    Save
                </Button>
                <Button onClick={handleCloseModal} color="secondary" variant="contained">
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    );
};
