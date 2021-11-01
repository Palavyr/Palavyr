import { BasicThreshold } from "./tableComponents/BasicThreshold/BasicThreshold";
import { CategoryNestedThreshold } from "./tableComponents/CategoryNestedThreshold/CategoryNestedThreshold";
import { PercentOfThreshold } from "./tableComponents/PercentOfThreshold/PercentOfThreshold";
import { SelectOneFlat } from "./tableComponents/SelectOneFlat/SelectOneFlat";
import { TwoNestedCategories } from "./tableComponents/TwoNestedCategories/TwoNestedCategories";
import { DynamicTableComponentMap } from "@Palavyr-Types";

//These must be kept in sync
export const DynamicTableTypes = {
    SelectOneFlat: "SelectOneFlat",
    PercentOfThreshold: "PercentOfThreshold",
    BasicThreshold: "BasicThreshold",
    TwoNestedCategory: "TwoNestedCategory",
    CategoryNestedThreshold: "CategoryNestedThreshold",
};

export const dynamicTableComponentMap: DynamicTableComponentMap = {
    [DynamicTableTypes.SelectOneFlat]: SelectOneFlat,
    [DynamicTableTypes.PercentOfThreshold]: PercentOfThreshold,
    [DynamicTableTypes.BasicThreshold]: BasicThreshold,
    [DynamicTableTypes.TwoNestedCategory]: TwoNestedCategories,
    [DynamicTableTypes.CategoryNestedThreshold]: CategoryNestedThreshold,
};
