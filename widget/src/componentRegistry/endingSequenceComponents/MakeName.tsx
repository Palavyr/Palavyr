import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { TextField, Table, TableRow, TableCell } from '@material-ui/core';
import { useState } from 'react';
import { getChildNodes } from '../utils';
import { responseAction, IProgressTheChat, ConvoContextProperties } from '..';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';


export const makeName = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [nameResponse, setResponse] = useState<string>("");
        const noBorder = { borderBottom: "none" };

        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell>
                            {node.text}
                        </TableCell>
                    </TableRow>
                    <TableRow >
                        <TableCell style={noBorder}>
                            <TextField
                                label="Name"
                                type="text"
                                onChange={
                                    (event) => {
                                        setResponse(event.target.value)
                                    }
                                }
                            />

                        </TableCell>
                    </TableRow>
                    <TableRow >
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                onClick={
                                    (e) => {
                                        e.preventDefault();
                                        if (node.isCritical) {
                                            convoContext[ConvoContextProperties.KeyValues].push({ "Name": nameResponse })
                                        }
                                        convoContext["Name"] = nameResponse;

                                        responseAction(node, child, nodeList, client, convoId, nameResponse, convoContext)
                                        toggleInputDisabled()
                                    }
                                }
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper >
        )
    }
    return Component;
}