import { Statement } from '@common/components/Statement'
import React from 'react'

interface Props {
    defaultOpen?: boolean;
}

export const EmailHelp = ({defaultOpen = false}: Props) => {

    return (
        <Statement title="Response Email Configuration"  defaultOpen={defaultOpen} >
            <>
                <span>This section is used to configure the email used to send the PDF response. What you see below is what your users will see in their inbox. </span>
                <span>There are two ways to configure your response emails: </span>
                <ol>
                    <li>Upload an html template you prepared earlier</li>
                    <li>Use our inline editor</li>
                </ol>
                <span>
                    <h4>Upload</h4>
                To upload a precreated email template, you simply need to use the upload dialog to select your email html file. We currently only support html formats for uploads.
            </span>
                <span>
                    <h4>Inline Editor</h4>
                If you don't have a prepared html email template, you can use our inline editor. This provides functionality to fully customize your email response in rich text.
                Behind the scenes, this editor will convert your formatted text to html.
            </span>
            </>
        </Statement>
    )
}