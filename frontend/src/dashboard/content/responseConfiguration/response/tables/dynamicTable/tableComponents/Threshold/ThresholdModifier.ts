import { cloneDeep } from "lodash";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { DynamicTableTypes, TableData } from "../../DynamicTableTypes";

export class ThresholdModifier {

    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.Threshold;
    }

    setTables(newState: TableData) {
        this.onClick(cloneDeep(newState));
    }

    addThreshold(tableData: TableData) {
        console.log("adding new thershold")
    }

    setThresholdValue(tableData: TableData, rowId: number, value: number) {
        console.log("setting new threshold value")
    }

    setValueMin(tableData: TableData, rowId: number, value: number) {
        console.log("seetting the value Min")
    }

    setValueMax(tableData: TableData, rowId: number, value: number) {
        console.log("Setting the value Max");
    }

    setRangeOrValue(tableData: TableData, rowId: number) {
        console.log("Setting the range or value");
    }

}