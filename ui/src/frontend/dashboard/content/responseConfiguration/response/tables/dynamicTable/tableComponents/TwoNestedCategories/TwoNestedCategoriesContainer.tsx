import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, TwoNestedCategoryData } from "@Palavyr-Types";
import { TwoNestedCategoriesItemTable } from "./TwoNestedCategoriesItemTable";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    container: {
        border: "0px solid black",
        borderTop: `4px solid ${theme.palette.primary.main}`,
        borderBottom: `4px solid ${theme.palette.primary.main}`,
        boxShadow: "none",
    },
}));

export interface ITwoNestedCategoriesContainer extends IDynamicTableBody {
    addInnerCategory(): void;
    modifier: TwoNestedCategoriesModifier;
}

export const TwoNestedCategoriesContainer = ({ addInnerCategory, tableData, modifier }: ITwoNestedCategoriesContainer) => {
    const cls = useStyles();
    const sortedByOuterCategory = sortByPropertyNumeric(modifier.outerCategoryOrderGetter, tableData);
    const orderedOuterCategoryGroups: TableGroup<TwoNestedCategoryData[]> = groupBy(sortedByOuterCategory, x => x.itemId); // use this groupby method in the modifier.

    return (
        <div className={cls.container}>
            {Object.keys(orderedOuterCategoryGroups).map((outerCategoryId: string, outerCategoryIndex: number) => {
                const sortedRows: TwoNestedCategoryData[] = sortByPropertyNumeric(modifier.innerCategoryOrderGetter, orderedOuterCategoryGroups[outerCategoryId]);
                const categoryName = sortedRows[0].itemName;
                return (
                    <TwoNestedCategoriesItemTable
                        key={outerCategoryIndex}
                        outerCategoryIndex={outerCategoryIndex}
                        tableData={tableData}
                        outerCategoryId={outerCategoryId}
                        outerCategoryData={sortedRows}
                        outerCategoryName={categoryName}
                        modifier={modifier}
                        addInnerCategory={addInnerCategory}
                    />
                );
            })}
        </div>
    );
};