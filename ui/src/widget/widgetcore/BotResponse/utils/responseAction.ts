import { WidgetNodeResource, WidgetConversationUpdate, WidgetNodes, ContextProperties, DynamicResponses, KeyValue, IMessage } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";

import { floor, max, min } from "lodash";
import { ConvoContextProperties } from "@widgetcore/componentRegistry/registry";
import { renderNextBotMessage } from "./renderNextComponent";
import { setDynamicResponse } from "./setDynamicResponse";
import { IAppContext } from "widget/hook";
import { Message } from "@widgetcore/components/Messages/components/Message/Message";

const WORDS_READ_PER_MINUTE_FOR_A_TYPICAL_HUMAN = 22;

export const extractContent = (inputTextWithHtml: string, space: boolean = true) => {
    var span = document.createElement("span");
    span.innerHTML = inputTextWithHtml;
    if (space) {
        var children = span.querySelectorAll("*");
        for (var i = 0; i < children.length; i++) {
            if (children[i].textContent) children[i].textContent += " ";
        }
    }
    return [span.textContent || span.innerText].toString().replace(/ +/g, " ");
};

export const computeReadingTime = (node: WidgetNodeResource): number => {
    const typicalReadingSpeed = (node: WidgetNodeResource) => floor((extractContent(node.text).length / WORDS_READ_PER_MINUTE_FOR_A_TYPICAL_HUMAN) * 1000, 0);
    const timeout = min([18000, max([2000, typicalReadingSpeed(node)])]);
    return timeout as number;
};

const stripHtml = html => {
    // Create a new div element
    let temp = document.createElement("div");
    // Set HTML content using provider
    temp.innerHTML = html;
    // Get the text property of the element (browser support)
    return temp.textContent || temp.innerText || "";
};

export const responseAction = async (
    context: IAppContext,
    node: WidgetNodeResource,
    child: WidgetNodeResource,
    nodeList: WidgetNodes,
    client: PalavyrWidgetRepository,
    convoId: string,
    response: string | null = null,
    callback: (() => void) | null = null
) => {
    if (response) {
        if (node.isCritical) {
            const keyValue = { [node.text]: response.toString() } as KeyValue;
            context.setKeyValues([...context.AppContext.keyValues, keyValue]);
        }

        if (node.isDynamicTableNode && node.dynamicType) {
            setDynamicResponse(context, node.dynamicType, node.nodeId, response.toString());

            const dynamicResponses = context.AppContext[ConvoContextProperties.dynamicResponses] as DynamicResponses;

            const currentDynamicResponseState = dynamicResponses.filter(x => Object.keys(x)[0] === node.dynamicType)[0];

            const tooComplicated = await client.Widget.Post.InternalCheck(node, response, currentDynamicResponseState);
            if (tooComplicated) {
                child = nodeList.filter(x => x.nodeType === "TooComplicated")[0];
            }
        }

        let userText: string;
        if (child.optionPath !== null && child.optionPath !== "" && child.optionPath !== "Continue") {
            userText = child.optionPath;
        } else {
            userText = response;
        }
        const userResponse = createUserResponseComponent(userText, convoId);
        context.addNewUserMessage(userResponse);
    }

    const timeout = computeReadingTime(child);

    if (callback) callback();

    if (convoId !== null) {
        const updatePayload: WidgetConversationUpdate = {
            ConversationId: convoId,
            Prompt: stripHtml(node.text),
            UserResponse: response,
            NodeId: node.nodeId,
            NodeCritical: node.isCritical,
            NodeType: node.nodeType,
        };
        await client.Widget.Post.UpdateConvoHistory(updatePayload); // no need to await for this
    }

    setTimeout(() => {
        context.toggleMessageLoader();
        setTimeout(() => {
            renderNextBotMessage(context, child, nodeList, client, convoId);
            context.toggleMessageLoader();
        }, timeout);
    }, 2000);
};

export const createUserResponseComponent = (text: string, id: string | null): IMessage => {
    return {
        type: "user",
        component: Message,
        text,
        sender: "user-response",
        timestamp: new Date(),
        showAvatar: true,
        customId: id ?? "",
        unread: false,
        nodeType: "user-response"
    };
};
