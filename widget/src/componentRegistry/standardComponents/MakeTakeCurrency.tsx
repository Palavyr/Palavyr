import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { Table } from "@material-ui/core";
import { useState } from "react";
import { IProgressTheChat, responseAction, ConvoContextProperties } from "..";
import { getChildNodes } from "../utils";
import { ResponseButton } from "../../common/ResponseButton";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import { MessageWrapper } from "../common";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeTakeCurrency = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {

    toggleInputDisabled();

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {

        const [response, setResponse] = useState<number>(0);
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <MessageWrapper>
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <SingleRowSingleCell>
                        <CurrencyTextField
                            label="Amount"
                            disabled={disabled}
                            variant="standard"
                            value={response}
                            currencySymbol="$"
                            minimumValue="0"
                            outputFormat="number"
                            decimalCharacter="."
                            digitGroupSeparator=","
                            onChange={(event: any, value: number) => {
                                if (value !== undefined) {
                                    setResponse(value);
                                }
                            }}
                        />
                    </SingleRowSingleCell>
                    <SingleRowSingleCell align="right">
                        <ResponseButton
                            disabled={disabled}
                            onClick={() => {
                                if (node.isCritical) {
                                    convoContext[ConvoContextProperties.KeyValues].push({
                                        [node.text]: response,
                                    });
                                }
                                responseAction(node, child, nodeList, client, convoId, response.toString(), convoContext);
                                toggleInputDisabled();
                                setDisabled(true);
                            }}
                        />
                    </SingleRowSingleCell>
                </Table>
            </MessageWrapper>
        );
    };
    return Component;
};
