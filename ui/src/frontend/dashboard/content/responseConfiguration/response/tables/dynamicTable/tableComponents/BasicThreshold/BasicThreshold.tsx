import React, { useContext, useEffect } from "react";
import { BasicThresholdData, DynamicTableProps } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { BasicThresholdHeader } from "./BasicThresholdHeader";
import { BasicThresholdBody } from "./BasicThresholdBody";
import { useState } from "react";
import { Button, makeStyles, Table, TableContainer, AccordionActions } from "@material-ui/core";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";
import { Align } from "@common/positioning/Align";
import { cloneDeep } from "lodash";
import { DynamicTableHeader } from "../../DynamicTableHeader";

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

export const BasicThreshold = ({
    showDebug,
    tableId,
    tables,
    setTables,
    tableMetaIndex,
    areaIdentifier,
    deleteAction,
    onSaveFactory,
    availableDynamicTableOptions,
    tableNameMap,
    unitTypes,
    tableTag,
    setTableTag,
    inUse,
    setLocalTable,
    localTable,
}: DynamicTableProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [name, setItemName] = useState<string>("");

    const [tableRows, setTableRows] = useState<BasicThresholdData[]>([]);
    useEffect(() => {
        const tableRows = tables[tableMetaIndex].tableRows;
        setTableRows(tableRows);
    }, []);

    const modifier = new BasicThresholdModifier(updatedRows => {
        setTableRows(updatedRows);
    });

    const onSave = onSaveFactory(modifier, DynamicTableTypes.BasicThreshold, () => {});

    const addThresholdOnClick = () => modifier.addThreshold(tableRows, areaIdentifier, tableId, repository);

    return (
        <>
            <DynamicTableHeader
                localTable={localTable}
                setLocalTable={setLocalTable}
                setTables={setTables}
                availableDynamicTableOptions={availableDynamicTableOptions}
                tableNameMap={tableNameMap}
                unitTypes={unitTypes}
                inUse={inUse}
                tableTag={tableTag}
                setTableTag={setTableTag}
            />
            <div className={cls.container}>
                <Align direction="flex-start">
                    <TextInput
                        className={cls.input}
                        variant="standard"
                        label="Name to use in PDF fee table"
                        type="text"
                        value={tables[tableMetaIndex].tableRows[0].itemName}
                        InputLabelProps={{ className: cls.inputPropsCls }}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setItemName(tableRows, event.target.value);
                            setItemName(event.target.value);
                        }}
                    />
                </Align>
                <TableContainer>
                    <Table>
                        <BasicThresholdHeader tableData={tableRows} modifier={modifier} />
                        <BasicThresholdBody tableData={tableRows} modifier={modifier} unitPrettyName={tables[tableMetaIndex].tableMeta.unitPrettyName} unitGroup={tables[tableMetaIndex].tableMeta.unitGroup} />
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
            {showDebug && <DisplayTableData tableData={tableRows} />}
        </>
    );
};
