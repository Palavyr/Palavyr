import React from 'react'

interface IHelpDetails {
    children: React.ReactNode;
}
export const HelpDetails = ({children}: IHelpDetails) => {

    return (
        <>
            {children}
        </>
    )
}