import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { Divider, Table, TableRow, TableCell } from '@material-ui/core';
import { useState } from 'react';
import { IProgressTheChat, responseAction, ConvoContextProperties } from '..';
import { getChildNodes } from '../utils';
import { ResponseButton } from '../../common/ResponseButton';
import CurrencyTextField from '@unicef/material-ui-currency-textfield';
import { MessageWrapper } from '../common';


export const makeTakeCurrency = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<number>(0);

        return (
            <MessageWrapper>
                {node.text}
                <Divider />
                <Table>
                    <TableRow>
                        <TableCell>
                            <CurrencyTextField
                                label="Amount"
                                variant="standard"
                                value={0.00}
                                currencySymbol="$"
                                minimumValue="0"
                                outputFormat="string"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onChange={(value: { floatValue: number | undefined; }) => {
                                    if (value.floatValue !== undefined) { setResponse(value.floatValue) }
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell align="right">
                            <ResponseButton onClick={
                                () => {
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response })
                                    }

                                    responseAction(node, child, nodeList, client, convoId, response.toString(), convoContext)
                                    toggleInputDisabled()
                                }
                            } />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        )
    }
    return Component;
}