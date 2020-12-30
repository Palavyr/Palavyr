import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { TextField, Table, TableRow, TableCell } from "@material-ui/core";
import { useState } from "react";
import { getChildNodes } from "../utils";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import { MessageWrapper } from "../common";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeName = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [nameResponse, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);
        const noBorder = { borderBottom: "none" };

        return (
            <MessageWrapper>
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell style={noBorder}>
                            <TextField
                                disabled={disabled}
                                label={ConvoContextProperties.Name}
                                type="text"
                                onChange={event => {
                                    setResponse(event.target.value);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                disabled={disabled}
                                onClick={e => {
                                    e.preventDefault();
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ [ConvoContextProperties.Name]: nameResponse });
                                    }
                                    convoContext[ConvoContextProperties.Name] = nameResponse;

                                    responseAction(node, child, nodeList, client, convoId, nameResponse, convoContext);
                                    toggleInputDisabled();
                                    setDisabled(true);
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
