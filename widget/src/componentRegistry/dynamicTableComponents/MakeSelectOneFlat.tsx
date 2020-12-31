import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { responseAction, IProgressTheChat, ConvoContextProperties, NodeTypes } from "..";
import { uuid } from "uuidv4";
import { MessageWrapper } from "../common";
import { ResponseButton } from "../../common/ResponseButton";
import { useState } from "react";
import { SingleRowSingleCell } from "src/common/TableCell";

const useStyles = makeStyles(theme => ({
    wrapper: {
        overflowX: "auto",
    },
    table: {
        borderBottom: "none",
    },
}));

// All Dynamic results should add response formatted to the dynamic response AND the critical value lst
export const makeSelectOneFlat = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
    const options = node.valueOptions.split("|peg|");

    const Component: React.ElementType<{}> = () => {
        const cls = useStyles();
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <MessageWrapper>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <Table>
                    {options.map((option: string, index: number) => {
                        return (
                            <TableRow key={index}>
                                <TableCell className={cls.table}>
                                    <ResponseButton
                                        disabled={disabled}
                                        key={option + uuid()}
                                        text={option}
                                        onClick={() => {
                                            const dynamicResponse = {
                                                [node.nodeType]: option
                                            }

                                            convoContext[ConvoContextProperties.DynamicResponses].push(dynamicResponse);

                                            if (node.isCritical) {
                                                convoContext[ConvoContextProperties.KeyValues].push(dynamicResponse);
                                            }
                                            responseAction(node, child, nodeList, client, convoId, option, convoContext);
                                            toggleInputDisabled();
                                            setDisabled(true);
                                        }}
                                    />
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </Table>
            </MessageWrapper>
        );
    };
    return Component;
};
