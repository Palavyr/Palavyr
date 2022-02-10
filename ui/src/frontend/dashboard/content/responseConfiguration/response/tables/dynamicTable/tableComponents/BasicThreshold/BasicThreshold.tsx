import React, { useContext, useEffect } from "react";
import { BasicThresholdData, DynamicTable, DynamicTableProps } from "@Palavyr-Types";
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
import { DynamicTableHeader } from "../../DynamicTableHeader";
import { cloneDeep } from "lodash";
import { useIsMounted } from "@common/hooks/useIsMounted";

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

export const BasicThreshold = ({ showDebug, tableId, setTables, areaIdentifier, deleteAction, tables, tableIndex, availableDynamicTableOptions, tableNameMap, unitTypes, inUse, table }: DynamicTableProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);
    const [name, setItemName] = useState<string>("");
    const [localTable, setLocalTable] = useState<DynamicTable>();
    const isMounted = useIsMounted();

    useEffect(() => {
        if (isMounted) {
            setLocalTable(table);
        }
    }, [areaIdentifier, table, tables, table.tableRows, localTable?.tableMeta.unitId, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        if (isMounted) {
            (async () => {
                if (localTable) {
                    const { tableRows } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(localTable.tableMeta.areaIdentifier, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                    localTable.tableRows = tableRows;
                    setLocalTable(cloneDeep(localTable));
                }
            })();
        }
    }, [areaIdentifier, localTable?.tableMeta.tableType]);

    const modifier = new BasicThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addThresholdOnClick = async () => {
        if (localTable) await modifier.addThreshold(localTable.tableRows, areaIdentifier, tableId, repository);
    };

    const onSave = async () => {
        if (localTable) {
            const { isValid, tableRows } = modifier.validateTable(localTable.tableRows);

            if (isValid) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.saveDynamicTable<BasicThresholdData[]>(
                    areaIdentifier,
                    DynamicTableTypes.BasicThreshold,
                    tableRows,
                    localTable.tableMeta.tableId,
                    localTable.tableMeta.tableTag
                );

                const updatedTable = cloneDeep(localTable);
                updatedTable.tableRows = updatedRows;
                updatedTable.tableMeta = newTableMeta;
                setLocalTable(updatedTable);

                return true;
            } else {
                return false;
            }
        }
        return false;
    };

    return localTable ? (
        <>
            <DynamicTableHeader
                localTable={localTable}
                setLocalTable={setLocalTable}
                setTables={setTables}
                availableDynamicTableOptions={availableDynamicTableOptions}
                tableNameMap={tableNameMap}
                unitTypes={unitTypes}
                inUse={inUse}
            />
            <div className={cls.container}>
                <Align direction="flex-start">
                    <TextInput
                        className={cls.input}
                        variant="standard"
                        label="Name to use in PDF fee table"
                        type="text"
                        value={name}
                        InputLabelProps={{ className: cls.inputPropsCls }}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setItemName(localTable.tableRows, event.target.value);
                            setItemName(event.target.value);
                        }}
                    />
                </Align>
                <Table>
                    <BasicThresholdHeader tableData={localTable.tableRows} modifier={modifier} />
                    <BasicThresholdBody tableData={localTable.tableRows} modifier={modifier} unitPrettyName={localTable.tableMeta.unitPrettyName} unitGroup={localTable.tableMeta.unitGroup} />
                </Table>
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
            {showDebug && <DisplayTableData tableData={localTable.tableRows} />}
        </>
    ) : (
        <></>
    );
};
