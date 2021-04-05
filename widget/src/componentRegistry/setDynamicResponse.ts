import { cloneDeep } from "lodash";
import { getDynamicResponsesContext, setDynamicResponses } from "src/widgetCore/store/dispatcher";

export const setDynamicResponse = (nodeType: string, nodeId: string, response: string) => {
    let dynamicResponseObject = cloneDeep(getDynamicResponsesContext());
    // search the list for keys that match the nodeType, e.g. CategoricalCount-1231
    const currentResponseType = dynamicResponseObject.filter(resp => {
        return Object.keys(resp).includes(nodeType);
    });

    // maybe we haven't add this response type to the context yet, so this list is empty
    if (currentResponseType.length == 0) {
        // we need to add this response type to the context
        dynamicResponseObject = {
            ...dynamicResponseObject,
            [nodeType]: [{ [nodeId]: response }],
        };
    } else {
        // we can push a new response to the nodeType collection
        dynamicResponseObject[nodeType].push({[nodeId]: response});
    }
    setDynamicResponses(dynamicResponseObject);
};