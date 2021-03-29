import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, TextField, Paper } from "@material-ui/core";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import { TwoNestedCategoryData } from "../../DynamicTableTypes";
import { TwoNestedCategoriesHeader } from "./TwoNestedCategoriesHeader";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesRow } from "./TwoNestedCategoriesRow";

interface ITwoNestedCategoriesItemTable {
    tableData: TwoNestedCategoryData[];
    itemData: TwoNestedCategoryData[];
    outerCategory: string;
    itemId: string;
    modifier: TwoNestedCategoriesModifier;
    addRowOnClick(): void;
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

const getter = (x: TwoNestedCategoryData) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const TwoNestedCategoriesItemTable = ({ tableData, itemData, outerCategory, itemId, modifier, addRowOnClick }: ITwoNestedCategoriesItemTable) => {
    const [name, setItemName] = useState<string>("");

    const removeItem = (itemId: string) => {
        modifier.removeOuterCategory(tableData, itemId);
    };

    const cls = useStyles();

    useEffect(() => {
        setItemName(outerCategory);
    }, []);

    return (
        <>
            <TextField
                className={cls.input}
                variant="standard"
                type="text"
                value={outerCategory}
                color="primary"
                onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                    event.preventDefault();
                    modifier.setOuterCategoryName(tableData, itemId, event.target.value);
                    setItemName(event.target.value);
                }}
            />
            <TableContainer className={cls.tableStyles} component={Paper}>
                <TwoNestedCategoriesHeader />
                <TableBody>
                    {sortByPropertyNumeric(getter, itemData).map((row: TwoNestedCategoryData, index: number) => {
                        return <TwoNestedCategoriesRow key={row.rowId} tableData={tableData} row={row} modifier={modifier} />;
                    })}
                </TableBody>
            </TableContainer>
            <ItemToolbar
                addRowOnClick={addRowOnClick}
                removeItem={removeItem}
                itemId={itemId}
                addButton={
                    <Button variant="contained" style={{ width: "25ch" }} color="primary" onClick={addRowOnClick}>
                        Add sub category
                    </Button>
                }
                deleteButton={
                    <Button variant="contained" style={{ width: "18ch" }} color="primary" onClick={() => removeItem(itemId)}>
                        Delete Item
                    </Button>
                }
            />
        </>
    );
};

interface IItemToolbar {
    addRowOnClick(): void;
    removeItem(itemId: string): void;
    itemId: string;
    addButton: JSX.Element;
    deleteButton: JSX.Element;
}

export const ItemToolbar = ({ addRowOnClick, removeItem, itemId, addButton, deleteButton }: IItemToolbar) => {
    return (
        <>
            <br></br>
            <div style={{ marginBottom: "1rem" }}>
                <div style={{ float: "left", marginLeft: "1rem" }}>{addButton}</div>
                <div style={{ float: "right", marginRight: "1rem" }}>{deleteButton}</div>
            </div>
            <br></br>
            <hr></hr>
        </>
    );
};
