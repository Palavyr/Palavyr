import * as React from "react";
import { addResponseMessage, toggleInputDisabled } from "src/widgetCore/store/dispatcher";
import { getChildNodes } from "../utils";
import { Table, TableRow, TableCell } from "@material-ui/core";
import { responseAction, IProgressTheChat, ConvoContextProperties } from "..";
import { ResponseButton } from "../../common/ResponseButton";
import { CompleteConverationDetails } from "../../types";

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
    // TODO: lift this widget and add  'isInputDisabled()'
    addResponseMessage(node.text);
    toggleInputDisabled(); // can manually toggle in each component when necessary

    const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
    const areaId = nodeList[0].areaIdentifier;

    const sendEmail = async () => {
        const email = convoContext[ConvoContextProperties.EmailAddress];
        const name = convoContext[ConvoContextProperties.Name];
        const phone = convoContext[ConvoContextProperties.PhoneNumber];
        const dynamicResponses = convoContext[ConvoContextProperties.DynamicResponses];
        const keyvalues = convoContext[ConvoContextProperties.KeyValues];

        var { data } = await client.Widget.Access.sendConfirmationEmail(areaId, email, dynamicResponses, keyvalues, convoId);
        if (data) {
            var completeConvo = assembleCompletedConvo(convoId, areaId, name, email, phone);
            await client.Widget.Access.postCompleteConversation(completeConvo);
        }
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
                            onClick={() => {
                                sendEmail();
                                responseAction(node, child, nodeList, client, convoId, null, convoContext);
                                toggleInputDisabled();
                            }}
                        />
                    </TableCell>
                </TableRow>
            </Table>
        );
    };

    // TODO: The backup here is and if else
    // if (res.success) { return SuccessComponent } else {return "Shall I retry ? component and execute that pathway."}

    return SuccessComponent;
};
