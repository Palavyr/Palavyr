import { cloneDeep, findIndex } from "lodash";
import { IAppContext } from "widget/hook";

export const setDynamicResponse = (context: IAppContext, dynamicType: string, nodeId: string, response: string) => {
    let dynamicResponseContext = cloneDeep(context.dynamicResponses);

    const currentResponseTypeIndex = findIndex(dynamicResponseContext, (resp: Object) => {
        return Object.keys(resp).includes(dynamicType);
    });

    if (currentResponseTypeIndex === -1) {
        dynamicResponseContext.push({ [dynamicType]: [{ [nodeId]: response }] });
    } else {
        dynamicResponseContext[currentResponseTypeIndex][dynamicType].push({ [nodeId]: response });
    }
    context.setDynamicResponses(dynamicResponseContext);
};
