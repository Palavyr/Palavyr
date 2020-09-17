import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { getChildNodes } from '../utils';
import { Table, TableRow, TableCell } from '@material-ui/core';
import { responseAction, IProgressTheChat } from '..';
import { ResponseButton } from '../../common/ResponseButton';
import { MessageWrapper } from '../common';


export const makeProvideInfo = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

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
                        <TableCell align="right">
                            <ResponseButton
                                text="Proceed"
                                onClick={
                                    () => {
                                        responseAction(node, child, nodeList, client, convoId, null, convoContext)
                                        toggleInputDisabled()
                                    }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        )
    }
    return Component;
}