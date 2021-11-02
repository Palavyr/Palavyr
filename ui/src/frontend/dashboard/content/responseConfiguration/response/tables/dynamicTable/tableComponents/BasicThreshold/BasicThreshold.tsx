import React, { useContext } from "react";
import { BasicThresholdData, DynamicTableProps } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { BasicThresholdHeader } from "./BasicThresholdHeader";
import { BasicThresholdBody } from "./BasicThresholdBody";
import { useState } from "react";
import { Button, makeStyles, Table, TableContainer, TextField, AccordionActions } from "@material-ui/core";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";
import { Align } from "@common/positioning/Align";

const useStyles = makeStyles(theme => ({
    alignLeft: {
        position: "relative",
        top: "50%",
        float: "left",
        paddingLeft: "0.3rem",
    },
    alignRight: {
        position: "relative",
        top: "50%",
        transform: "translateY(25%)",
        float: "right",
        paddingRight: "0.3rem",
        height: "100%",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {
        marginRight: "0.4rem",
    },
    input: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
    container: {
        borderTop: `4px solid ${theme.palette.primary.main}`,
        borderBottom: `4px solid ${theme.palette.primary.main}`,
    },
    inputPropsCls: {
        paddingLeft: "0.4rem",
    },
}));

export const BasicThreshold = ({ showDebug, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: Omit<DynamicTableProps, "tableMeta" | "setTableMeta">) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [name, setItemName] = useState<string>("");

    const modifier = new BasicThresholdModifier(setTableData);

    const onSave = async () => {
        const reorderedData = modifier.reorderThresholdData(tableData);
        const result = modifier.validateTable(reorderedData);

        if (result) {
            const saveBasicThreshold = await repository.Configuration.Tables.Dynamic.saveDynamicTable<BasicThresholdData[]>(areaIdentifier, DynamicTableTypes.BasicThreshold, reorderedData, tableId, tableTag);
            setTableData(saveBasicThreshold);
            return true;
        } else {
            return false;
        }
    };
    const addThresholdOnClick = () => modifier.addThreshold(tableData, areaIdentifier, tableId, repository);

    return (
        <>
            <div className={cls.container}>
                <Align direction="flex-start">
                    <TextInput
                        className={cls.input}
                        variant="standard"
                        label="Name to use in PDF fee table"
                        type="text"
                        value={tableData[0].itemName}
                        InputLabelProps={{ className: cls.inputPropsCls }}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setItemName(tableData, event.target.value);
                            setItemName(event.target.value);
                        }}
                    />
                </Align>
                <TableContainer>
                    <Table>
                        <BasicThresholdHeader tableData={tableData} modifier={modifier} />
                        <BasicThresholdBody tableData={tableData} modifier={modifier} />
                    </Table>
                </TableContainer>
            </div>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addThresholdOnClick} color="primary" variant="contained">
                            Add Threshold
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableData} />}
        </>
    );
};
