import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { TextField, Table } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction, ConvoContextProperties } from "..";
import { getChildNodes } from "../utils";
import { SingleRowSingleCell } from "src/common/TableCell";
import { ResponseButton } from "src/common/ResponseButton";

export const makeTakeText = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>

                <SingleRowSingleCell>
                    <TextField
                        disabled={disabled}
                        label="Write here..."
                        type="text"
                        onChange={event => {
                            setResponse(event.target.value);
                        }}
                    />
                </SingleRowSingleCell>
                <SingleRowSingleCell align="right">
                    <ResponseButton
                        disabled={disabled || response === ""}
                        text="Submit"
                        onClick={() => {
                            setResponse(response);
                            if (node.isCritical) {
                                convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response });
                            }
                            responseAction(node, child, nodeList, client, convoId, response, convoContext);
                            toggleInputDisabled();
                            setDisabled(true);
                        }}
                    />
                </SingleRowSingleCell>
            </Table>
        );
    };
    return Component;
};
