import { sortByPropertyNumeric } from "@common/utils/sorting";
import { BasicThresholdData } from "@Palavyr-Types";

export const reOrderBasicThresholdTableData = (tableData: any) => {
    const getter = (x: BasicThresholdData) => x.threshold;
    const sortedByThreshold = sortByPropertyNumeric(getter, tableData);

    const reOrdered: BasicThresholdData[] = [];
    let shouldReassignTriggerFallback = false;
    sortedByThreshold.forEach((row: BasicThresholdData, newRowNumber: number) => {
        console.log("STARTS FROM " + newRowNumber);

        row.rowOrder = newRowNumber;
        if (newRowNumber + 1 !== sortedByThreshold.length && row.triggerFallback) {
            row.triggerFallback = false;
            shouldReassignTriggerFallback = true;
        }

        if (newRowNumber + 1 === sortedByThreshold.length && shouldReassignTriggerFallback) {
            row.triggerFallback = true;
        }
        reOrdered.push(row);
    });
    return reOrdered;
};
