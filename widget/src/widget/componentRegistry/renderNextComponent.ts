
import { WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";
import { renderCustomComponent } from "@store-dispatcher";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { dummyFailComponent } from "./DummyComponentDev";
import { ComponentRegistry } from "./registry";

export const renderNextComponent = (
    node: WidgetNodeResource,
    nodeList: WidgetNodes,
    client: PalavyrWidgetRepository,
    convoId: string | null,
) => {
    //TODO: make this impossible by geting the configuration right

    if (node.nodeType === "" || node.nodeType === null || node.nodeChildrenString === "" || node.nodeChildrenString === null || node === undefined) {
        return renderCustomComponent(dummyFailComponent, {}, false);
    }
    const makeNextComponent = ComponentRegistry[node.nodeComponentType];

    const nextComponent = makeNextComponent({ node, nodeList, client, convoId });
    return renderCustomComponent(nextComponent, {}, false);
};