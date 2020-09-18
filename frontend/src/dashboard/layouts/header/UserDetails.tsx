import * as React from 'react';
import { LocalStorage } from 'localStorage/localStorage';


export const UserDetails = () => {

    const email = LocalStorage.getEmailAddress();

    return (
        <span>Logged in as: {email}</span>
    )
}