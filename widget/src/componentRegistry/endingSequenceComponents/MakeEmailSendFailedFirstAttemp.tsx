import * as React from "react";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell, Button } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { openUserDetails } from "src/widgetCore/store/dispatcher";

export const makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
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
                                responseAction(node, child, nodeList, client, convoId, null);
                            }}
                        />
                        <Button onClick={() => openUserDetails()}>Check Details</Button>
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return SuccessComponent;
};
