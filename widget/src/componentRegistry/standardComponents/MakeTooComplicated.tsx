import * as React from "react";
import { toggleInputDisabled, toggleMsgLoader } from "src/widgetCore/store/dispatcher";
import { Table } from "@material-ui/core";
import { getChildNodes } from "../utils";
import { IProgressTheChat, responseAction } from "..";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useEffect } from "react";

export const makeTooComplicated = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    toggleInputDisabled();

    const Component: React.ElementType<{}> = () => {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
        useEffect(() => {
            setTimeout(() => {
                responseAction(node, child, nodeList, client, convoId, null, convoContext);
            }, 1500);
        }, [])

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
            </Table>
        );
    };
    return Component;
};
