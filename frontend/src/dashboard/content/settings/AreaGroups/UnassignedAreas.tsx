import { NullArea } from "./GroupTree";
import { GroupRow, Groups } from "@Palavyr-Types";
import React, { useState } from "react";
import { GroupAssignmentModal } from "./GroupAssignmentModal";
import { Paper, Chip } from "@material-ui/core";


export interface IUnassignedAreas {
  nullAreas: Array<NullArea>;
  groups: Array<GroupRow>;
  setNodes: (val: Groups) => void;
}

export const UnassignedAreas = ({ nullAreas, groups, setNodes }: IUnassignedAreas) => {

  const [modalState, setModalState] = useState<boolean>(false);

  return (
      <>
        <h3>Unassigned Areas</h3>
        <Paper style={{ borderRadius: "0px", padding: "0.5rem" }}>
        <>
          {
            nullAreas.map(area => <GroupAssignmentModal
                  key={area.areaIdentifier}
                  modalState={modalState}
                  setModalState={setModalState}
                  groups={groups}
                  areaIdentifier={area.areaIdentifier}
                  setNodes={setNodes}
                />)
            }
            {
              nullAreas.map(area => <Chip
                  key={area.areaIdentifier + "-Chip"}
                  variant="outlined"
                  color="secondary"
                  style={{ color: "black", margin: "1rem", padding: "0.8rem" }}
                  label={area.areaName}
                  onClick={() => setModalState(true)}
                />)
              }
          </>
        </Paper>
      </>
  )
}