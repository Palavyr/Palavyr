import { DynamicTableMeta } from "@Palavyr-Types";
import { Dispatch, SetStateAction } from "react";
import { BasicThreshold } from "./tableComponents/BasicThreshold/BasicThreshold";
import { PercentOfThreshold } from "./tableComponents/PercentOfThreshold/PercentOfThreshold";
import { SelectOneFlat } from "./tableComponents/SelectOneFlat/SelectOneFlat";
import { TwoNestedCategories } from "./tableComponents/TwoNestedCategories/TwoNestedCategories";

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

export type CategoryNestedThresholdData = {
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
    threshold: number;
};

export type TableData = SelectOneFlatData[] | PercentOfThresholdData[] | BasicThresholdData[] | TwoNestedCategoryData[] | CategoryNestedThresholdData[] | any; // | SelectOneThresholdData etc

//These must be kept in sync
export const DynamicTableTypes = {
    SelectOneFlat: "SelectOneFlat",
    PercentOfThreshold: "PercentOfThreshold",
    BasicThreshold: "BasicThreshold",
    TwoNestedCategory: "TwoNestedCategory",
    CategoryNestedThreshold: "CategoryNestedThreshold",
};

export interface IDynamicTableBody {
    tableData: TableData;
    modifier: any;
}

export type DynamicTableProps = {
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

export type DynamicTableComponentMap = {
    [key: string]: (props: DynamicTableProps) => JSX.Element;
};

export const DynamicTableComponentMap: DynamicTableComponentMap = {
    [DynamicTableTypes.SelectOneFlat]: SelectOneFlat,
    [DynamicTableTypes.PercentOfThreshold]: PercentOfThreshold,
    [DynamicTableTypes.BasicThreshold]: BasicThreshold,
    [DynamicTableTypes.TwoNestedCategory]: TwoNestedCategories,
    [DynamicTableTypes.CategoryNestedThreshold]: TwoNestedCategories,
};
