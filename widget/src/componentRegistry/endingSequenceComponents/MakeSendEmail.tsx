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

export const makeSendEmail = ({ node, nodeList, client, convoId, convoContext }: IProgressTheChat) => {
    toggleInputDisabled(); // can manually toggle in each component when necessary
    const areaId = nodeList[0].areaIdentifier;

    const sendEmail = async () => {
        const email = convoContext[ConvoContextProperties.EmailAddress];
        const name = convoContext[ConvoContextProperties.Name];
        const phone = convoContext[ConvoContextProperties.PhoneNumber];
        const dynamicResponses = convoContext[ConvoContextProperties.DynamicResponses];
        const keyvalues = convoContext[ConvoContextProperties.KeyValues];

        const { data: response } = await client.Widget.Access.sendConfirmationEmail(areaId, email, dynamicResponses, keyvalues, convoId);
        if (response.result) {
            var completeConvo = assembleCompletedConvo(convoId, areaId, name, email, phone);
            await client.Widget.Access.postCompleteConversation(completeConvo);
        }
        return response;
    };

    const SuccessComponent: React.ElementType<{}> = () => {
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
                                const response = await sendEmail();
                                const child = nodeList.filter((x: ConvoTableRow) => x.nodeId === response.nextNodeId)[0];
                                responseAction(node, child, nodeList, client, convoId, null, convoContext);
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
