import { Statement } from '@common/components/Statement'
import React from 'react'


interface Props {
    defaultOpen?: boolean;
}


export const AttachmentsHelp = ({defaultOpen = false}: Props) => {

    return (
        <Statement title="Response Attachments"  defaultOpen={defaultOpen}>
            <>
                <span>This section is used to upload PDF attachments that will be sent alongside the pdf response from this area. </span>
                <span>
                    <h4>Uploads</h4>
                We currently only support uploading PDF attachments under 2MB. You can use as many pages as you'd like per PDF.
                (So for example, if you're limited to a single attachment, you <i>could</i> combine all of your attachments into a single file. Just sayin'. ;) )
                <h4>Usage</h4>
                PDF attachments might typically be used to provide standard literature you send to your potential customers. For example, you might include standard forms,
                company background information, or legal information regarding the area of inquiry.
            </span>
            </>
        </Statement>
    )
}