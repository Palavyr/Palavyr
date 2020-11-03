import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'src/widgetCore/store/dispatcher';
import { getChildNodes } from '../utils';
import { Table, TableRow, TableCell } from '@material-ui/core';
import { responseAction, IProgressTheChat, ConvoContextProperties } from '..';
import { uuid } from 'uuidv4';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';


export const makeMultipleChoiceContinueButtons = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {

    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
    const valueOptions = node.valueOptions.split(",");
    const Component: React.ElementType<{}> = () => {
        const noBorder = {borderBottom: "none"}
        return (

            <MessageWrapper>
                <Table>
                    <TableRow>
                        {
                            valueOptions.map((valueOption: string) => {
                                return (
                                    <TableCell style={noBorder}>
                                        <ResponseButton
                                            key={valueOption + "-" + uuid()}
                                            text={valueOption}
                                            onClick={() => {
                                                const response = valueOption;
                                                if (node.isCritical) {
                                                    convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response })
                                                }
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
            </MessageWrapper >
        )
    }
    return Component;
}

