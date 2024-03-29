import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, Paper, Table } from "@material-ui/core";
import { CategoryNestedThresholdResource, UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { ButtonBar } from "../../components/SaveBar";
import { CategoryNestedThresholdHeader } from "./CategoryNestedThresholdHeader";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { CategoryNestedThresholdRow } from "./CategoryNestedThresholdRow";

interface CategoryNestedThresholdItemTableProps {
    tableData: CategoryNestedThresholdResource[];
    tableId: string;
    categoryIndex: number;
    categoryData: CategoryNestedThresholdResource[];
    categoryName: string;
    categoryId: string;
    modifier: CategoryNestedThresholdModifier;
    intentId: string;
    unitGroup?: UnitGroups;
    unitPrettyName?: UnitPrettyNames;
}

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    input: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
    tableStyles: {
        background: "transparent",
        boxShadow: "none",
        border: "0px solid black",
    },
    body: {
        boxShadow: "none",
        border: "0px solid black",
    },
}));

const getter = (x: CategoryNestedThresholdResource) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const CategoryNestedThresholdItemTable = ({
    categoryIndex,
    tableData,
    tableId,
    categoryData,
    categoryName,
    categoryId,
    modifier,
    intentId,
    unitPrettyName,
    unitGroup,
}: CategoryNestedThresholdItemTableProps) => {
    const cls = useStyles();
    const { repository } = useContext(DashboardContext);

    const addThresholdOnClick = () => modifier.addThreshold(tableData, categoryId, repository, intentId, tableId);
    return (
        <>
            <Table className={cls.tableStyles}>
                {categoryIndex === 0 && <CategoryNestedThresholdHeader />}
                <TableBody className={cls.body}>
                    {sortByPropertyNumeric(getter, categoryData).map((row: CategoryNestedThresholdResource, rowIndex: number) => {
                        row.rowOrder = rowIndex;
                        return (
                            <React.Fragment key={rowIndex}>
                                {unitGroup && unitPrettyName && row ? (
                                    <CategoryNestedThresholdRow
                                        key={row.rowId}
                                        categorySize={categoryData.length}
                                        categoryId={categoryId}
                                        categoryName={categoryName}
                                        rowIndex={rowIndex}
                                        tableData={tableData}
                                        row={row}
                                        modifier={modifier}
                                        unitGroup={unitGroup}
                                        unitPrettyName={unitPrettyName}
                                    />
                                ) : (
                                    <></>
                                )}
                            </React.Fragment>
                        );
                    })}
                </TableBody>
            </Table>
            <ButtonBar
                addInnerButton={
                    <Button onClick={addThresholdOnClick} color="primary" variant="contained">
                        Add Threshold
                    </Button>
                }
                deleteButton={
                    <Button variant="contained" style={{ width: "38ch" }} color="primary" onClick={() => modifier.removeCategory(tableData, categoryId)}>
                        Delete {categoryName}
                    </Button>
                }
            />
        </>
    );
};
