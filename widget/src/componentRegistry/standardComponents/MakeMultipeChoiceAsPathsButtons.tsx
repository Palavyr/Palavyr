import * as React from 'react';
import { ConvoTableRow } from '../../types';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { getChildNodes } from '../utils';
import { TableRow, TableCell, Table } from '@material-ui/core';
import { responseAction, IProgressTheChat } from '..';
import { ResponseButton } from '../../common/ResponseButton';
import { MessageWrapper } from '../common';


export const makeMultipleChoiceAsPathButtons = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const children = getChildNodes(node.nodeChildrenString, nodeList);

    const Component: React.ElementType<{}> = () => {
        const noBorder = {borderBottom: "none"}
        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        {
                            children.map((child: ConvoTableRow) => {
                                return (
                                    <TableCell style={noBorder}>
                                        <ResponseButton
                                            key={child.id}
                                            text={child.optionPath}
                                            onClick={() => {
                                                var response = child.optionPath;
                                                responseAction(node, child, nodeList, client, convoId, response, convoContext)
                                                toggleInputDisabled();
                                            }}
                                        />
                                    </TableCell>
                                )
                            })
                        }
                    </TableRow>
                </Table>
            </MessageWrapper>
        )
    }
    return Component;
}
