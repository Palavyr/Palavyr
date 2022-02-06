import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTable, DynamicTableProps, TwoNestedCategoryData } from "@Palavyr-Types";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesContainer } from "./TwoNestedCategoriesContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { cloneDeep } from "lodash";
import { DynamicTableHeader } from "../../DynamicTableHeader";

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

export const TwoNestedCategories = ({
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

    const modifier = new TwoNestedCategoriesModifier(updatedRows => {
        if (localTable) {
            localTable.tableRows = updatedRows;
            localTable.tableMeta = localTable.tableMeta;
        }
        setLocalTable(cloneDeep(localTable));
    });

    const addOuterCategory = async () => {
        if (localTable) await modifier.addOuterCategory(localTable.tableRows, repository, areaIdentifier, tableId);
    };

    const addInnerCategory = async () => {
        if (localTable) await modifier.addInnerCategory(localTable.tableRows, repository, areaIdentifier, tableId);
    };

    const onSave = async () => {
        if (localTable) {
            const result = modifier.validateTable(localTable.tableRows);

            if (result) {
                const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(localTable.tableMeta);
                const updatedRows = await repository.Configuration.Tables.Dynamic.saveDynamicTable<TwoNestedCategoryData[]>(
                    areaIdentifier,
                    DynamicTableTypes.TwoNestedCategory,
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
            <TwoNestedCategoriesContainer
                addInnerCategory={addInnerCategory}
                tableData={localTable.tableRows}
                modifier={modifier}
                unitPrettyName={localTable.tableMeta.unitPrettyName}
                unitGroup={localTable.tableMeta.unitGroup}
            />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addOuterCategory} color="primary" variant="contained">
                            Add Outer Category
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={localTable.tableRows} properties={["category", "subCategory"]} />}
        </>
    ) : (
        <></>
    );
};
