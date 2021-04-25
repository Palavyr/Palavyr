import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { CategoryNestedThresholdData, IDynamicTableBody } from "@Palavyr-Types";
import { CategoryNestedThresholdItemTable } from "./CategoryNestedThresholdItemTable";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";

interface CategoryNestedThresholdProps extends IDynamicTableBody {
    tableId: string;
    areaIdentifier: string;
    modifier: CategoryNestedThresholdModifier;
}

export const CategoryNestedThresholdContainer = ({ tableData, modifier, tableId, areaIdentifier }: CategoryNestedThresholdProps) => {
    const sortedByCategory = sortByPropertyNumeric(modifier.categoryIdGetter, tableData);

    const orderedCategoryGroups: TableGroup<CategoryNestedThresholdData[]> = groupBy(sortedByCategory, (x) => x.itemId); // use this groupby method in the modifier.

    return (
        <>
            {Object.keys(orderedCategoryGroups).map((categoryId: string, categoryIndex: number) => {
                const sortedRows: CategoryNestedThresholdData[] = sortByPropertyNumeric(modifier.rowOrderGetter, orderedCategoryGroups[categoryId]);
                const categoryName = sortedRows[0].category;
                return (
                    <CategoryNestedThresholdItemTable
                        key={categoryIndex}
                        tableId={tableId}
                        areaIdentifier={areaIdentifier}
                        categoryIndex={categoryIndex}
                        tableData={tableData}
                        categoryData={sortedRows}
                        categoryName={categoryName}
                        categoryId={categoryId}
                        modifier={modifier}
                    />
                );
            })}
        </>
    );
};
