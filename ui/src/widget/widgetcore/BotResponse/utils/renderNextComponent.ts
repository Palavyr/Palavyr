import { WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { dummyFailComponent } from "@widgetcore/componentRegistry/DummyComponentDev";
import { ComponentRegistry } from "@widgetcore/componentRegistry/registry";
import { IAppContext } from "widget/hook";

export const renderNextBotMessage = (context: IAppContext, node: WidgetNodeResource, nodeList: WidgetNodes, client: PalavyrWidgetRepository, convoId: string | null) => {
    if (node.nodeType === "" || node.nodeType === null || node.nodeChildrenString === "" || node.nodeChildrenString === null || node === undefined) {
        const botMessage = {
            type: "bot",
            component: dummyFailComponent,
            props: {},
            sender: "bot-response",
            timestamp: new Date(),
            showAvatar: true,
            customId: convoId ?? "",
            unread: true,
            specialId: "",
            nodeType: "",
        };
        return context.addNewBotMessage(botMessage);
    }

    const makeNextComponent = ComponentRegistry[node.nodeComponentType];
    const component = makeNextComponent({ node, nodeList, client, convoId });

    const botMessage = {
        type: "bot",
        component,
        props: {},
        sender: "bot-response",
        timestamp: new Date(),
        showAvatar: true,
        customId: convoId ?? "",
        unread: true,
        nodeType: node.nodeComponentType,
    };
    context.addNewBotMessage(botMessage);
};
