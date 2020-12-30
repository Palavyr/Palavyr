import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { TextField, TableRow, Table, TableCell } from "@material-ui/core";
import { useState } from "react";
import { getChildNodes } from "../utils";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import { MessageWrapper } from "../common";
import { ResponseButton } from "../../common/ResponseButton";

export const makeEmail = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
    const Component: React.ElementType<{}> = () => {
        const [emailResponse, setResponse] = useState<string>("");
        const noBorder = { borderBottom: "none" };

        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell>{node.text}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell style={noBorder}>
                            <TextField
                                label="Email"
                                type="email"
                                onChange={event => {
                                    setResponse(event.target.value);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                onClick={e => {
                                    e.preventDefault();
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ "Email Address": emailResponse });
                                    }
                                    convoContext[ConvoContextProperties.EmailAddress] = emailResponse;
                                    responseAction(node, child, nodeList, client, convoId, emailResponse, convoContext);
                                    toggleInputDisabled();
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        );
    };
    return Component;
};
