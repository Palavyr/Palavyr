import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { Button, TextField, Table } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction, ConvoContextProperties } from "..";
import { getChildNodes } from "../utils";
import { MessageWrapper } from "../common";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeTakeText = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <MessageWrapper>
                <Table>
                    <SingleRowSingleCell>
                        <TextField
                            label="Write here..."
                            type="text"
                            onChange={event => {
                                setResponse(event.target.value);
                            }}
                        />
                    </SingleRowSingleCell>
                    <SingleRowSingleCell align="right">
                        <Button
                            disabled={disabled}
                            color="primary"
                            variant="outlined"
                            size="small"
                            onClick={() => {
                                setResponse(response);
                                if (node.isCritical) {
                                    convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response });
                                }
                                responseAction(node, child, nodeList, client, convoId, response, convoContext);
                                toggleInputDisabled();
                                setDisabled(true);
                            }}
                        >
                            Submit
                        </Button>
                    </SingleRowSingleCell>
                </Table>
            </MessageWrapper>
        );
    };
    return Component;
};
