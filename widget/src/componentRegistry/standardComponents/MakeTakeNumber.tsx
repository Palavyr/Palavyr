import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { Divider, TextField, Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction, ConvoContextProperties } from "..";
import { getChildNodes } from "../utils";
import { ResponseButton } from "../../common/ResponseButton";
import { MessageWrapper } from "../common";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeTakeNumber = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text); // Use if you want to have the message display as a separate message block
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const useStyles = makeStyles(theme => ({
        root: {
            borderBottom: "0px solid white",
        },
    }));

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);

        const classes = useStyles();

        return (
            <MessageWrapper>
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={classes.root}>
                            <TextField
                                disabled={disabled}
                                label=""
                                type="number"
                                onChange={event => {
                                    setResponse(event.target.value);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell className={classes.root} align="right">
                            <ResponseButton
                                disabled={disabled}
                                onClick={() => {
                                    if (node.isCritical) {
                                        convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response });
                                    }
                                    responseAction(node, child, nodeList, client, convoId, response, convoContext);
                                    toggleInputDisabled();
                                    setDisabled(true);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            </MessageWrapper>
        );
    };
    return Component;
};
