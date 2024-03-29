import { TwoNestedCategoryResource } from "@common/types/api/EntityResources";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { takeNCharacters } from "@common/utils/textSlicing";
import { Button, makeStyles, TableBody, Table } from "@material-ui/core";
import React from "react";
import { ButtonBar } from "../../components/SaveBar";
import { TwoNestedCategoriesHeader } from "./SelectWithNestedSelectHeader";
import { TwoNestedCategoriesModifier } from "./SelectWithNestedSelectModifier";
import { TwoNestedCategoriesRow } from "./SelectWithNestedSelectRow";

interface ITwoNestedCategoriesItemTable {
    tableData: TwoNestedCategoryResource[];
    outerCategoryIndex: number;
    outerCategoryData: TwoNestedCategoryResource[];
    outerCategoryName: string;
    outerCategoryId: string;
    modifier: TwoNestedCategoriesModifier;
    addInnerCategory(): void;
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
}));

const getter = (x: TwoNestedCategoryResource) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const TwoNestedCategoriesItemTable = ({ outerCategoryIndex, tableData, outerCategoryData, outerCategoryName, outerCategoryId, modifier, addInnerCategory }: ITwoNestedCategoriesItemTable) => {
    const removeOuterCategory = (outerCategoryId: string) => {
        modifier.removeOuterCategory(tableData, outerCategoryId);
    };

    const cls = useStyles();

    return (
        <>
            <Table className={cls.tableStyles}>
                <TwoNestedCategoriesHeader show={outerCategoryIndex === 0} />
                <TableBody>
                    {sortByPropertyNumeric(getter, outerCategoryData).map((row: TwoNestedCategoryResource, index: number) => {
                        return (
                            <React.Fragment key={index}>
                                {row && (
                                    <TwoNestedCategoriesRow
                                        key={row.rowId}
                                        shouldDisableInnerCategory={outerCategoryIndex > 0}
                                        outerCategoryId={outerCategoryId}
                                        outerCategoryName={outerCategoryName}
                                        index={index}
                                        tableData={tableData}
                                        row={row}
                                        modifier={modifier}
                                    />
                                )}
                            </React.Fragment>
                        );
                    })}
                </TableBody>
            </Table>
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
                    <Button variant="contained" style={{ width: "38ch" }} color="primary" onClick={() => removeOuterCategory(outerCategoryId)}>
                        Delete {takeNCharacters(outerCategoryName)}
                    </Button>
                }
            />
        </>
    );
};
