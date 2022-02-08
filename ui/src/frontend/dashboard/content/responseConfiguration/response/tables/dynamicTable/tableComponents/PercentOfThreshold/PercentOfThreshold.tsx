import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTable, DynamicTableProps, PercentOfThresholdData } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdContainer } from "./PercentOfThresholdContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DynamicTableHeader } from "../../DynamicTableHeader";
import { cloneDeep } from "lodash";

const useStyles = makeStyles(theme => ({
    root: {},
    tableStyles: {
        background: "transparent",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {},
    alignLeft: {
        float: "left",
    },
    alignRight: {
        float: "right",
    },
}));

export const PercentOfThreshold = ({
    showDebug,
    tableId,
    setTables,
    areaIdentifier,
    deleteAction,
    tables,
    tableIndex,
    availableDynamicTableOptions,
    tableNameMap,
    unitTypes,
    inUse,
    table,
}: DynamicTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [localTable, setLocalTable] = useState<DynamicTable>();
    const [loaded, setLoaded] = useState(false);

    useEffect(() => {
        setLocalTable(table);
        setLoaded(true);
    }, [table, tables, table.tableRows, localTable?.tableMeta.unitId, localTable?.tableMeta.unitPrettyName]);

    useEffect(() => {
        (async () => {
            if (localTable && loaded) {
                const { tableRows } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(localTable.tableMeta.areaIdentifier, localTable.tableMeta.tableType, localTable.tableMeta.tableId);
                localTable.tableRows = tableRows;
                setLocalTable(cloneDeep(localTable));
            }
        })();

    }, [localTable?.tableMeta.tableType]);

    const modifier = new PercentOfThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addItemOnClick = async () => {
        if (localTable) await modifier.addItem(localTable.tableRows, repository, areaIdentifier, tableId);
    };

    const addRowOnClickFactory = (itemId: string) => async () => {
        if (localTable) await modifier.addRow(localTable.tableRows, repository, areaIdentifier, tableId, itemId);
    };

    const onSave = async () => {
        if (localTable) {
            const result = modifier.validateTable(localTable.tableRows);

            if (result) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.saveDynamicTable<PercentOfThresholdData[]>(
                    areaIdentifier,
                    DynamicTableTypes.PercentOfThreshold,
                    localTable.tableRows,
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
            <PercentOfThresholdContainer
                tableData={localTable.tableRows}
                modifier={modifier}
                addRowOnClickFactory={addRowOnClickFactory}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Item
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
