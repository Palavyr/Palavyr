import { Dispatch, SetStateAction } from "react";
import { DynamicTableTypes, TableData } from "../../DynamicTableTypes";
import { cloneDeep } from "lodash";
import { ApiClient } from "@api-client/Client";


export class PercentOfThresholdModifier {

    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.SelectOneFlat;
    }

    setTables(newState: TableData) {
        this.onClick(cloneDeep(newState));
    }


}