import { Statement } from '@common/components/Statement'
import React from 'react'

interface Props {
    defaultOpen?: boolean;
}

export const PreviewHelp = ({defaultOpen = false}: Props) => {

    return (
        <Statement title="Response Preview" defaultOpen={defaultOpen}>
            <>
                <span>This is your response preview! </span>
                <span>
                    The PDF displayed below is a preview of what your customers will see attached with the email response sent after completing the conversation.
                    The preview reflects the current state of your configuration and can be broken down into two main sections.
                </span>
                <span>
                    <h4>Header Section</h4>
                    The header section displays your company's information, your logo (if you've provided on) as well as the responses to questions (conversation nodes)
                    marked in the conversation configuration tree.
                </span>
                    <span>
                        <h4>Body Section</h4>
                    The body section contains the response configration including the intro statement, outro statement, and fee tables.
                </span>
            </>
        </Statement>
    )
}