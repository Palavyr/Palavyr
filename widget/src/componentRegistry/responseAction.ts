import { WidgetNodeResource, ConversationUpdate, WidgetNodes } from "@Palavyr-Types";
import { addKeyValue, addUserMessage, toggleMsgLoader } from "@store-dispatcher";
import { WidgetClient } from "client/Client";
import { random } from "lodash";

import { renderNextComponent } from "./renderNextComponent";
import { setDynamicResponse } from "./setDynamicResponse";

export const responseAction = (node: WidgetNodeResource, child: WidgetNodeResource, nodeList: WidgetNodes, client: WidgetClient, convoId: string, response: string | null, callback: (() => void) | null = null) => {
    if (response) {
        if (node.isCritical) {
            addKeyValue({ [node.text]: response.toString() }); // TODO: make unique
        }

        if (node.isDynamicTableNode && node.dynamicType) {
            setDynamicResponse(node.dynamicType, node.nodeId, response.toString());
        }

        if (child.optionPath !== null && child.optionPath !== "" && child.optionPath !== "Continue") {
            addUserMessage(child.optionPath);
        } else {
            addUserMessage(response);
        }
    }
    toggleMsgLoader();

    var updatePayload: ConversationUpdate = {
        ConversationId: convoId,
        Prompt: node.text,
        UserResponse: response,
        NodeId: node.nodeId,
        NodeCritical: node.isCritical,
        NodeType: node.nodeType,
    };

    client.Widget.Post.ReplyUpdate(updatePayload); // no need to await for this
    setTimeout(() => {
        if (callback) callback();
        renderNextComponent(child, nodeList, client, convoId); // convoId should come from redux store in the future
        toggleMsgLoader();
    }, random(1000, 3000, undefined));
};
