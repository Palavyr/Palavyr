import { sortByPropertyNumeric } from "@common/utils/sorting";
import { uniqBy } from "lodash";
import { PercentOfThresholdData } from "../../DynamicTableTypes";

export const reOrderPercentOfThresholdTableData = (tableData: PercentOfThresholdData[]) => {

    let reOrdered: PercentOfThresholdData[] = [];

    const itemIds = tableData.map((row: PercentOfThresholdData) => row.itemId)
    const uniqueItemIds = uniqBy(itemIds, (id: string) => id);

    uniqueItemIds.forEach((itemId: string) => {
        const itemGroup = tableData.filter((row: PercentOfThresholdData) => row.itemId === itemId);

        const getter = (x: PercentOfThresholdData) => x.threshold;
        const sortedByThreshold = sortByPropertyNumeric(getter, itemGroup);


        const itemGroupReordered: PercentOfThresholdData[] = [];
        sortedByThreshold.forEach((row: PercentOfThresholdData, index: number) => {
            row.rowOrder = index;
            itemGroupReordered.push(row);
        });
        reOrdered = [...reOrdered, ...itemGroupReordered]
    })
    return reOrdered;
}
