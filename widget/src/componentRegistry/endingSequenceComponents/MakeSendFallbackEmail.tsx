import * as React from "react";
import { toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { Table, TableRow, TableCell } from "@material-ui/core";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { CompleteConverationDetails, ConvoTableRow } from "../../types";

const assembleCompletedConvo = (conversationId: string, areaIdentifier: string, name: string, email: string, PhoneNumber: string): CompleteConverationDetails => {
    return {
        ConversationId: conversationId,
        AreaIdentifier: areaIdentifier,
        Name: name,
        Email: email,
        PhoneNumber: PhoneNumber,
    };
};

export const makeSendFallbackEmail = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary
    const areaId = nodeList[0].areaIdentifier;

    const SuccessComponent: React.ElementType<{}> = () => {
        const sendFallbackEmail = async () => {



            const email = contextProperties[ConvoContextProperties.emailAddress];
            const name = contextProperties[ConvoContextProperties.name];
            const phone = contextProperties[ConvoContextProperties.phoneNumber];

            const { data: response } = await client.Widget.Access.sendFallbackEmail(areaId, email, convoId);
            if (response.result) {
                var completeConvo = assembleCompletedConvo(convoId, areaId, name, email, phone);
                await client.Widget.Access.postCompleteConversation(completeConvo);
            }
            return response;
        };
        return (
            <Table>
                <TableRow>
                    <TableCell>{node.text}</TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <ResponseButton
                            text="Grant permission to send email"
                            variant="contained"
                            onClick={async () => {
                                const response = await sendFallbackEmail();
                                const child = nodeList.filter((x: ConvoTableRow) => x.nodeId === response.nextNodeId)[0];
                                responseAction(node, child, nodeList, client, convoId, null);
                                toggleInputDisabled();
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };
    return SuccessComponent;
};
