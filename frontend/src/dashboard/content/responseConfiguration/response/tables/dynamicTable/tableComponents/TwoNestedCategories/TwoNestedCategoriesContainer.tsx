import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, TwoNestedCategoryData } from "../../DynamicTableTypes";
import { TwoNestedCategoriesItemTable } from "./TwoNestedCategoriesItemTable";

interface ITwoNestedCategoriesContainer extends IDynamicTableBody {
    addRowOnClickFactory(itemId: string): () => void;
}

type TableGroup = {
    [itemGroup: string]: TwoNestedCategoryData[];
};

export const TwoNestedCategoriesContainer = ({ tableData, modifier, addRowOnClickFactory }: ITwoNestedCategoriesContainer) => {
    const outerCategoryGroups: TableGroup = groupBy(tableData, (x) => x.itemId);

    return (
        <>
            {Object.keys(outerCategoryGroups).map((itemId: string, index: number) => {
                const itemData: TwoNestedCategoryData[] = outerCategoryGroups[itemId];

                return (
                    <TwoNestedCategoriesItemTable
                        key={index}
                        tableData={tableData}
                        itemId={itemId}
                        itemData={itemData}
                        outerCategory={itemData[0].category} // TODO: is there a better way to get this?
                        modifier={modifier}
                        addRowOnClick={addRowOnClickFactory(itemId)}
                    />
                );
            })}
        </>
    );
};
