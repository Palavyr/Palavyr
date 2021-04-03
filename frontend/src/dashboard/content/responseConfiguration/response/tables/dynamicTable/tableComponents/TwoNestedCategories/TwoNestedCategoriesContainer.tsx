import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, TwoNestedCategoryData } from "../../DynamicTableTypes";
import { TwoNestedCategoriesItemTable } from "./TwoNestedCategoriesItemTable";

interface ITwoNestedCategoriesContainer extends IDynamicTableBody {
    addInnerCategory(): void;
}

export const TwoNestedCategoriesContainer = ({ addInnerCategory, tableData, modifier }: ITwoNestedCategoriesContainer) => {
    const outerCategoryGroups: TableGroup<TwoNestedCategoryData[]> = groupBy(tableData, (x) => x.itemId); // use this groupby method in the modifier.

    return (
        <>
            {Object.keys(outerCategoryGroups).map((outerCategoryId: string, outerCategoryIndex: number) => {
                const itemData: TwoNestedCategoryData[] = outerCategoryGroups[outerCategoryId];

                return (
                    <TwoNestedCategoriesItemTable
                        key={outerCategoryIndex}
                        outerCategoryIndex={outerCategoryIndex}
                        tableData={tableData}
                        outerCategoryId={outerCategoryId}
                        outerCategoryData={itemData}
                        outerCategoryName={itemData[0].category} // TODO: is there a better way to get this?
                        modifier={modifier}
                        addInnerCategory={addInnerCategory}
                    />
                );
            })}
        </>
    );
};
