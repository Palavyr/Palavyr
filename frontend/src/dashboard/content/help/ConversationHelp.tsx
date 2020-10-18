import { Statement } from '@common/components/Statement'
import { Alert, AlertTitle } from '@material-ui/lab'
import React from 'react'

interface Props {
    defaultOpen?: boolean;
}
export const ConversationHelp = ({ defaultOpen = false }: Props) => {

    return (

        <Statement title="Conversation Configuration" defaultOpen={defaultOpen}>
            <>
                <span>
                    This section is used to configure the conversations your users will follow when using your chat customized chat widget! This section will explain how
                    to use the conversation editor.
                    </span>
                <span>
                    <Alert severity="info">
                        <AlertTitle><strong>Top Tip!</strong></AlertTitle>
                            Plan your conversation using tools such as Whimsical (https://whimsical.com) to ease the configuration process!
                        </Alert>
                </span>
                <span>
                    <h4>Nodes</h4>
                    <p>
                        Conversations are nothing more than a series of connected conversation nodes, where each node represents a branch point. Different types of nodes
                        might have different numbers of branches. For example, a 'yes or no' node has two branches, one for yes, and one for no. Alternatively
                        a node that presents information might have a single branch that continues directly to the next node.
                        </p>
                        When configuring a conversation, you will create a sequence of nodes that branch out wider and wider the more questions you ask.
                    </span>
                <span>
                    <h4>Node Editor</h4>
                        Nodes will typically require you to first provide a short amount of information to the user. For example, a question. You can click the top text box on the node
                        to edit the text to be presented to the end user. We don't currently support richly formatted text, however paragraph breaks are supported.
                    </span>
                <p>
                    <h4>Node Type Selector</h4>
                    <span>
                        There are many types of conversation nodes you can select from, but in general, there are only two basic types: single choice and multiple choice.
                        </span>
                    <span>
                        <h5>Single Choice</h5>
                            These types of nodes lead directly to another node. You might provide information in reponse to a previous selection, or you might take
                            a standard bit of information from the user. Regardless, these types of nodes always lead directly to another node.

                            <h5>Multiple choice</h5>
                            Multiple choice nodes may either lead directly to the next node, or may produce a branch point. For example, a 'yes or no' type node is a Multiple
                            choice node that produces two branches (one for yes, and one for no). This particular choice is an example of a predefined multiple choice option.
                            Other predefined multiple choice options include:
                            <ul>
                            <li>Yes / No or Not sure</li>
                            <li>Yes / No / Not sure</li>
                            <li>Yes or Not sure / No</li>
                        </ul>
                            If you wish to populate your own options, there are generic multiple choice node type options available.
                        </span>
                    <span>
                        <h4>Including conversation responese in your PDF response</h4>
                            You may wish to include certain replies from users in their PDF response (attached to the response email). To enable this, simple select the checkbox
                            labeled 'Show response in PDF' in the node.
                        </span>
                </p>
            </>
        </Statement>

    )
}