import { GroupRow, Groups } from "@Palavyr-Types";
import { Dialog, DialogTitle, DialogContent } from "@material-ui/core";
import React from "react";
import { GroupSelector } from "./GroupSelector";

interface IAssignmentEditor {
    modalState: boolean;
    groups: Array<GroupRow>;
    areaIdentifier: string;
    setModalState: (state: boolean) => void;
    setNodes: (val: Groups) => void;
}

export const GroupAssignmentModal = ({ modalState, groups, areaIdentifier, setModalState, setNodes }: IAssignmentEditor) => {

    const handleCloseModal = () => {
        setModalState(false);
    }

    return (
        <Dialog open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Select a group</DialogTitle>
            <DialogContent>
                <GroupSelector areaIdentifier={areaIdentifier} groups={groups} setModalState={setModalState} setNodes={setNodes} />
            </DialogContent>
        </Dialog>
    );
};
