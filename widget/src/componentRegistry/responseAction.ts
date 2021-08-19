import { WidgetNodeResource, ConversationUpdate, WidgetNodes, ContextProperties, DynamicResponses } from "@Palavyr-Types";
import { addKeyValue, addUserMessage, getContextProperties, toggleMsgLoader } from "@store-dispatcher";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { ConvoContextProperties } from "./registry";

import { renderNextComponent } from "./renderNextComponent";
import { setDynamicResponse } from "./setDynamicResponse";
import { floor, max, min } from "lodash";

export const computeReadingTime = (node: WidgetNodeResource) => {
    const typicalReadingSpeed = (node: WidgetNodeResource) => floor((node.text.length / 19) * 1000, 0);
    const timeout = min([18000, max([2000, typicalReadingSpeed(node)])]);

    return timeout;
};

export const responseAction = async (
    node: WidgetNodeResource,
    child: WidgetNodeResource,
    nodeList: WidgetNodes,
    client: PalavyrWidgetRepository,
    convoId: string,
    response: string | null,
    callback: (() => void) | null = null
) => {
    if (response) {
        if (node.isCritical) {
            addKeyValue({ [node.text]: response.toString() }); // TODO: make unique
        }

        if (node.isDynamicTableNode && node.dynamicType) {
            setDynamicResponse(node.dynamicType, node.nodeId, response.toString());

            // CONFIRM THIS IS THE RIGHT PLACE
            // we have some kind of dynamic table node that may or may not
            const contextProperties: ContextProperties = getContextProperties();
            const dynamicResponses = contextProperties[ConvoContextProperties.dynamicResponses] as DynamicResponses;

            // const tableId = extractDynamicTypeGuid(node.dynamicType);
            const currentDynamicResponseState = dynamicResponses.filter(x => Object.keys(x)[0] === node.dynamicType)[0];

            // send the dynamic responses, the
            const tooComplicated = await client.Widget.Post.InternalCheck(node, response, currentDynamicResponseState);
            if (tooComplicated) {
                child = nodeList.filter(x => x.nodeType === "TooComplicated")[0];
            }
        }

        if (child.optionPath !== null && child.optionPath !== "" && child.optionPath !== "Continue") {
            addUserMessage(child.optionPath);
        } else {
            addUserMessage(response);
        }
    }

    const updatePayload: ConversationUpdate = {
        ConversationId: convoId,
        Prompt: node.text,
        UserResponse: response,
        NodeId: node.nodeId,
        NodeCritical: node.isCritical,
        NodeType: node.nodeType,
    };

    const timeout = computeReadingTime(child);
    if (callback) callback();
    client.Widget.Post.ReplyUpdate(updatePayload); // no need to await for this


    setTimeout(() => {
        toggleMsgLoader();
        setTimeout(() => {
            renderNextComponent(child, nodeList, client, convoId); // convoId should come from redux store in the future
            toggleMsgLoader();
        }, timeout);
    }, 2000);
};
