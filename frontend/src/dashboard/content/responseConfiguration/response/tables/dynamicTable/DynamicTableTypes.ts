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
    areaId: string;
    option: string;
    valueMin: number;
    valueMax: number;
    range: number;
    threshhold: number;
};

export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | any; // | SelectOneThresholdData etc

//These must be kept in sync
export const DynamicTableTypes = {
    SelectOneFlat: "SelectOneFlat",
    PercentOfThreshold: "PercentOfThreshold",
};

export interface IDynamicTableBody {
    tableData: TableData;
    modifier: any;
}

export type IDynamicTableProps = {
    tableData: Array<SelectOneFlatData>;
    setTableData: Dispatch<SetStateAction<TableData>>;
    areaIdentifier: string;
    tableId: string;
    tableTag: string;
    tableMeta: DynamicTableMeta;
    setTableMeta: any;
    deleteAction(): void;
};
