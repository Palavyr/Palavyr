import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'src/widgetCore/store/dispatcher';
import { getChildNodes } from '../utils';
import { TableRow, TableCell, Table } from '@material-ui/core';
import { responseAction, IProgressTheChat } from '..';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';


export const makeStartEndingSequence = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const noBorder = { borderBottom: "none" };

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
                                text="Proceed"
                                onClick={() => {
                                    responseAction(node, child, nodeList, client, convoId, null, convoContext)
                                    toggleInputDisabled()
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table >
            </MessageWrapper >
        )
    }
    return Component;
}