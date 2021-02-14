import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";

export const makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const SuccessComponent: React.ElementType<{}> = () => {
        return (
            <Table>
                <TableRow>
                    <TableCell>{node.text}</TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <ResponseButton
                            text="Check your email details."
                            variant="contained"
                            onClick={async () => {
                                // TODO: Open the customer Details Panel and allow them to update the email address.
                                responseAction(node, child, nodeList, client, convoId, null);
                                toggleInputDisabled();
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return SuccessComponent;
};
