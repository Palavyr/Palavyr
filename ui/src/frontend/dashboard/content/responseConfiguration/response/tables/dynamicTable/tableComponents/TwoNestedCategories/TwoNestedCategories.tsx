import React, { useContext, useEffect, useState } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableProps, TwoNestedCategoryData } from "@Palavyr-Types";
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
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const [tableRows, setTableRows] = useState<TwoNestedCategoryData[]>([]);
    useEffect(() => {
        const tableRows = tables[tableMetaIndex].tableRows;
        setTableRows(tableRows);
    }, []);
    const modifier = new TwoNestedCategoriesModifier(updatedRows => {
        setTableRows(updatedRows);
    });

    const addOuterCategory = () => modifier.addOuterCategory(tableRows, repository, areaIdentifier, tableId);
    const addInnerCategory = () => modifier.addInnerCategory(tableRows, repository, areaIdentifier, tableId);

    const onSave = onSaveFactory(modifier, DynamicTableTypes.TwoNestedCategory, () => {});

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
            <TwoNestedCategoriesContainer addInnerCategory={addInnerCategory} tableData={tableRows} modifier={modifier} />
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
            {showDebug && <DisplayTableData tableData={tableRows} properties={["category", "subCategory"]} />}
        </>
    );
};
