import { cloneDeep, findIndex } from "lodash";
import { getDynamicResponsesContext, setDynamicResponses } from "src/widgetCore/store/dispatcher";

export const setDynamicResponse = (dynamicType: string, nodeId: string, response: string) => {
    const context = getDynamicResponsesContext();

    let dynamicResponseContext = cloneDeep(context);

    const currentResponseTypeIndex = findIndex(dynamicResponseContext, resp => {
        return Object.keys(resp).includes(dynamicType);
    });

    if (currentResponseTypeIndex == -1) {
        dynamicResponseContext.push({ [dynamicType]: [{ [nodeId]: response }] });
    } else {
        dynamicResponseContext[currentResponseTypeIndex][dynamicType].push({ [nodeId]: response });
    }
    setDynamicResponses(dynamicResponseContext);
};
