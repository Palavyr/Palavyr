import * as React from "react";
import { getChildNodes } from "../utils";
import { responseAction, IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { useState } from "react";

export const makeProvideInfo = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

    const Component: React.ElementType<{}> = () => {
        const [, setDisabled] = useState<boolean>(false);
        return (
            <div>
                {node.text}
                <div style={{ marginTop: "1rem", width: "100%", display: "flex", flexDirection: "column", justifyContent: "right" }}>
                    <ResponseButton
                        text="Proceed"
                        onClick={() => {
                            responseAction(node, child, nodeList, client, convoId, null);
                            setDisabled(true);
                        }}
                    />
                </div>
            </div>
        );
    };
    return Component;
};
