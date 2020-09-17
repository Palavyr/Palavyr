import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'react-chat-widget';
import { IProgressTheChat } from '..';
import { MessageWrapper } from '../common';
import { ResponseButton } from '../../common/ResponseButton';
import { Table, TableRow, TableCell } from '@material-ui/core';

export const makeTooComplicated = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {

    addResponseMessage("End of the line! This the begining of the closing sequence.")
    toggleInputDisabled();

    const component = () => {
        const noBorder = { borderBottom: "none" };
        return (
            <MessageWrapper>
                <Table>
                    <TableRow>
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                text="Click to End"
                                onClick={
                                    () => {
                                        alert("Good Job!")
                                    }
                                }
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        )
    }
    return component
}
