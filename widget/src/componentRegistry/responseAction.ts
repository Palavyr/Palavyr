import { random } from "lodash";
import { IClient } from "src/client/Client";
import { ConvoTableRow, ConversationUpdate } from "src/types";
import { addUserMessage, toggleMsgLoader } from "src/widget";
import { addKeyValue } from "src/widgetCore/store/dispatcher";
import { renderNextComponent } from "./renderNextComponent";
import { setDynamicResponse } from "./setDynamicResponse";


export const responseAction = (
    node: ConvoTableRow,
    child: ConvoTableRow,
    nodeList: Array<ConvoTableRow>,
    client: IClient,
    convoId: string,
    response: string,
    callback: () => void = null
) => {

    if (response) {
        if (node.isCritical) {
            addKeyValue({ [node.text]: response.toString() })
        }

        if (node.isDynamicTableNode){
            if (node.isDynamicTableNode) {
                setDynamicResponse(node.nodeType, node.nodeId, response.toString());
            }
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

    client.Widget.Access.postUpdateAsync(updatePayload); // no need to await for this
    setTimeout(() => {
        if (callback) callback();
        renderNextComponent(child, nodeList, client, convoId); // convoId should come from redux store in the future
        toggleMsgLoader();
    }, random(1000, 3000, undefined));
};