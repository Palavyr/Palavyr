import * as React from 'react';


interface Props {
    defaultOpen?: boolean;
}
export const EditorHelp = ({defaultOpen = false}: Props) => {

    return (
        <div>
            Thank you for checking out the help! This is a work in progress and will be availble very soon!
        </div>
    )
}