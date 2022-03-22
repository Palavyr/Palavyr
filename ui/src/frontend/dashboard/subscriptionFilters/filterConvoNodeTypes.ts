import { NodeTypeOptions, PlanTypeMeta, PurchaseTypes, NodeOption } from "@Palavyr-Types";

export const filterNodeTypeOptionsOnSubscription = (nodeTypeOptions: NodeTypeOptions, planTypeMeta: PlanTypeMeta) => {
    const excludeFromFree: string[] = ["ShowImage", "ShowFileAsset"];
    const excludeFromLyte: string[] = ["ShowImage", "ShowFileAsset"];
    const excludeFromPremium: string[] = [];

    let filteredNodes = [...nodeTypeOptions];
    if (planTypeMeta.planType === PurchaseTypes.Premium) {
        filteredNodes = filteredNodes.filter((x: NodeOption) => !excludeFromPremium.includes(x.value));
    }
    if (planTypeMeta.planType === PurchaseTypes.Lyte) {
        filteredNodes = filteredNodes.filter((x: NodeOption) => !excludeFromLyte.includes(x.value));
    }

    if (planTypeMeta.planType === PurchaseTypes.Free) {
        filteredNodes = nodeTypeOptions.filter((x: NodeOption) => !excludeFromFree.includes(x.value));
    }
    return filteredNodes;
};
