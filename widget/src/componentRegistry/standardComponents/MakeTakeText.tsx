import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'src/widgetCore/store/dispatcher';
import { Divider, Button, TextField, TableRow, TableCell, Table } from '@material-ui/core';
import { useState } from 'react';
import { IProgressTheChat, responseAction, ConvoContextProperties } from '..';
import { getChildNodes } from '../utils';
import { MessageWrapper } from '../common';


export const makeTakeText = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");

        return (
            <MessageWrapper>

                {node.text}
                <Divider />
                <Table>
                    <TableRow>
                        <TableCell >
                            <TextField
                                label="Write here..."
                                type="text"
                                onChange={(event) => {
                                    setResponse(event.target.value)
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell align="right">
                            <Button
                                color="primary"
                                variant="outlined"
                                size="small"
                                onClick={() => {
                                    setResponse(response);
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response })
                                    }
                                    responseAction(node, child, nodeList, client, convoId, response, convoContext)
                                    toggleInputDisabled()
                                }}
                            >
                                Submit
                            </Button>
                        </TableCell>
                    </TableRow>
                </Table>
            </ MessageWrapper>

        )
    }
    return Component;
}