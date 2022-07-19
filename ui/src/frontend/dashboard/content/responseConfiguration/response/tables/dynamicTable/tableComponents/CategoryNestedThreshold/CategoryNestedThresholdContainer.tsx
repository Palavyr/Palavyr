import { TableGroup } from "@Palavyr-Types";
import { groupBy } from "lodash";
import React from "react";
import { CategoryNestedThresholdData, IPricingStrategyBody } from "@Palavyr-Types";
import { CategoryNestedThresholdItemTable } from "./CategoryNestedThresholdItemTable";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    container: {
        borderBottom: `4px solid ${theme.palette.primary.main}`,
    },
}));

export interface CategoryNestedThresholdProps extends IPricingStrategyBody {
    tableId: string;
    intentId: string;
    modifier: CategoryNestedThresholdModifier;
}

export const CategoryNestedThresholdContainer = ({ tableData, modifier, tableId, intentId, unitGroup, unitPrettyName }: CategoryNestedThresholdProps) => {
    const cls = useStyles();
    const sortedByCategory = sortByPropertyNumeric(modifier.itemOrderGetter, tableData);

    const orderedCategoryGroups: TableGroup<CategoryNestedThresholdData[]> = groupBy(sortedByCategory, x => x.itemId); // use this groupby method in the modifier.

    return (
        <div className={cls.container}>
            {Object.keys(orderedCategoryGroups).map((categoryId: string, categoryIndex: number) => {
                const sortedRows: CategoryNestedThresholdData[] = sortByPropertyNumeric(modifier.rowOrderGetter, orderedCategoryGroups[categoryId]);
                const categoryName = sortedRows[0].itemName;
                return (
                    <CategoryNestedThresholdItemTable
                        key={categoryIndex}
                        tableId={tableId}
                        intentId={intentId}
                        categoryIndex={categoryIndex}
                        tableData={tableData}
                        categoryData={sortedRows}
                        categoryName={categoryName}
                        categoryId={categoryId}
                        modifier={modifier}
                        unitPrettyName={unitPrettyName}
                        unitGroup={unitGroup}
                    />
                );
            })}
        </div>
    );
};
