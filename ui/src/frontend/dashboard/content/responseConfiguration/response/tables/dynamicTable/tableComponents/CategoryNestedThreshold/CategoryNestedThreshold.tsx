import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { CategoryNestedThresholdData, DynamicTable, DynamicTableProps } from "@Palavyr-Types";

import { DisplayTableData } from "../DisplayTableData";
import { CategoryNestedThresholdContainer } from "./CategoryNestedThresholdContainer";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DynamicTableHeader } from "../../DynamicTableHeader";
import { cloneDeep } from "lodash";

const useStyles = makeStyles(theme => ({
    root: {
        borderTop: "3px solid red",
    },
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

export const CategoryNestedThreshold = ({
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
    useEffect(() => {
        setLocalTable(table);
    }, [table, tables, table.tableRows, localTable?.tableMeta.unitId, localTable?.tableMeta.unitPrettyName, localTable?.tableMeta.tableType, localTable?.tableRows]);

    const modifier = new CategoryNestedThresholdModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addCategory = async () => {
        if (localTable) await modifier.addCategory(localTable.tableRows, repository, areaIdentifier, tableId);
    };

    const onSave = async () => {
        if (localTable) {
            const result = modifier.validateTable(localTable.tableRows);

            if (result) {
                const currentMeta = localTable.tableMeta;

                const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(currentMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.saveDynamicTable<CategoryNestedThresholdData[]>(
                    areaIdentifier,
                    DynamicTableTypes.CategoryNestedThreshold,
                    localTable.tableRows,
                    localTable.tableMeta.tableId,
                    localTable.tableMeta.tableTag
                );

                tables[tableIndex].tableRows = updatedRows;
                tables[tableIndex].tableMeta = newTableMeta;
                setTables(cloneDeep(tables));

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
            <CategoryNestedThresholdContainer
                tableData={localTable.tableRows}
                modifier={modifier}
                tableId={tableId}
                areaIdentifier={areaIdentifier}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addCategory} color="primary" variant="contained">
                            Add Category
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} properties={["category", "triggerFallback", "threshold", "valueMin", "valueMax", "rowOrder", "rowId", "itemOrder", "itemId"]} />}
        </>
    ) : (
        <></>
    );
};
