import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, Paper } from "@material-ui/core";
import { TwoNestedCategoryData } from "@Palavyr-Types";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import { ButtonBar } from "../../components/SaveBar";
import { TwoNestedCategoriesHeader } from "./TwoNestedCategoriesHeader";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesRow } from "./TwoNestedCategoriesRow";

interface ITwoNestedCategoriesItemTable {
    tableData: TwoNestedCategoryData[];
    outerCategoryIndex: number;
    outerCategoryData: TwoNestedCategoryData[];
    outerCategoryName: string;
    outerCategoryId: string;
    modifier: TwoNestedCategoriesModifier;
    addInnerCategory(): void;
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
}));

const getter = (x: TwoNestedCategoryData) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const TwoNestedCategoriesItemTable = ({ outerCategoryIndex, tableData, outerCategoryData, outerCategoryName, outerCategoryId, modifier, addInnerCategory }: ITwoNestedCategoriesItemTable) => {
    const [name, setOuterCategoryName] = useState<string>("");

    const removeOuterCategory = (outerCategoryId: string) => {
        modifier.removeOuterCategory(tableData, outerCategoryId);
    };

    const cls = useStyles();

    useEffect(() => {
        setOuterCategoryName(outerCategoryName);
    }, []);

    return (
        <>
            <TableContainer className={cls.tableStyles} component={Paper}>
                {outerCategoryIndex === 0 && <TwoNestedCategoriesHeader />}
                <TableBody>
                    {sortByPropertyNumeric(getter, outerCategoryData).map((row: TwoNestedCategoryData, index: number) => {
                        return (
                            <TwoNestedCategoriesRow
                                key={row.rowId}
                                shouldDisableInnerCategory={outerCategoryIndex > 0}
                                outerCategoryId={outerCategoryId}
                                setOuterCategoryName={setOuterCategoryName}
                                outerCategoryName={name}
                                index={index}
                                tableData={tableData}
                                row={row}
                                modifier={modifier}
                            />
                        );
                    })}
                </TableBody>
            </TableContainer>
            <ButtonBar
                addInnerButton={
                    outerCategoryIndex === 0 ? (
                        <Button onClick={addInnerCategory} color="primary" variant="contained">
                            Add Inner Category
                        </Button>
                    ) : (
                        <></>
                    )
                }
                deleteButton={
                    <Button variant="contained" style={{ width: "38ch"}} color="primary" onClick={() => removeOuterCategory(outerCategoryId)}>
                        Delete Outer Category
                    </Button>
                }
            />
        </>
    );
};


