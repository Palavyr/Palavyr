import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, TwoNestedCategoryData } from "@Palavyr-Types";
import { TwoNestedCategoriesItemTable } from "./TwoNestedCategoriesItemTable";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { sortByPropertyNumeric } from "@common/utils/sorting";

interface ITwoNestedCategoriesContainer extends IDynamicTableBody {
    addInnerCategory(): void;
    modifier: TwoNestedCategoriesModifier;
}

export const TwoNestedCategoriesContainer = ({ addInnerCategory, tableData, modifier }: ITwoNestedCategoriesContainer) => {
    const sortedByOuterCategory = sortByPropertyNumeric(modifier.outerCategoryOrderGetter, tableData);
    const orderedOuterCategoryGroups: TableGroup<TwoNestedCategoryData[]> = groupBy(sortedByOuterCategory, (x) => x.itemId); // use this groupby method in the modifier.

    return (
        <>
            {Object.keys(orderedOuterCategoryGroups).map((outerCategoryId: string, outerCategoryIndex: number) => {
                const sortedRows: TwoNestedCategoryData[] = sortByPropertyNumeric(modifier.innerCategoryOrderGetter, orderedOuterCategoryGroups[outerCategoryId]);
                const categoryName = sortedRows[0].category;
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
        </>
    );
};
