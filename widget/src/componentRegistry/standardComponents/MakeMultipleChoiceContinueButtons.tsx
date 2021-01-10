import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell, makeStyles } from "@material-ui/core";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import { uuid } from "uuidv4";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useState } from "react";

const useStyles = makeStyles(theme => ({
    table: {
        borderBottom: "none",
    },
}));

export const makeMultipleChoiceContinueButtons = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    // TODO: lift this widget and add  'isInputDisabled()'
    // addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
    const valueOptions = node.valueOptions.split(",");
    const Component: React.ElementType<{}> = () => {
        const cls = useStyles();
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <TableRow>
                    {valueOptions.map((valueOption: string) => {
                        return (
                            <TableCell className={cls.table}>
                                <ResponseButton
                                    disabled={disabled}
                                    key={valueOption + "-" + uuid()}
                                    text={valueOption}
                                    onClick={() => {
                                        const response = valueOption;
                                        if (node.isCritical) {
                                            convoContext[ConvoContextProperties.KeyValues].push({ [node.text]: response });
                                        }
                                        responseAction(node, child, nodeList, client, convoId, response, convoContext);
                                        toggleInputDisabled();
                                        setDisabled(true);
                                    }}
                                />
                            </TableCell>
                        );
                    })}
                </TableRow>
            </Table>
        );
    };
    return Component;
};
