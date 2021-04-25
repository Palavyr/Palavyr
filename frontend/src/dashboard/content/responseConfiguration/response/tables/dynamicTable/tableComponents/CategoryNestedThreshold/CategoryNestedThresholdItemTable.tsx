import { ApiClient } from "@api-client/Client";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, Paper } from "@material-ui/core";
import { CategoryNestedThresholdData } from "@Palavyr-Types";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
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

const useStyles = makeStyles((theme) => ({
    input: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
    tableStyles: {
        background: "transparent",
    },
}));

const getter = (x: CategoryNestedThresholdData) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const CategoryNestedThresholdItemTable = ({ categoryIndex, tableData, tableId, categoryData, categoryName, categoryId, modifier, areaIdentifier }: CategoryNestedThresholdItemTableProps) => {
    const [name, setCategoryName] = useState<string>("");

    const cls = useStyles();
    const client = new ApiClient();

    useEffect(() => {
        setCategoryName(categoryName);
    }, []);

    return (
        <>
            <TableContainer className={cls.tableStyles} component={Paper}>
                {categoryIndex === 0 && <CategoryNestedThresholdHeader />}
                <TableBody>
                    {sortByPropertyNumeric(getter, categoryData).map((row: CategoryNestedThresholdData, index: number) => {
                        return <CategoryNestedThresholdRow key={row.rowId} categorySize={categoryData.length} categoryId={categoryId} setCategoryName={setCategoryName} categoryName={name} index={index} tableData={tableData} row={row} modifier={modifier} />;
                    })}
                </TableBody>
            </TableContainer>
            <ItemToolbar
                addInnerButton={
                    <Button onClick={() => modifier.addThreshold(tableData, categoryId, client, areaIdentifier, tableId)} color="primary" variant="contained">
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

interface IItemToolbar {
    deleteButton: JSX.Element;
    addInnerButton: JSX.Element;
}

export const ItemToolbar = ({ deleteButton, addInnerButton }: IItemToolbar) => {
    return (
        <>
            <br></br>
            <div style={{ marginBottom: "1rem" }}>
                <div style={{ float: "left", marginLeft: "1rem" }}>{addInnerButton}</div>
                <div style={{ float: "right", marginRight: "1rem" }}>{deleteButton}</div>
            </div>
            <br></br>
            <hr></hr>
        </>
    );
};
