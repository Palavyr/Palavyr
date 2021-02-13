import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { TableRow, TableCell, Table } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { useState } from "react";
import { SingleRowSingleCell } from "src/common/TableCell";

export const makeStartEndingSequence = ({ node, nodeList, client, convoId, contextProperties, setContextProperties }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const noBorder = { borderBottom: "none" };
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <TableRow>
                    <TableCell style={noBorder} align="right">
                        <ResponseButton
                            disabled={disabled}
                            text="Proceed"
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, null, contextProperties, setContextProperties);
                                toggleInputDisabled();
                                setDisabled(true);
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return Component;
};
