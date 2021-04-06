import { cloneDeep, findIndex } from "lodash";
import { getDynamicResponsesContext, setDynamicResponses } from "src/widgetCore/store/dispatcher";

export const setDynamicResponse = (dynamicType: string, nodeId: string, response: string) => {
    const context = getDynamicResponsesContext(); // an array

    let dynamicResponseContext = cloneDeep(context);
    // search the list for keys that match the dynamicType, e.g. CategoricalCount-1231

    const currentResponseTypeIndex = findIndex(dynamicResponseContext, resp => {
        return Object.keys(resp).includes(dynamicType);
    });
    // const currentResponseType = dynamicResponseContext.filter(resp => {
    //     return Object.keys(resp).includes(dynamicType);
    // });

    if (currentResponseTypeIndex == -1) {
        // maybe we haven't add this response type to the context yet, so the dynamicType key doesn't exist,
        // so we need to add this response type to the context

        dynamicResponseContext.push({[dynamicType]: [{ [nodeId]: response }]});
    } else {
        // otherwise, we can push a new response to the dynamicType's collection
        dynamicResponseContext[currentResponseTypeIndex][dynamicType].push({ [nodeId]: response });
    }
    setDynamicResponses(dynamicResponseContext);
};
