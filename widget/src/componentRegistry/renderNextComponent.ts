import { IClient } from "src/client/Client";
import { ConvoTableRow } from "src/types";
import { renderCustomComponent } from "src/widget";
import { dummyFailComponent } from "./DummyComponentDev";
import { ComponentRegistry } from "./registry";

export const renderNextComponent = (
    node: ConvoTableRow,
    nodeList: Array<ConvoTableRow>,
    client: IClient,
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