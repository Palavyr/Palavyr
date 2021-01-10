import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { IProgressTheChat } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { Table, TableRow, TableCell } from "@material-ui/core";
import { useState } from "react";

export const makeTooComplicated = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    addResponseMessage("End of the line! This the begining of the closing sequence.");
    toggleInputDisabled();

    const Component: React.ElementType<{}> = () => {
        const noBorder = { borderBottom: "none" };
        const [disabled, setDisabled] = useState<boolean>(false);

        return (
            <Table>
                <TableRow>
                    <TableCell style={noBorder} align="right">
                        <ResponseButton
                            disabled={disabled}
                            text="Click to End"
                            onClick={() => {
                                alert("Good Job!");
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
