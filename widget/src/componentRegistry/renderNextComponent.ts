
import { WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";
import { renderCustomComponent } from "@store-dispatcher";
import { WidgetClient } from "client/Client";
import { dummyFailComponent } from "./DummyComponentDev";
import { ComponentRegistry } from "./registry";

export const renderNextComponent = (
    node: WidgetNodeResource,
    nodeList: WidgetNodes,
    client: WidgetClient,
    convoId: string,
) => {
    //TODO: make this impossible by geting the configuration right
    if (node.nodeType === "" || node.nodeType === null || node.nodeChildrenString === "" || node.nodeChildrenString === null) {
        return renderCustomComponent(dummyFailComponent, {}, false);
    }
    var makeNextComponent = ComponentRegistry[node.nodeComponentType];

    var nextComponent = makeNextComponent({ node, nodeList, client, convoId });
    return renderCustomComponent(nextComponent, {}, false);
};