import React from 'react'

interface Props {
    defaultOpen?: boolean;
}

export const AreaSettingsHelp = ({defaultOpen = false}: Props) => {

    return (
        <div>
            <h2>Area Settings Help!</h2>
        </div>
    )
}