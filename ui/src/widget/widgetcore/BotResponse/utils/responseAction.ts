import { ConversationHistoryRowResource, KeyValue, UserMessageData } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";

import { floor, max, min } from "lodash";
import { renderNextBotMessage } from "./renderBotMessage";
import { setDynamicResponse } from "./setDynamicResponse";
import { IAppContext } from "widget/hook";
import { UserMessage } from "@widgetcore/components/Messages/components/Message/Message";
import { WidgetNodeResource, WidgetNodeResources } from "@common/types/api/EntityResources";

const WORDS_READ_PER_MINUTE_FOR_A_TYPICAL_HUMAN = 11;
const MIN_SPEED_MILLISECONDS = 18000;
const MAX_SPEED_MILLISECONDS = 2000;
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

export const computeReadingTime = (node: WidgetNodeResource, readingTime: number): number => {
    const ajustedReadTime = WORDS_READ_PER_MINUTE_FOR_A_TYPICAL_HUMAN * readingTime;

    const typicalReadingSpeed = (node: WidgetNodeResource) => floor((extractContent(node.text).length / ajustedReadTime) * 1000, 0);
    const timeout = min([MIN_SPEED_MILLISECONDS, max([MAX_SPEED_MILLISECONDS, typicalReadingSpeed(node)])]);
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
    nodeList: WidgetNodeResources,
    client: PalavyrWidgetRepository,
    convoId: string,
    isDemo: boolean,
    response: string | null = null,
    callback: (() => void) | null = null
) => {
    // LOCK THE RESET BUTTON on activation of this block

    context.disableReset();

    if (response) {
        if (node.isCritical) {
            const keyValue = { [node.text]: response.toString() } as KeyValue;
            context.addKeyValue(keyValue);
        }

        if (node.isPricingStrategyTableNode && node.pricingStrategyType) {
            const updatedDynamicResponses = setDynamicResponse(context.dynamicResponses, node.pricingStrategyType, node.nodeId, response.toString());

            context.setDynamicResponses(updatedDynamicResponses);

            const currentDynamicResponseState = updatedDynamicResponses.filter(x => Object.keys(x)[0] === node.pricingStrategyType)[0];

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

    if (callback) callback();

    if (!isDemo) {
        if (convoId !== null) {
            const updatePayload: ConversationHistoryRowResource = {
                conversationId: convoId,
                prompt: stripHtml(node.text),
                userResponse: response ?? "",
                nodeId: node.nodeId,
                nodeCritical: node.isCritical,
                nodeType: node.nodeType,
                timeStamp: new Date().toDateString(),
                id: null,
            };

            await client.Widget.Post.UpdateConvoHistory(updatePayload); // no need to await for this
        }
    }

    const timeout = computeReadingTime(child, context.readingSpeed);
    setTimeout(() => {
        context.enableMessageLoader();
        setTimeout(() => {
            if (context.chatStarted) {
                context.enableReset();
            }
            setTimeout(() => {
                context.disableMessageLoader();
                renderNextBotMessage(context, child, nodeList, client, convoId);
            }, 200);
        }, timeout);
    }, 1400);
};

export const CSS_LINKER_and_NODE_TYPE = {
    BOT: "bot-response",
    USER: "user-response",
};

export const createUserResponseComponent = (text: string, id: string | null): UserMessageData => {
    return {
        type: "user",
        component: UserMessage,
        text,
        sender: CSS_LINKER_and_NODE_TYPE.USER,
        timestamp: new Date(),
        showAvatar: true,
        customId: id ?? "",
        unread: false,
        nodeType: CSS_LINKER_and_NODE_TYPE.USER,
    };
};
