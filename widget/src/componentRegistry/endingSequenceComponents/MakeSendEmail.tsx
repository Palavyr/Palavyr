import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { getChildNodes } from '../utils';
import { Table, TableRow, TableCell } from '@material-ui/core';
import { responseAction, IProgressTheChat, ConvoContextProperties } from '..';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';



export const makeSendEmail = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
    const areaId = nodeList[0].areaIdentifier;

    const sendEmail = async () => {
        const email = convoContext[ConvoContextProperties.EmailAddress];
        const dynamicResponse = convoContext[ConvoContextProperties.DynamicResponse];
        const keyvalues = convoContext[ConvoContextProperties.KeyValues];

        await client.Widget.Access.sendConfirmationEmail(areaId, email, dynamicResponse, keyvalues);
        // TODO: handle emailing erros and whatnot in makeSendEmail
    }

    const Component: React.ElementType<{}> = () => {
        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell>
                            {node.text}
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>
                            <ResponseButton
                                text="Grant permission to send email"
                                variant="contained"
                                onClick={
                                    () => {
                                        sendEmail()
                                        responseAction(node, child, nodeList, client, convoId, null, convoContext)
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