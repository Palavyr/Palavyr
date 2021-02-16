import * as React from "react";
import { TextField, Table } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction } from "..";
import { getChildNodes } from "../utils";
import { SingleRowSingleCell } from "src/common/TableCell";
import { ResponseButton } from "src/common/ResponseButton";

export const makeTakeText = ({ node, nodeList, client, convoId }: IProgressTheChat) => {

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [response, setResponse] = useState<string>("");
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <>
                {node.text}
                <Table>
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
                                responseAction(node, child, nodeList, client, convoId, response);
                                setDisabled(true);

                            }}
                        />
                    </SingleRowSingleCell>
                </Table>
            </>
        );
    };
    return Component;
};
