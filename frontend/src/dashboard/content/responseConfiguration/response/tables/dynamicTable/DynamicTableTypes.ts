import { DynamicTableMeta } from "@Palavyr-Types";
import { Dispatch, SetStateAction } from "react";

// Dynamic Table types
export type SelectOneFlatData = {
    id: number;
    accountId: string;
    areaId: string;
    tableId: string;
    option: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
};

export type PercentOfThresholdData = {
    id: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    itemId: string;
    itemName: string;
    rowId: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    modifier: number;
    posNeg: boolean;
};

export type ThresholdData = {
    id: number;
    rowId: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
};

export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | ThresholdData[] | any; // | SelectOneThresholdData etc

//These must be kept in sync
export const DynamicTableTypes = {
    SelectOneFlat: "SelectOneFlat",
    PercentOfThreshold: "PercentOfThreshold",
    Threshold: "Threshold",
};

export interface IDynamicTableBody {
    tableData: TableData;
    modifier: any;
}

export type IDynamicTableProps = {
    tableData: Array<TableData>;
    setTableData: Dispatch<SetStateAction<TableData>>;
    areaIdentifier: string;
    tableId: string;
    tableTag: string;
    tableMeta: DynamicTableMeta;
    setTableMeta: any;
    deleteAction(): Promise<any>;
};
