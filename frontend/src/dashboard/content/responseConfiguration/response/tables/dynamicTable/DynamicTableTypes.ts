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
    rowOrder: number;
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
    rowOrder: number;
};

export type BasicThresholdData = {
    id: number;
    rowId: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    itemName: string;
    threshold: number;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowOrder: number;
};

export type TwoNestedCategoryData = {
    id: number;
    accountId: string;
    areaIdentifier: string;
    tableId: string;
    valueMin: number;
    valueMax: number;
    range: boolean;
    rowId: string;
    rowOrder: number;
    itemId: string;
    itemOrder: number;
    category: string;
    subCategory: string;
};


export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | BasicThresholdData[] | TwoNestedCategoryData[] | any; // | SelectOneThresholdData etc

//These must be kept in sync
export const DynamicTableTypes = {
    SelectOneFlat: "SelectOneFlat",
    PercentOfThreshold: "PercentOfThreshold",
    BasicThreshold: "BasicThreshold",
    TwoNestedCategory: "TwoNestedCategory"
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
    showDebug: boolean;
};
