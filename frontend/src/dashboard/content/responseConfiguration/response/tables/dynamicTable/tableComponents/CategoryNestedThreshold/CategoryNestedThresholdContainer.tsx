import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { CategoryNestedThresholdData, IDynamicTableBody } from "../../DynamicTableTypes";
import { CategoryNestedThresholdItemTable } from "./CategoryNestedThresholdItemTable";

interface CategoryNestedThresholdProps extends IDynamicTableBody {
    tableId: string;
    areaIdentifier: string;
}

export const CategoryNestedThresholdContainer = ({ tableData, modifier, tableId, areaIdentifier }: CategoryNestedThresholdProps) => {
    const categoryGroups: TableGroup<CategoryNestedThresholdData[]> = groupBy(tableData, (x) => x.itemId); // use this groupby method in the modifier.

    return (
        <>
            {Object.keys(categoryGroups).map((categoryId: string, categoryIndex: number) => {
                const itemData: CategoryNestedThresholdData[] = categoryGroups[categoryId];

                return (
                    <CategoryNestedThresholdItemTable
                        key={categoryIndex}
                        tableId={tableId}
                        areaIdentifier={areaIdentifier}
                        categoryIndex={categoryIndex}
                        tableData={tableData}
                        categoryData={itemData}
                        categoryName={itemData[0].category}
                        categoryId={categoryId}
                        modifier={modifier}
                    />
                );
            })}
        </>
    );
};
