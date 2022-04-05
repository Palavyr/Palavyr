import { DynamicResponses } from "@Palavyr-Types";
import { cloneDeep, findIndex } from "lodash";


export const setDynamicResponse = (dynamicResponseContext: DynamicResponses, dynamicType: string, nodeId: string, response: string) => {

    const currentResponseTypeIndex = findIndex(dynamicResponseContext, (resp: Object) => {
        return Object.keys(resp).includes(dynamicType);
    });

    if (currentResponseTypeIndex === -1) { // doesn't yet exist in the array
        dynamicResponseContext.push({ [dynamicType]: [{ [nodeId]: response }] });
    } else {
        dynamicResponseContext[currentResponseTypeIndex][dynamicType].push({ [nodeId]: response });
    }
    return dynamicResponseContext;
};
