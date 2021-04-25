import { sortByPropertyNumeric } from "@common/utils/sorting";
import { BasicThresholdData } from "@Palavyr-Types";

export const reOrderBasicThresholdTableData = (tableData: any) => {
    const getter = (x: BasicThresholdData) => x.threshold;
    const sortedByThreshold = sortByPropertyNumeric(getter, tableData);

    const reOrdered: BasicThresholdData[] = [];
    sortedByThreshold.forEach((row: BasicThresholdData, index: number) => {
        row.rowOrder = index;
        reOrdered.push(row);
    });
    return reOrdered;
}
