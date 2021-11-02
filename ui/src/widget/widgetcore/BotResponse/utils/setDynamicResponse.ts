import { getDynamicResponsesContext, setDynamicResponses } from "@store-dispatcher";
import { cloneDeep, findIndex } from "lodash";

export const setDynamicResponse = (dynamicType: string, nodeId: string, response: string) => {
    const context = getDynamicResponsesContext();

    let dynamicResponseContext = cloneDeep(context);

    const currentResponseTypeIndex = findIndex(dynamicResponseContext, (resp: Object) => {
        return Object.keys(resp).includes(dynamicType);
    });

    if (currentResponseTypeIndex === -1) {
        dynamicResponseContext.push({ [dynamicType]: [{ [nodeId]: response }] });
    } else {
        dynamicResponseContext[currentResponseTypeIndex][dynamicType].push({ [nodeId]: response });
    }
    setDynamicResponses(dynamicResponseContext);
};
