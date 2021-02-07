import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useState } from "react";

export const makeProvideInfo = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                <SingleRowSingleCell align="right">

                        <ResponseButton
                            text="Proceed"
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, null, convoContext);
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
