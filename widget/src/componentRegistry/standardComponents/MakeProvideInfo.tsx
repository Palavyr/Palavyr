import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table } from "@material-ui/core";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { SingleRowSingleCell } from "src/common/TableCell";
import { useState } from "react";

export const makeProvideInfo = ({ node, nodeList, client, convoId, contextProperties, setContextProperties }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary
    console.log(contextProperties.name + " From makeProviceInfo tops"   )
    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [, setDisabled] = useState<boolean>(false);

        return (
            <>
                {node.text}
                <Table>
                    <SingleRowSingleCell align="right">
                        <ResponseButton
                            text="Proceed"
                            onClick={() => {
                                console.log(contextProperties.name + " From Make Profice Info")

                                responseAction(node, child, nodeList, client, convoId, null, contextProperties, setContextProperties);
                                toggleInputDisabled();
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
