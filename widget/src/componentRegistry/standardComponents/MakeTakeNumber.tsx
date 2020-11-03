import * as React from 'react';
import { addResponseMessage, toggleInputDisabled } from 'src/widgetCore/store/dispatcher';
import { Divider, TextField, Table, TableRow, TableCell, makeStyles } from '@material-ui/core';
import { useState } from 'react';
import { IProgressTheChat, responseAction, ConvoContextProperties } from '..';
import { getChildNodes } from '../utils';
import { ResponseButton } from '../../common/ResponseButton';
import { MessageWrapper } from '../common';


export const makeTakeNumber = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const useStyles = makeStyles(theme => ({
        root: {
            borderBottom: "0px solid white"
        }
    }));

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");

        const classes = useStyles();

        return (
            <MessageWrapper>
                {node.text}
                <Divider />
                <Table>
                    <TableRow >
                        <TableCell className={classes.root}>
                            <TextField
                                label=""
                                type="number"
                                onChange={(event) => {
                                    setResponse(event.target.value)
                                }}
                            />
                        </TableCell>
                    </TableRow >
                    <TableRow >
                        <TableCell className={classes.root} align="right">
                            <ResponseButton onClick={
                                () => {
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response })
                                    }

                                    responseAction(node, child, nodeList, client, convoId, response, convoContext)
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