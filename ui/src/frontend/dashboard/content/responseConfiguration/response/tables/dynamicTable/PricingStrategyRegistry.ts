import { BasicThreshold } from "./tableComponents/BasicThreshold/BasicThreshold";
import { CategoryNestedThreshold } from "./tableComponents/CategoryNestedThreshold/CategoryNestedThreshold";
import { PercentOfThreshold } from "./tableComponents/PercentOfThreshold/PercentOfThreshold";
import { SelectOneFlat } from "./tableComponents/SelectOneFlat/SelectOneFlat";
import { TwoNestedCategories } from "./tableComponents/TwoNestedCategories/TwoNestedCategories";

//These must be kept in sync
export enum PricingStrategyTypes {
    SelectOneFlat = "SelectOneFlat",
    PercentOfThreshold = "PercentOfThreshold",
    BasicThreshold = "BasicThreshold",
    TwoNestedCategory = "TwoNestedCategory",
    CategoryNestedThreshold = "CategoryNestedThreshold",
}

export const PricingStrategyComponentMap = {
    [PricingStrategyTypes.SelectOneFlat]: SelectOneFlat,
    [PricingStrategyTypes.PercentOfThreshold]: PercentOfThreshold,
    [PricingStrategyTypes.BasicThreshold]: BasicThreshold,
    [PricingStrategyTypes.TwoNestedCategory]: TwoNestedCategories,
    [PricingStrategyTypes.CategoryNestedThreshold]: CategoryNestedThreshold,
};
