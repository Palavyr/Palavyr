import { NodeTypeOptionResource, NodeTypeOptionResources } from "@common/types/api/ApiContracts";
import { PurchaseTypes } from "@common/types/api/Enums";
import { PlanTypeMeta } from "@Palavyr-Types";

export const filterNodeTypeOptionsOnSubscription = (nodeTypeOptions: NodeTypeOptionResources, planTypeMeta: PlanTypeMeta) => {
    const excludeFromFree: string[] = ["ShowImage", "ShowFileAsset"];
    const excludeFromLyte: string[] = ["ShowImage", "ShowFileAsset"];
    const excludeFromPremium: string[] = [];

    let filteredNodes = [...nodeTypeOptions];
    if (planTypeMeta.planType === PurchaseTypes.Premium) {
        filteredNodes = filteredNodes.filter((x: NodeTypeOptionResource) => !excludeFromPremium.includes(x.value));
    }
    if (planTypeMeta.planType === PurchaseTypes.Lyte) {
        filteredNodes = filteredNodes.filter((x: NodeTypeOptionResource) => !excludeFromLyte.includes(x.value));
    }

    if (planTypeMeta.planType === PurchaseTypes.Free) {
        filteredNodes = nodeTypeOptions.filter((x: NodeTypeOptionResource) => !excludeFromFree.includes(x.value));
    }
    return filteredNodes;
};
