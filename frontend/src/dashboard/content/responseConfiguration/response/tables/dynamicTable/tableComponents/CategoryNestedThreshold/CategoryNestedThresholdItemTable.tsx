import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, Paper } from "@material-ui/core";
import { CategoryNestedThresholdData } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useContext } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { SaveBar } from "../../components/SaveBar";
import { CategoryNestedThresholdHeader } from "./CategoryNestedThresholdHeader";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { CategoryNestedThresholdRow } from "./CategoryNestedThresholdRow";

interface CategoryNestedThresholdItemTableProps {
    tableData: CategoryNestedThresholdData[];
    tableId: string;
    categoryIndex: number;
    categoryData: CategoryNestedThresholdData[];
    categoryName: string;
    categoryId: string;
    modifier: CategoryNestedThresholdModifier;
    areaIdentifier: string;
}

const useStyles = makeStyles(theme => ({
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

const getter = (x: CategoryNestedThresholdData) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const CategoryNestedThresholdItemTable = ({ categoryIndex, tableData, tableId, categoryData, categoryName, categoryId, modifier, areaIdentifier }: CategoryNestedThresholdItemTableProps) => {
    const [name, setCategoryName] = useState<string>("");

    const cls = useStyles();
    const { repository } = useContext(DashboardContext);

    useEffect(() => {
        setCategoryName(categoryName);
    }, []);
    const addThresholdOnClick = () => modifier.addThreshold(tableData, categoryId, repository, areaIdentifier, tableId);
    return (
        <>
            <TableContainer className={cls.tableStyles} component={Paper}>
                {categoryIndex === 0 && <CategoryNestedThresholdHeader tableData={tableData} modifier={modifier} />}
                <TableBody className={cls.body}>
                    {sortByPropertyNumeric(getter, categoryData).map((row: CategoryNestedThresholdData, rowIndex: number) => {
                        row.rowOrder = rowIndex;
                        return (
                            <CategoryNestedThresholdRow
                                key={row.rowId}
                                categorySize={categoryData.length}
                                categoryId={categoryId}
                                setCategoryName={setCategoryName}
                                categoryName={name}
                                rowIndex={rowIndex}
                                tableData={tableData}
                                row={row}
                                modifier={modifier}
                            />
                        );
                    })}
                </TableBody>
            </TableContainer>
            <SaveBar
                addInnerButton={
                    <Button onClick={addThresholdOnClick} color="primary" variant="contained">
                        Add Threshold
                    </Button>
                }
                deleteButton={
                    <Button variant="contained" style={{ width: "38ch" }} color="primary" onClick={() => modifier.removeCategory(tableData, categoryId)}>
                        Delete Category
                    </Button>
                }
            />
        </>
    );
};


