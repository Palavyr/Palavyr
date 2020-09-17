import * as React from 'react';
import { IProgressTheChat } from '..';
import {  TableCell, Table, TableRow } from '@material-ui/core';
import { ResponseButton } from '../../common/ResponseButton';
import { MessageWrapper } from '../common';


export const makeRestart = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text);

    // const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const noBorder = {borderBottom: "none"}
        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell>
                            {node.text}
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                text="restart"
                                onClick={
                                    () => {
                                        window.location.reload();
                                    }
                                }
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        )
    }
    return Component;
}