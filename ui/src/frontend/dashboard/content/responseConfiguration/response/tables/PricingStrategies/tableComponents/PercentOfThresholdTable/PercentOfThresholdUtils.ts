import { sortByPropertyNumeric } from "@common/utils/sorting";
import { uniqBy } from "lodash";
import { PercentOfThresholdResource } from "@Palavyr-Types";

export const reOrderPercentOfThresholdTableData = (tableData: PercentOfThresholdResource[]) => {

    let reOrdered: PercentOfThresholdResource[] = [];

    const itemIds = tableData.map((row: PercentOfThresholdResource) => row.itemId)
    const uniqueItemIds = uniqBy(itemIds, (id: string) => id);

    uniqueItemIds.forEach((itemId: string) => {
        const itemGroup = tableData.filter((row: PercentOfThresholdResource) => row.itemId === itemId);

        const getter = (x: PercentOfThresholdResource) => x.threshold;
        const sortedByThreshold = sortByPropertyNumeric(getter, itemGroup);


        const itemGroupReordered: PercentOfThresholdResource[] = [];
        sortedByThreshold.forEach((row: PercentOfThresholdResource, index: number) => {
            row.rowOrder = index;
            itemGroupReordered.push(row);
        });
        reOrdered = [...reOrdered, ...itemGroupReordered]
    })
    return reOrdered;
}
