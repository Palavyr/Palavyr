import { BasicThreshold } from "./tableComponents/SimpleThresholdTable/SimpleThreshold";
import { CategoryNestedThreshold } from "./tableComponents/CategoryNestedThresholdTable/CategoryNestedThreshold";
import { PercentOfThreshold } from "./tableComponents/PercentOfThresholdTable/PercentOfThreshold";
import { CategorySelect } from "./tableComponents/CategorySelectTable/CaetgorySelect";
import { TwoNestedCategories } from "./tableComponents/SelectWithNestedSelectTable/SelectWithNestedSelect";

//These must be kept in sync
export enum PricingStrategyTypes {
    CategorySelect = "CategorySelect",
    PercentOfThreshold = "PercentOfThreshold",
    BasicThreshold = "BasicThreshold",
    TwoNestedCategory = "TwoNestedCategory",
    CategoryNestedThreshold = "CategoryNestedThreshold",
}

export const PricingStrategyComponentMap = {
    [PricingStrategyTypes.CategorySelect]: CategorySelect,
    [PricingStrategyTypes.PercentOfThreshold]: PercentOfThreshold,
    [PricingStrategyTypes.BasicThreshold]: BasicThreshold,
    [PricingStrategyTypes.TwoNestedCategory]: TwoNestedCategories,
    [PricingStrategyTypes.CategoryNestedThreshold]: CategoryNestedThreshold,
};
